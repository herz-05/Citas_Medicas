import { Component, OnInit, OnDestroy, signal, effect, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ExpedientesClinicosService, ExpedienteClinico } from '../../../core/services/MicroExpedientesClinicos/expediente-clinico';
import { BusquedaService, SugerenciaBusqueda } from '../../../core/services/Compartidos/busqueda';
import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';

@Component({
  selector: 'app-expedientes-clinicos',
  standalone: true,
  imports: [CommonModule, FormsModule, BotonesAcciones, Paginacion],
  templateUrl: './expediente-clinico.html',
  styleUrl: './expediente-clinico.css'
})
export class ExpedientesClinicos implements OnInit, OnDestroy {
  // DATOS Y PAGINACIÓN
  expedientes = signal<ExpedienteClinico[]>([]);
  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  // TEMPORIZADOR DE PREDICCIONES
  private temporizadorPredicciones: ReturnType<typeof setTimeout> | null = null;

  // FORMULARIO Y ESTADOS
  mostrarFormulario = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';
  expedienteSeleccionado: ExpedienteClinico | null = null;
  nuevoExpediente: ExpedienteClinico = this.crearExpedienteVacio();

  constructor(
    private expedientesService: ExpedientesClinicosService,
    private busquedaService: BusquedaService
  ) {
    // BÚSQUEDA CONFIRMADA
    effect(() => {
      this.busquedaService.texto();

      untracked(() => {
        this.paginaActual = 1;
        this.cargarExpedientes();
      });
    });

    // PREDICCIONES CON DEBOUNCE
    effect(() => {
      const texto = this.busquedaService.entrada();

      if (this.temporizadorPredicciones) {
        clearTimeout(this.temporizadorPredicciones);
      }

      if (texto.trim().length < 2) {
        this.busquedaService.establecerSugerencias([]);
        return;
      }

      this.temporizadorPredicciones = setTimeout(() => {
        this.cargarPredicciones(texto);
      }, 350);
    });
  }

  ngOnInit(): void {
    this.busquedaService.configurar('Buscar expedientes...');
  }

  ngOnDestroy(): void {
    if (this.temporizadorPredicciones) {
      clearTimeout(this.temporizadorPredicciones);
    }
    this.busquedaService.establecerSugerencias([]);
  }

  // CARGAR EXPEDIENTES PAGINADOS
  cargarExpedientes(): void {
    const textoBusqueda = this.busquedaService.texto();
    const filtroSeleccionado = this.busquedaService.filtroSeleccionado();
    const filtro = filtroSeleccionado !== '' ? filtroSeleccionado : this.crearFiltroBusqueda(textoBusqueda);

    this.expedientesService.getExpedientes(this.paginaActual, this.tamanoPagina, filtro).subscribe({
      next: (resultado) => {
        this.expedientes.set(resultado.data);
        this.totalRegistros = resultado.totalRecords;
        this.totalPaginas = resultado.totalPages;
        this.paginaActual = resultado.currentPage;
      },
      error: (error) => console.error('Error cargando expedientes:', error)
    });
  }

  // CARGAR PREDICCIONES DE BÚSQUEDA
  private cargarPredicciones(texto: string): void {
    const filtro = this.crearFiltroBusqueda(texto);

    if (filtro === '') {
      this.busquedaService.establecerSugerencias([]);
      return;
    }

    this.expedientesService.getExpedientes(1, 5, filtro).subscribe({
      next: (resultado) => {
        if (this.busquedaService.entrada().trim() !== texto.trim()) {
          return;
        }

        const sugerencias: SugerenciaBusqueda[] = resultado.data.map((expediente) => ({
          texto: `Expediente #${expediente.idExpedienteClinico}`,
          descripcion: `Paciente #${expediente.idPaciente}`,
          filtro: `IdExpedienteClinico == ${expediente.idExpedienteClinico}`
        }));

        this.busquedaService.establecerSugerencias(sugerencias);
      },
      error: (error) => {
        console.error('Error cargando predicciones:', error);
        this.busquedaService.establecerSugerencias([]);
      }
    });
  }

  // CREAR FILTRO DE BÚSQUEDA DINÁMICO
  private crearFiltroBusqueda(texto: string): string {
    const valor = texto.trim();
    if (valor === '') return '';

    const palabras = valor.split(/\s+/).filter(palabra => palabra.trim() !== '');

    const filtros = palabras.map(palabra => {
      const palabraSegura = palabra.replace(/\\/g, '\\\\').replace(/"/g, '\\"');
      return `(Estado.Contains("${palabraSegura}") || Observaciones.Contains("${palabraSegura}"))`;
    });

    return filtros.join(' && ');
  }

  // ACCIONES DEL MODAL
  abrirNuevoExpediente(): void {
    this.modoFormulario = 'nuevo';
    this.expedienteSeleccionado = null;
    this.nuevoExpediente = this.crearExpedienteVacio();
    this.mostrarFormulario = true;
  }

  verExpediente(expediente: ExpedienteClinico): void {
    this.modoFormulario = 'ver';
    this.expedienteSeleccionado = expediente;
    this.nuevoExpediente = this.copiarExpediente(expediente);
    this.mostrarFormulario = true;
  }

  editarExpediente(expediente: ExpedienteClinico): void {
    this.modoFormulario = 'editar';
    this.expedienteSeleccionado = expediente;
    this.nuevoExpediente = this.copiarExpediente(expediente);
    this.mostrarFormulario = true;
  }

  eliminarExpediente(expediente: ExpedienteClinico): void {
    if (!confirm(`¿Desea eliminar el expediente #${expediente.idExpedienteClinico}?`)) return;

    this.expedientesService.deleteExpediente(expediente.idExpedienteClinico).subscribe({
      next: () => {
        if (this.expedientes().length === 1 && this.paginaActual > 1) {
          this.paginaActual--;
        }
        this.cargarExpedientes();
      },
      error: (error) => console.error('Error eliminando expediente:', error)
    });
  }

  guardarExpediente(): void {
    if (this.modoFormulario === 'nuevo') {
      this.agregarExpediente();
      return;
    }
    if (this.modoFormulario === 'editar') {
      this.actualizarExpediente();
    }
  }

  private agregarExpediente(): void {
    this.expedientesService.addExpediente(this.nuevoExpediente).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.paginaActual = 1;
        this.cargarExpedientes();
      },
      error: (error) => console.error('Error agregando expediente:', error)
    });
  }

  private actualizarExpediente(): void {
    this.expedientesService.updateExpediente(this.nuevoExpediente).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.cargarExpedientes();
      },
      error: (error) => console.error('Error actualizando expediente:', error)
    });
  }

  cerrarFormulario(): void {
    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.expedienteSeleccionado = null;
    this.limpiarFormulario();
  }

  limpiarFormulario(): void {
    this.nuevoExpediente = this.crearExpedienteVacio();
  }

  // PAGINACIÓN
  cambiarPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas || pagina === this.paginaActual) return;
    this.paginaActual = pagina;
    this.cargarExpedientes();
  }

  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = tamano;
    this.paginaActual = 1;
    this.cargarExpedientes();
  }

  // COPIA PARA EDITAR (la fecha se recorta a yyyy-MM-dd para el input date)
  private copiarExpediente(expediente: ExpedienteClinico): ExpedienteClinico {
    return {
      ...expediente,
      fechaApertura: expediente.fechaApertura ? expediente.fechaApertura.substring(0, 10) : ''
    };
  }

  // MODELO VACÍO
  private crearExpedienteVacio(): ExpedienteClinico {
    return {
      idExpedienteClinico: 0,
      idPaciente: 0,
      fechaApertura: new Date().toISOString().substring(0, 10),
      estado: 'Activo',
      observaciones: ''
    };
  }
}
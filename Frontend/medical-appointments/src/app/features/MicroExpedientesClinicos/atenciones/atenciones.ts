import { Component, OnInit, OnDestroy, signal, effect, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AtencionesService, Atencion } from '../../../core/services/MicroExpedientesClinicos/atenciones';
import { BusquedaService, SugerenciaBusqueda } from '../../../core/services/Compartidos/busqueda';
import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';

@Component({
  selector: 'app-atenciones',
  standalone: true,
  imports: [CommonModule, FormsModule, BotonesAcciones, Paginacion],
  templateUrl: './atenciones.html',
  styleUrl: './atenciones.css'
})
export class Atenciones implements OnInit, OnDestroy {
  // DATOS Y PAGINACIÓN
  atenciones = signal<Atencion[]>([]);
  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  // TEMPORIZADOR DE PREDICCIONES
  private temporizadorPredicciones: ReturnType<typeof setTimeout> | null = null;

  // FORMULARIO Y ESTADOS
  mostrarFormulario = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';
  atencionSeleccionada: Atencion | null = null;
  nuevoAtencion: Atencion = this.crearAtencionVacio();

  constructor(
    private atencionesService: AtencionesService,
    private busquedaService: BusquedaService
  ) {
    // BÚSQUEDA CONFIRMADA
    effect(() => {
      this.busquedaService.texto();

      untracked(() => {
        this.paginaActual = 1;
        this.cargarAtenciones();
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
    this.busquedaService.configurar('Buscar atenciones...');
  }

  ngOnDestroy(): void {
    if (this.temporizadorPredicciones) {
      clearTimeout(this.temporizadorPredicciones);
    }
    this.busquedaService.establecerSugerencias([]);
  }

  // CARGAR EXPEDIENTES PAGINADOS
  cargarAtenciones(): void {
    const textoBusqueda = this.busquedaService.texto();
    const filtroSeleccionado = this.busquedaService.filtroSeleccionado();
    const filtro = filtroSeleccionado !== '' ? filtroSeleccionado : this.crearFiltroBusqueda(textoBusqueda);

    this.atencionesService.getAtenciones(this.paginaActual, this.tamanoPagina, filtro).subscribe({
      next: (resultado) => {
        this.atenciones.set(resultado.data);
        this.totalRegistros = resultado.totalRecords;
        this.totalPaginas = resultado.totalPages;
        this.paginaActual = resultado.currentPage;
      },
      error: (error) => console.error('Error cargando atenciones:', error)
    });
  }

  // CARGAR PREDICCIONES DE BÚSQUEDA
  private cargarPredicciones(texto: string): void {
    const filtro = this.crearFiltroBusqueda(texto);

    if (filtro === '') {
      this.busquedaService.establecerSugerencias([]);
      return;
    }

    this.atencionesService.getAtenciones(1, 5, filtro).subscribe({
      next: (resultado) => {
        if (this.busquedaService.entrada().trim() !== texto.trim()) {
          return;
        }

        const sugerencias: SugerenciaBusqueda[] = resultado.data.map((atencion) => ({
          texto: `Atención #${atencion.idAtencion}`,
          descripcion: atencion.motivoConsulta,
          filtro: `IdAtencion == ${atencion.idAtencion}`
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
      return `(MotivoConsulta.Contains("${palabraSegura}") || Observaciones.Contains("${palabraSegura}"))`;
    });

    return filtros.join(' && ');
  }

  // ACCIONES DEL MODAL
  abrirNuevoAtencion(): void {
    this.modoFormulario = 'nuevo';
    this.atencionSeleccionada = null;
    this.nuevoAtencion = this.crearAtencionVacio();
    this.mostrarFormulario = true;
  }

  verAtencion(atencion: Atencion): void {
    this.modoFormulario = 'ver';
    this.atencionSeleccionada = atencion;
    this.nuevoAtencion = this.copiarAtencion(atencion);
    this.mostrarFormulario = true;
  }

  editarAtencion(atencion: Atencion): void {
    this.modoFormulario = 'editar';
    this.atencionSeleccionada = atencion;
    this.nuevoAtencion = this.copiarAtencion(atencion);
    this.mostrarFormulario = true;
  }

  eliminarAtencion(atencion: Atencion): void {
    if (!confirm(`¿Desea eliminar la atención #${atencion.idAtencion}?`)) return;

    this.atencionesService.deleteAtencion(atencion.idAtencion).subscribe({
      next: () => {
        if (this.atenciones().length === 1 && this.paginaActual > 1) {
          this.paginaActual--;
        }
        this.cargarAtenciones();
      },
      error: (error) => console.error('Error eliminando atencion:', error)
    });
  }

  guardarAtencion(): void {
    if (this.modoFormulario === 'nuevo') {
      this.agregarAtencion();
      return;
    }
    if (this.modoFormulario === 'editar') {
      this.actualizarAtencion();
    }
  }

  private agregarAtencion(): void {
    this.atencionesService.addAtencion(this.nuevoAtencion).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.paginaActual = 1;
        this.cargarAtenciones();
      },
      error: (error) => console.error('Error agregando atencion:', error)
    });
  }

  private actualizarAtencion(): void {
    this.atencionesService.updateAtencion(this.nuevoAtencion).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.cargarAtenciones();
      },
      error: (error) => console.error('Error actualizando atencion:', error)
    });
  }

  cerrarFormulario(): void {
    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.atencionSeleccionada = null;
    this.limpiarFormulario();
  }

  limpiarFormulario(): void {
    this.nuevoAtencion = this.crearAtencionVacio();
  }

  // PAGINACIÓN
  cambiarPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas || pagina === this.paginaActual) return;
    this.paginaActual = pagina;
    this.cargarAtenciones();
  }

  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = tamano;
    this.paginaActual = 1;
    this.cargarAtenciones();
  }

  // COPIA PARA EDITAR (la fecha se recorta a yyyy-MM-dd para el input date)
  private copiarAtencion(atencion: Atencion): Atencion {
    return {
      ...atencion,
      fechaAtencion: atencion.fechaAtencion ? atencion.fechaAtencion.substring(0, 10) : ''
    };
  }

  // MODELO VACÍO
  private crearAtencionVacio(): Atencion {
    return {
      idAtencion: 0,
      idExpedienteClinico: 0,
      idMedico: 0,
      fechaAtencion: new Date().toISOString().substring(0, 10),
      motivoConsulta: '',
      observaciones: ''
    };
  }
}
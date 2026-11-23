import { Component, OnInit, OnDestroy, signal, effect, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { DiagnosticosService, Diagnostico } from '../../../core/services/MicroExpedientesClinicos/diagnosticos';
import { BusquedaService, SugerenciaBusqueda } from '../../../core/services/Compartidos/busqueda';
import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';

@Component({
  selector: 'app-diagnosticos',
  standalone: true,
  imports: [CommonModule, FormsModule, BotonesAcciones, Paginacion],
  templateUrl: './diagnosticos.html',
  styleUrl: './diagnosticos.css'
})
export class Diagnosticos implements OnInit, OnDestroy {
  // DATOS Y PAGINACIÓN
  diagnosticos = signal<Diagnostico[]>([]);
  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  // TEMPORIZADOR DE PREDICCIONES
  private temporizadorPredicciones: ReturnType<typeof setTimeout> | null = null;

  // FORMULARIO Y ESTADOS
  mostrarFormulario = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';
  diagnosticoSeleccionado: Diagnostico | null = null;
  nuevoDiagnostico: Diagnostico = this.crearDiagnosticoVacio();

  constructor(
    private diagnosticosService: DiagnosticosService,
    private busquedaService: BusquedaService
  ) {
    // BÚSQUEDA CONFIRMADA
    effect(() => {
      this.busquedaService.texto();

      untracked(() => {
        this.paginaActual = 1;
        this.cargarDiagnosticos();
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
    this.busquedaService.configurar('Buscar diagnosticos...');
  }

  ngOnDestroy(): void {
    if (this.temporizadorPredicciones) {
      clearTimeout(this.temporizadorPredicciones);
    }
    this.busquedaService.establecerSugerencias([]);
  }

  // CARGAR EXPEDIENTES PAGINADOS
  cargarDiagnosticos(): void {
    const textoBusqueda = this.busquedaService.texto();
    const filtroSeleccionado = this.busquedaService.filtroSeleccionado();
    const filtro = filtroSeleccionado !== '' ? filtroSeleccionado : this.crearFiltroBusqueda(textoBusqueda);

    this.diagnosticosService.getDiagnosticos(this.paginaActual, this.tamanoPagina, filtro).subscribe({
      next: (resultado) => {
        this.diagnosticos.set(resultado.data);
        this.totalRegistros = resultado.totalRecords;
        this.totalPaginas = resultado.totalPages;
        this.paginaActual = resultado.currentPage;
      },
      error: (error) => console.error('Error cargando diagnosticos:', error)
    });
  }

  // CARGAR PREDICCIONES DE BÚSQUEDA
  private cargarPredicciones(texto: string): void {
    const filtro = this.crearFiltroBusqueda(texto);

    if (filtro === '') {
      this.busquedaService.establecerSugerencias([]);
      return;
    }

    this.diagnosticosService.getDiagnosticos(1, 5, filtro).subscribe({
      next: (resultado) => {
        if (this.busquedaService.entrada().trim() !== texto.trim()) {
          return;
        }

        const sugerencias: SugerenciaBusqueda[] = resultado.data.map((diagnostico) => ({
          texto: diagnostico.nombre,
          descripcion: diagnostico.codigoCie10,
          filtro: `IdDiagnostico == ${diagnostico.idDiagnostico}`
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
      return `(CodigoCie10.Contains("${palabraSegura}") || Nombre.Contains("${palabraSegura}") || Descripcion.Contains("${palabraSegura}"))`;
    });

    return filtros.join(' && ');
  }

  // ACCIONES DEL MODAL
  abrirNuevoDiagnostico(): void {
    this.modoFormulario = 'nuevo';
    this.diagnosticoSeleccionado = null;
    this.nuevoDiagnostico = this.crearDiagnosticoVacio();
    this.mostrarFormulario = true;
  }

  verDiagnostico(diagnostico: Diagnostico): void {
    this.modoFormulario = 'ver';
    this.diagnosticoSeleccionado = diagnostico;
    this.nuevoDiagnostico = { ...diagnostico };
    this.mostrarFormulario = true;
  }

  editarDiagnostico(diagnostico: Diagnostico): void {
    this.modoFormulario = 'editar';
    this.diagnosticoSeleccionado = diagnostico;
    this.nuevoDiagnostico = { ...diagnostico };
    this.mostrarFormulario = true;
  }

  eliminarDiagnostico(diagnostico: Diagnostico): void {
    if (!confirm(`¿Desea eliminar el diagnóstico ${diagnostico.nombre}?`)) return;

    this.diagnosticosService.deleteDiagnostico(diagnostico.idDiagnostico).subscribe({
      next: () => {
        if (this.diagnosticos().length === 1 && this.paginaActual > 1) {
          this.paginaActual--;
        }
        this.cargarDiagnosticos();
      },
      error: (error) => console.error('Error eliminando diagnostico:', error)
    });
  }

  guardarDiagnostico(): void {
    if (this.modoFormulario === 'nuevo') {
      this.agregarDiagnostico();
      return;
    }
    if (this.modoFormulario === 'editar') {
      this.actualizarDiagnostico();
    }
  }

  private agregarDiagnostico(): void {
    this.diagnosticosService.addDiagnostico(this.nuevoDiagnostico).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.paginaActual = 1;
        this.cargarDiagnosticos();
      },
      error: (error) => console.error('Error agregando diagnostico:', error)
    });
  }

  private actualizarDiagnostico(): void {
    this.diagnosticosService.updateDiagnostico(this.nuevoDiagnostico).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.cargarDiagnosticos();
      },
      error: (error) => console.error('Error actualizando diagnostico:', error)
    });
  }

  cerrarFormulario(): void {
    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.diagnosticoSeleccionado = null;
    this.limpiarFormulario();
  }

  limpiarFormulario(): void {
    this.nuevoDiagnostico = this.crearDiagnosticoVacio();
  }

  // PAGINACIÓN
  cambiarPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas || pagina === this.paginaActual) return;
    this.paginaActual = pagina;
    this.cargarDiagnosticos();
  }

  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = tamano;
    this.paginaActual = 1;
    this.cargarDiagnosticos();
  }

  // MODELO VACÍO
  private crearDiagnosticoVacio(): Diagnostico {
    return {
      idDiagnostico: 0,
      codigoCie10: '',
      nombre: '',
      descripcion: ''
    };
  }
}
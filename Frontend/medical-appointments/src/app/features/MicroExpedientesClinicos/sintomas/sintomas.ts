import { Component, OnInit, OnDestroy, signal, effect, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { SintomasService, Sintoma } from '../../../core/services/MicroExpedientesClinicos/sintomas';
import { BusquedaService, SugerenciaBusqueda } from '../../../core/services/Compartidos/busqueda';
import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';

@Component({
  selector: 'app-sintomas',
  standalone: true,
  imports: [CommonModule, FormsModule, BotonesAcciones, Paginacion],
  templateUrl: './sintomas.html',
  styleUrl: './sintomas.css'
})
export class Sintomas implements OnInit, OnDestroy {
  // DATOS Y PAGINACIÓN
  sintomas = signal<Sintoma[]>([]);
  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  // TEMPORIZADOR DE PREDICCIONES
  private temporizadorPredicciones: ReturnType<typeof setTimeout> | null = null;

  // FORMULARIO Y ESTADOS
  mostrarFormulario = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';
  sintomaSeleccionado: Sintoma | null = null;
  nuevoSintoma: Sintoma = this.crearSintomaVacio();

  constructor(
    private sintomasService: SintomasService,
    private busquedaService: BusquedaService
  ) {
    // BÚSQUEDA CONFIRMADA
    effect(() => {
      this.busquedaService.texto();

      untracked(() => {
        this.paginaActual = 1;
        this.cargarSintomas();
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
    this.busquedaService.configurar('Buscar sintomas...');
  }

  ngOnDestroy(): void {
    if (this.temporizadorPredicciones) {
      clearTimeout(this.temporizadorPredicciones);
    }
    this.busquedaService.establecerSugerencias([]);
  }

  // CARGAR EXPEDIENTES PAGINADOS
  cargarSintomas(): void {
    const textoBusqueda = this.busquedaService.texto();
    const filtroSeleccionado = this.busquedaService.filtroSeleccionado();
    const filtro = filtroSeleccionado !== '' ? filtroSeleccionado : this.crearFiltroBusqueda(textoBusqueda);

    this.sintomasService.getSintomas(this.paginaActual, this.tamanoPagina, filtro).subscribe({
      next: (resultado) => {
        this.sintomas.set(resultado.data);
        this.totalRegistros = resultado.totalRecords;
        this.totalPaginas = resultado.totalPages;
        this.paginaActual = resultado.currentPage;
      },
      error: (error) => console.error('Error cargando sintomas:', error)
    });
  }

  // CARGAR PREDICCIONES DE BÚSQUEDA
  private cargarPredicciones(texto: string): void {
    const filtro = this.crearFiltroBusqueda(texto);

    if (filtro === '') {
      this.busquedaService.establecerSugerencias([]);
      return;
    }

    this.sintomasService.getSintomas(1, 5, filtro).subscribe({
      next: (resultado) => {
        if (this.busquedaService.entrada().trim() !== texto.trim()) {
          return;
        }

        const sugerencias: SugerenciaBusqueda[] = resultado.data.map((sintoma) => ({
          texto: sintoma.nombre,
          descripcion: sintoma.descripcion,
          filtro: `IdSintoma == ${sintoma.idSintoma}`
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
      return `(Nombre.Contains("${palabraSegura}") || Descripcion.Contains("${palabraSegura}"))`;
    });

    return filtros.join(' && ');
  }

  // ACCIONES DEL MODAL
  abrirNuevoSintoma(): void {
    this.modoFormulario = 'nuevo';
    this.sintomaSeleccionado = null;
    this.nuevoSintoma = this.crearSintomaVacio();
    this.mostrarFormulario = true;
  }

  verSintoma(sintoma: Sintoma): void {
    this.modoFormulario = 'ver';
    this.sintomaSeleccionado = sintoma;
    this.nuevoSintoma = { ...sintoma };
    this.mostrarFormulario = true;
  }

  editarSintoma(sintoma: Sintoma): void {
    this.modoFormulario = 'editar';
    this.sintomaSeleccionado = sintoma;
    this.nuevoSintoma = { ...sintoma };
    this.mostrarFormulario = true;
  }

  eliminarSintoma(sintoma: Sintoma): void {
    if (!confirm(`¿Desea eliminar el síntoma ${sintoma.nombre}?`)) return;

    this.sintomasService.deleteSintoma(sintoma.idSintoma).subscribe({
      next: () => {
        if (this.sintomas().length === 1 && this.paginaActual > 1) {
          this.paginaActual--;
        }
        this.cargarSintomas();
      },
      error: (error) => console.error('Error eliminando sintoma:', error)
    });
  }

  guardarSintoma(): void {
    if (this.modoFormulario === 'nuevo') {
      this.agregarSintoma();
      return;
    }
    if (this.modoFormulario === 'editar') {
      this.actualizarSintoma();
    }
  }

  private agregarSintoma(): void {
    this.sintomasService.addSintoma(this.nuevoSintoma).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.paginaActual = 1;
        this.cargarSintomas();
      },
      error: (error) => console.error('Error agregando sintoma:', error)
    });
  }

  private actualizarSintoma(): void {
    this.sintomasService.updateSintoma(this.nuevoSintoma).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.cargarSintomas();
      },
      error: (error) => console.error('Error actualizando sintoma:', error)
    });
  }

  cerrarFormulario(): void {
    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.sintomaSeleccionado = null;
    this.limpiarFormulario();
  }

  limpiarFormulario(): void {
    this.nuevoSintoma = this.crearSintomaVacio();
  }

  // PAGINACIÓN
  cambiarPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas || pagina === this.paginaActual) return;
    this.paginaActual = pagina;
    this.cargarSintomas();
  }

  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = tamano;
    this.paginaActual = 1;
    this.cargarSintomas();
  }

  // MODELO VACÍO
  private crearSintomaVacio(): Sintoma {
    return {
      idSintoma: 0,
      nombre: '',
      descripcion: ''
    };
  }
}
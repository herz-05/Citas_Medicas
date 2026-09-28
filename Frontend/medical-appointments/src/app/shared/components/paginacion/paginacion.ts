import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-paginacion',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './paginacion.html',
  styleUrl: './paginacion.css'
})
export class Paginacion {
  // DATOS RECIBIDOS
  @Input() paginaActual = 1;
  @Input() tamanoPagina = 10;
  @Input() totalRegistros = 0;
  @Input() totalPaginas = 0;
  @Input() nombreRegistros = 'registros';
  @Input() opcionesTamano: number[] = [5, 10, 20];

  // EVENTOS DE CAMBIO
  @Output() paginaCambiada = new EventEmitter<number>();
  @Output() tamanoPaginaCambiado = new EventEmitter<number>();

  // NAVEGAR A LA PÁGINA ANTERIOR
  paginaAnterior(): void {
    if (this.paginaActual <= 1) return;
    this.paginaCambiada.emit(this.paginaActual - 1);
  }

  // NAVEGAR A LA PÁGINA SIGUIENTE
  paginaSiguiente(): void {
    if (this.paginaActual >= this.totalPaginas) return;
    this.paginaCambiada.emit(this.paginaActual + 1);
  }

  // CAMBIAR A UNA PÁGINA ESPECÍFICA
  cambiarPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas || pagina === this.paginaActual) return;
    this.paginaCambiada.emit(pagina);
  }

  // CAMBIAR EL TAMAÑO DE PÁGINA (REGISTROS POR VISTA)
  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPaginaCambiado.emit(Number(tamano));
  }

  // OBTENER LISTA DE PÁGINAS DISPONIBLES
  obtenerPaginas(): number[] {
    if (this.totalPaginas <= 0) return [];
    return Array.from({ length: this.totalPaginas }, (_, indice) => indice + 1);
  }

  // CALCULAR EL REGISTRO INICIAL MOSTRADO EN LA TABLA
  obtenerRegistroInicial(): number {
    if (this.totalRegistros === 0) return 0;
    return ((this.paginaActual - 1) * this.tamanoPagina) + 1;
  }

  // CALCULAR EL REGISTRO FINAL MOSTRADO EN LA TABLA
  obtenerRegistroFinal(): number {
    if (this.totalRegistros === 0) return 0;
    return Math.min(this.paginaActual * this.tamanoPagina, this.totalRegistros);
  }
}
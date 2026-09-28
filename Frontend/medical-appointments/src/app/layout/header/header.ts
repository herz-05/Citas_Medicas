import { Component, Input } from '@angular/core';
import { BusquedaService, SugerenciaBusqueda } from '../../core/services/busqueda';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header {
  // SIDEBAR
  @Input() sidebarColapsado = false;

  constructor(
    public busquedaService: BusquedaService
  ) {}

  // ESCRIBIR EN EL BUSCADOR
  onEscribir(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.busquedaService.escribir(input.value);
  }

  // CONFIRMAR CON ENTER
  onEnter(): void {
    this.realizarBusqueda();
  }

  // CLIC EN EL BOTÓN DE LUPA
  onClickBuscar(): void {
    this.realizarBusqueda();
  }

  // REALIZAR BÚSQUEDA GLOBAL
  private realizarBusqueda(): void {
    this.busquedaService.buscar();
  }

  // SELECCIONAR UNA SUGERENCIA DE LA PREDICCIÓN
  seleccionarSugerencia(sugerencia: SugerenciaBusqueda): void {
    this.busquedaService.seleccionarSugerencia(sugerencia);
  }

  // FOCO EN EL INPUT
  onFocus(): void {
    if (this.busquedaService.sugerencias().length > 0) {
      this.busquedaService.mostrarSugerencias.set(true);
    }
  }
}
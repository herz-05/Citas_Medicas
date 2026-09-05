import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css'
})
export class Sidebar {

  @Input() colapsado = false;
  @Output() cambiarEstado = new EventEmitter<void>();

  pacientesAbierto = false;
  identidadAbierto = false;

  menuFlotante: string | null = null;

  toggleSidebar(): void {
    this.cambiarEstado.emit();
    this.menuFlotante = null;
  }

  togglePacientes(): void {
    if (this.colapsado) {
      this.menuFlotante =
        this.menuFlotante === 'pacientes'
          ? null
          : 'pacientes';
    } else {
      this.pacientesAbierto = !this.pacientesAbierto;
    }
  }

  toggleIdentidad(): void {
    if (this.colapsado) {
      this.menuFlotante =
        this.menuFlotante === 'identidad'
          ? null
          : 'identidad';
    } else {
      this.identidadAbierto = !this.identidadAbierto;
    }
  }

  abrirFlotante(nombre: string): void {
    if (!this.colapsado) {
      return;
    }

    this.menuFlotante =
      this.menuFlotante === nombre
        ? null
        : nombre;
  }

  cerrarFlotante(): void {
    this.menuFlotante = null;
  }
}
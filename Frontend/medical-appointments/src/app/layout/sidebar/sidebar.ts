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

  toggleSidebar(): void {
    this.cambiarEstado.emit();
  }
}
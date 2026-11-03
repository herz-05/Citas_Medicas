import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  Output
} from '@angular/core';

@Component({
  selector: 'app-botones-acciones',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './botones-acciones.html',
  styleUrl: './botones-acciones.css'
})
export class BotonesAcciones {

  @Input() mostrarVer = true;
  @Input() mostrarEditar = true;
  @Input() mostrarEliminar = true;
  @Input() mostrarAnular = false;
  @Input() deshabilitado = false;

  @Output() ver = new EventEmitter<void>();
  @Output() editar = new EventEmitter<void>();
  @Output() eliminar = new EventEmitter<void>();
  @Output() anular = new EventEmitter<void>();

  emitirVer(): void {
    if (this.deshabilitado) return;

    this.ver.emit();
  }

  emitirEditar(): void {
    if (this.deshabilitado) return;

    this.editar.emit();
  }

  emitirEliminar(): void {
    if (this.deshabilitado) return;

    this.eliminar.emit();
  }

  emitirAnular(): void {
    if (this.deshabilitado) return;

    this.anular.emit();
  }
}
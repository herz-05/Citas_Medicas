import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  PacientesService,
  Paciente
} from '../services/pacientes';

@Component({
  selector: 'app-pacientes',
  imports: [CommonModule],
  templateUrl: './pacientes.html',
  styleUrl: './pacientes.css'
})
export class Pacientes implements OnInit {

  pacientes = signal<Paciente[]>([]);

  constructor(private pacientesService: PacientesService) {}

  ngOnInit(): void {
    this.pacientesService.getPacientes().subscribe({
      next: (data) => {
        console.log('PACIENTES RECIBIDOS:', data);
        this.pacientes.set(data);
      },
      error: (error) => {
        console.error('Error cargando pacientes:', error);
      }
    });
  }
}
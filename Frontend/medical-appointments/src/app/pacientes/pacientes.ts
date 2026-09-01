import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  PacientesService,
  Paciente
} from '../services/pacientes';

@Component({
  selector: 'app-pacientes',
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './pacientes.html',
  styleUrl: './pacientes.css'
})
export class Pacientes implements OnInit {

  pacientes = signal<Paciente[]>([]);

  mostrarFormulario = false;

  nuevoPaciente: Paciente = {
    idPaciente: 0,
    nombres: '',
    apellidos: '',
    fechaNacimiento: '',
    sexo: '',
    dui: '',
    telefono: '',
    correo: '',
    direccion: '',
    fechaRegistro: new Date().toISOString()
  };

  constructor(
    private pacientesService: PacientesService
  ) {}

  ngOnInit(): void {
    this.cargarPacientes();
  }

  cargarPacientes(): void {
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

  agregarPaciente(): void {

    this.pacientesService.addPaciente(this.nuevoPaciente).subscribe({
      next: () => {

        console.log('Paciente agregado correctamente');

        this.mostrarFormulario = false;

        this.limpiarFormulario();

        this.cargarPacientes();
      },
      error: (error) => {
        console.error('Error agregando paciente:', error);
      }
    });
  }

  limpiarFormulario(): void {
    this.nuevoPaciente = {
      idPaciente: 0,
      nombres: '',
      apellidos: '',
      fechaNacimiento: '',
      sexo: '',
      dui: '',
      telefono: '',
      correo: '',
      direccion: '',
      fechaRegistro: new Date().toISOString()
    };
  }
}
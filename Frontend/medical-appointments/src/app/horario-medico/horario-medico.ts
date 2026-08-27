import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  HorarioMedicoService,
  HorarioMedico as HorarioMedicoModel
} from '../services/horario-medico';

@Component({
  selector: 'app-horario-medico',
  imports: [CommonModule],
  templateUrl: './horario-medico.html',
  styleUrl: './horario-medico.css'
})
export class HorarioMedico implements OnInit {

  horarios = signal<HorarioMedicoModel[]>([]);

  constructor(
    private horarioService: HorarioMedicoService
  ) {}

  ngOnInit(): void {
    this.horarioService.getHorarios().subscribe({
      next: (data) => {
        console.log('HORARIOS RECIBIDOS:', data);
        this.horarios.set(data);
      },
      error: (error) => {
        console.error('Error cargando horarios:', error);
      }
    });
  }
}
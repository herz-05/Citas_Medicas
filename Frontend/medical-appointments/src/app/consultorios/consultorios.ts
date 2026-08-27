import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ConsultoriosService,
  Consultorio
} from '../services/consultorios';

@Component({
  selector: 'app-consultorios',
  imports: [CommonModule],
  templateUrl: './consultorios.html',
  styleUrl: './consultorios.css'
})
export class Consultorios implements OnInit {

  consultorios = signal<Consultorio[]>([]);

  constructor(
    private consultoriosService: ConsultoriosService
  ) {}

  ngOnInit(): void {
    this.consultoriosService.getConsultorios().subscribe({
      next: (data) => {
        console.log('CONSULTORIOS RECIBIDOS:', data);
        this.consultorios.set(data);
      },
      error: (error) => {
        console.error('Error cargando consultorios:', error);
      }
    });
  }
}
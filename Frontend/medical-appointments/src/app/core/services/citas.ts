import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../generics/generic-crud.service';

// Modelo de datos para la entidad Cita
export interface Cita {
  idCita: number;
  idPaciente: number;
  idMedico: number;
  idConsultorio: number;
  idEstadoCita: number;
  fechaCita: string;
  horaInicio: string;
  horaFin: string;
  motivoConsulta: string | null;
  observaciones: string | null;
  fechaRegistro: string;
}

@Injectable({
  providedIn: 'root'
})
export class CitasService extends GenericCrudService<Cita> {
  constructor(http: HttpClient) {
    super(http, 'https://localhost:7250/Citas');
  }

  // Obtiene el listado paginado de citas
  getCitas(pageNumber: number = 1, pageSize: number = 10): Observable<PagedResult<Cita>> {
    return this.getPaged(pageNumber, pageSize);
  }

  // Obtiene una cita específica por su ID
  getCitaById(idCita: number): Observable<Cita> {
    return this.getById(idCita);
  }

  // Registra una nueva cita
  addCita(cita: Cita): Observable<boolean> {
    return this.add(cita);
  }

  // Actualiza una cita existente
  updateCita(cita: Cita): Observable<boolean> {
    return this.update(cita.idCita, cita);
  }

  // Elimina una cita por su ID
  deleteCita(idCita: number): Observable<boolean> {
    return this.delete(idCita);
  }
}
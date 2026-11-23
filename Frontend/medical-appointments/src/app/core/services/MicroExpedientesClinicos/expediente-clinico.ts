import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../../generics/generic-crud.service';

// Modelo de datos para la entidad ExpedienteClinico
export interface ExpedienteClinico {
  expedienteId: number;
  pacienteId: number;
  numeroExpediente: string;
  fechaApertura: string;
  estado: string;
  creadoEn: string;
  actualizadoEn: string;
}

@Injectable({
  providedIn: 'root'
})
export class ExpedientesClinicosService extends GenericCrudService<ExpedienteClinico> {
  constructor(http: HttpClient) {
    // Cambia el puerto por el de tu microservicio de expedientes
    super(http, 'http://localhost:5087/ExpedienteClinico');
  }

  // Obtiene el listado paginado de expedientes con opción de filtro
  getExpedientes(pageNumber: number = 1, pageSize: number = 10, filter: string = ''): Observable<PagedResult<ExpedienteClinico>> {
    return this.getPaged(pageNumber, pageSize, filter);
  }

  // Obtiene un expediente específico por su ID
  getExpedienteById(idExpedienteClinico: number): Observable<ExpedienteClinico> {
    return this.getById(idExpedienteClinico);
  }

  // Registra un nuevo expediente
  addExpediente(expediente: ExpedienteClinico): Observable<boolean> {
    return this.add(expediente);
  }

  // Actualiza un expediente existente
  updateExpediente(expediente: ExpedienteClinico): Observable<boolean> {
    return this.update(expediente.expedienteId, expediente);
  }

  // Elimina un expediente por su ID
  deleteExpediente(idExpedienteClinico: number): Observable<boolean> {
    return this.delete(idExpedienteClinico);
  }
}
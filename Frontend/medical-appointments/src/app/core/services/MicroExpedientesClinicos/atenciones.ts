import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../../generics/generic-crud.service';

// Modelo de datos para la entidad Atencion
export interface Atencion {
  idAtencion: number;
  idExpedienteClinico: number;
  idMedico: number;
  fechaAtencion: string;
  motivoConsulta: string;
  observaciones: string;
}

@Injectable({
  providedIn: 'root'
})
export class AtencionesService extends GenericCrudService<Atencion> {
  constructor(http: HttpClient) {
    super(http, 'http://localhost:5085/Atenciones');
  }

  // Obtiene el listado paginado de atenciones con opción de filtro
  getAtenciones(pageNumber: number = 1, pageSize: number = 10, filter: string = ''): Observable<PagedResult<Atencion>> {
    return this.getPaged(pageNumber, pageSize, filter);
  }

  // Obtiene una atención específica por su ID
  getAtencionById(idAtencion: number): Observable<Atencion> {
    return this.getById(idAtencion);
  }

  // Registra una nueva atención
  addAtencion(atencion: Atencion): Observable<boolean> {
    return this.add(atencion);
  }

  // Actualiza una atención existente
  updateAtencion(atencion: Atencion): Observable<boolean> {
    return this.update(atencion.idAtencion, atencion);
  }

  // Elimina una atención por su ID
  deleteAtencion(idAtencion: number): Observable<boolean> {
    return this.delete(idAtencion);
  }
}
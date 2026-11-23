import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../../generics/generic-crud.service';

// Modelo de datos para la entidad Diagnostico
export interface Diagnostico {
  idDiagnostico: number;
  codigoCie10: string;
  nombre: string;
  descripcion: string;
}

@Injectable({
  providedIn: 'root'
})
export class DiagnosticosService extends GenericCrudService<Diagnostico> {
  constructor(http: HttpClient) {
    super(http, 'http://localhost:5087/Diagnosticos');
  }

  // Obtiene el listado paginado de diagnósticos con opción de filtro
  getDiagnosticos(pageNumber: number = 1, pageSize: number = 10, filter: string = ''): Observable<PagedResult<Diagnostico>> {
    return this.getPaged(pageNumber, pageSize, filter);
  }

  // Obtiene un diagnóstico específico por su ID
  getDiagnosticoById(idDiagnostico: number): Observable<Diagnostico> {
    return this.getById(idDiagnostico);
  }

  // Registra un nuevo diagnóstico
  addDiagnostico(diagnostico: Diagnostico): Observable<boolean> {
    return this.add(diagnostico);
  }

  // Actualiza un diagnóstico existente
  updateDiagnostico(diagnostico: Diagnostico): Observable<boolean> {
    return this.update(diagnostico.idDiagnostico, diagnostico);
  }

  // Elimina un diagnóstico por su ID
  deleteDiagnostico(idDiagnostico: number): Observable<boolean> {
    return this.delete(idDiagnostico);
  }
}
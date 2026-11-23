import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../../generics/generic-crud.service';

// Modelo de datos para la entidad Sintoma
export interface Sintoma {
  idSintoma: number;
  nombre: string;
  descripcion: string;
}

@Injectable({
  providedIn: 'root'
})
export class SintomasService extends GenericCrudService<Sintoma> {
  constructor(http: HttpClient) {
    super(http, 'http://localhost:5085/Sintomas');
  }

  // Obtiene el listado paginado de síntomas con opción de filtro
  getSintomas(pageNumber: number = 1, pageSize: number = 10, filter: string = ''): Observable<PagedResult<Sintoma>> {
    return this.getPaged(pageNumber, pageSize, filter);
  }

  // Obtiene un síntoma específico por su ID
  getSintomaById(idSintoma: number): Observable<Sintoma> {
    return this.getById(idSintoma);
  }

  // Registra un nuevo síntoma
  addSintoma(sintoma: Sintoma): Observable<boolean> {
    return this.add(sintoma);
  }

  // Actualiza un síntoma existente
  updateSintoma(sintoma: Sintoma): Observable<boolean> {
    return this.update(sintoma.idSintoma, sintoma);
  }

  // Elimina un síntoma por su ID
  deleteSintoma(idSintoma: number): Observable<boolean> {
    return this.delete(idSintoma);
  }
}
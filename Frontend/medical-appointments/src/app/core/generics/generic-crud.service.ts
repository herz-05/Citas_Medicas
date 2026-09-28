import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

// Resultado paginado para las consultas con paginación
export interface PagedResult<T> {
  data: T[];
  totalRecords: number;
  pageSize: number;
  currentPage: number;
  totalPages: number;
}

// Servicio base abstracto para operaciones CRUD genéricas
export abstract class GenericCrudService<T> {

  protected constructor(
    protected http: HttpClient,
    protected apiUrl: string
  ) {}

  // Obtener todos los registros
  getAll(): Observable<T[]> {
    return this.http.get<T[]>(this.apiUrl);
  }

  // Obtener registros paginados con opción de filtro
  getPaged(
    pageNumber: number = 1,
    pageSize: number = 10,
    filter: string = ''
  ): Observable<PagedResult<T>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    // Aplicar filtro si no está vacío
    if (filter.trim() !== '') {
      params = params.set('filter', filter);
    }

    return this.http.get<PagedResult<T>>(this.apiUrl, { params });
  }

  // Obtener un registro por su ID
  getById(id: number): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/${id}`);
  }

  // Agregar un nuevo registro
  add(entity: T): Observable<boolean> {
    return this.http.post<boolean>(this.apiUrl, entity);
  }

  // Actualizar un registro existente
  update(id: number, entity: T): Observable<boolean> {
    return this.http.put<boolean>(`${this.apiUrl}/${id}`, entity);
  }

  // Eliminar un registro por su ID
  delete(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/${id}`);
  }
}
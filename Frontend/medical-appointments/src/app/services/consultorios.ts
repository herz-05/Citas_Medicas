import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Consultorio {
  idConsultorio: number;
  nombre: string;
  numeroConsultorio: string;
  piso: string;
  ubicacion: string;
  descripcion: string;
  estado: boolean;
  fechaRegistro: string;
}

@Injectable({
  providedIn: 'root'
})
export class ConsultoriosService {

  private apiUrl = 'https://localhost:7250/Consultorios';

  constructor(private http: HttpClient) {}

  getConsultorios(): Observable<Consultorio[]> {
    return this.http.get<Consultorio[]>(this.apiUrl);
  }
}
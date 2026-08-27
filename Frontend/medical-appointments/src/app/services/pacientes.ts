import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Paciente {
  idPaciente: number;
  nombres: string;
  apellidos: string;
  fechaNacimiento: string;
  sexo: string;
  dui: string;
  telefono: string;
  correo: string;
  direccion: string;
  fechaRegistro: string;
}

@Injectable({
  providedIn: 'root'
})
export class PacientesService {

  private apiUrl = 'https://localhost:7250/Pacientes';

  constructor(private http: HttpClient) {}

  getPacientes(): Observable<Paciente[]> {
    return this.http.get<Paciente[]>(this.apiUrl);
  }
}
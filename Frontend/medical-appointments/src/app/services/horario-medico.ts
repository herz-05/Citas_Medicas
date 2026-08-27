import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface HorarioMedico {
  idHorario: number;
  idMedico: number;
  idConsultorio: number;
  diaSemana: string;
  horaInicio: string;
  horaFin: string;
  estado: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class HorarioMedicoService {

  private apiUrl = 'https://localhost:7250/HorariosMedicos';

  constructor(private http: HttpClient) {}

  getHorarios(): Observable<HorarioMedico[]> {
    return this.http.get<HorarioMedico[]>(this.apiUrl);
  }
}
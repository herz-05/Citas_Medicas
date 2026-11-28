
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../../generics/generic-crud.service';

export interface Turno {
  idTurno: number;
  idCita: number | null;
  idHorario: number;
  idPaciente: number;
  numeroTurno: number;
  fechaTurno: string;
  estado: string;
  fechaRegistro: string;
}

export interface CitaPendienteTurno {
  idCita: number;
  idPaciente: number;
  idMedico: number;
  idEspecialidad: number;
  idConsultorio: number;
  fechaCita: string;
  horaInicio: string;
  horaFin: string;
  motivoConsulta: string | null;
}

export interface RespuestaLlegada {
  mensaje: string;
  numeroTurno: number;
}

@Injectable({
  providedIn: 'root'
})
export class TurnosService extends GenericCrudService<Turno> {

  private readonly turnosApiUrl = 'http://localhost:5031/Turnos';

  constructor(http: HttpClient) {
    super(http, 'http://localhost:5031/Turnos');
    this.httpClient = http;
  }

  private readonly httpClient: HttpClient;

  getTurnos(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<PagedResult<Turno>> {
    return this.getPaged(pageNumber, pageSize);
  }

  getTurnoById(idTurno: number): Observable<Turno> {
    return this.getById(idTurno);
  }

  getCitasPendientes(): Observable<CitaPendienteTurno[]> {
    return this.httpClient.get<CitaPendienteTurno[]>(
      `${this.turnosApiUrl}/citas-pendientes`
    );
  }

  registrarLlegada(idCita: number): Observable<RespuestaLlegada> {
    return this.httpClient.post<RespuestaLlegada>(
      `${this.turnosApiUrl}/registrar-llegada/${idCita}`,
      {}
    );
  }

  actualizarEstado(
    idTurno: number,
    estado: string
  ): Observable<boolean> {
    return this.httpClient.patch<boolean>(
      `${this.turnosApiUrl}/${idTurno}/estado`,
      { estado }
    );
  }

  deleteTurno(idTurno: number): Observable<boolean> {
    return this.delete(idTurno);
  }
}

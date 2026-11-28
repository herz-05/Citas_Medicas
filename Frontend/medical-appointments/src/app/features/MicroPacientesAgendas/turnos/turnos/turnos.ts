
import { Component, OnInit, OnDestroy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { finalize, interval, Subscription } from 'rxjs';

import {
  TurnosService,
  Turno,
  CitaPendienteTurno
} from '../../../../core/services/MicroPacientesAgendas/turnos';

import {
  PacientesService,
  Paciente
} from '../../../../core/services/MicroPacientesAgendas/pacientes';

import {
  HorarioMedicoService,
  HorarioMedico
} from '../../../../core/services/MicroPacientesAgendas/horario-medico';

import {
  MedicosService,
  Medico
} from '../../../../core/services/MicroIdentidadUsuarios/medicos';

import { BusquedaService } from '../../../../core/services/Compartidos/busqueda';
import { Paginacion } from '../../../../shared/components/paginacion/paginacion';

@Component({
  selector: 'app-turnos',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    Paginacion
  ],
  templateUrl: './turnos.html',
  styleUrl: './turnos.css'
})
export class TurnosComponent implements OnInit, OnDestroy {

  turnos = signal<Turno[]>([]);
  pacientes = signal<Paciente[]>([]);
  horarios = signal<HorarioMedico[]>([]);
  medicos = signal<Medico[]>([]);

  citasPendientes: CitaPendienteTurno[] = [];

  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  cargandoDatos = false;
  cargandoPendientes = false;
  cargandoPacientes = false;
  cargandoHorarios = false;
  cargandoMedicos = false;
  registrandoLlegada = false;

  private actualizacionAutomatica?: Subscription;

  constructor(
    private turnosService: TurnosService,
    private pacientesService: PacientesService,
    private horarioService: HorarioMedicoService,
    private medicosService: MedicosService,
    private busquedaService: BusquedaService
  ) {}

  // INICIALIZACIÓN

  ngOnInit(): void {

    this.busquedaService.configurar('Buscar turnos...');

    this.cargarTurnos();
    this.cargarCitasPendientes();
    this.cargarPacientes();
    this.cargarHorarios();
    this.cargarMedicos();

    // Actualizar citas pendientes cada 30 segundos.

    this.actualizacionAutomatica = interval(30000).subscribe(() => {
      this.cargarCitasPendientes();
    });
  }

  // DESTRUIR COMPONENTE

  ngOnDestroy(): void {
    this.actualizacionAutomatica?.unsubscribe();
  }

  // CARGAR TURNOS

  cargarTurnos(): void {

    if (this.cargandoDatos) return;

    this.cargandoDatos = true;

    this.turnosService.getTurnos(
      this.paginaActual,
      this.tamanoPagina
    )
    .pipe(
      finalize(() => {
        this.cargandoDatos = false;
      })
    )
    .subscribe({

      next: resultado => {

        this.turnos.set(resultado.data ?? []);

        this.totalRegistros = resultado.totalRecords;
        this.totalPaginas = resultado.totalPages;

        this.paginaActual = resultado.currentPage;
        this.tamanoPagina = resultado.pageSize;

      },

      error: error => {

        console.error('Error cargando turnos:', error);

      }

    });
  }

  // CARGAR CITAS PENDIENTES

  cargarCitasPendientes(): void {

    if (this.cargandoPendientes) return;

    this.cargandoPendientes = true;

    this.turnosService.getCitasPendientes()
    .pipe(
      finalize(() => {
        this.cargandoPendientes = false;
      })
    )
    .subscribe({

      next: resultado => {

        this.citasPendientes = resultado ?? [];

      },

      error: error => {

        console.error('Error cargando citas pendientes:', error);

      }

    });
  }

  // CARGAR PACIENTES

  cargarPacientes(): void {

    if (this.cargandoPacientes) return;

    this.cargandoPacientes = true;

    this.pacientesService.getPacientes(1, 1000)
    .pipe(
      finalize(() => {
        this.cargandoPacientes = false;
      })
    )
    .subscribe({

      next: resultado => {

        this.pacientes.set(resultado.data ?? []);

      },

      error: error => {

        console.error('Error cargando pacientes:', error);

      }

    });
  }

  // CARGAR HORARIOS

  cargarHorarios(): void {

    if (this.cargandoHorarios) return;

    this.cargandoHorarios = true;

    this.horarioService.getHorarios(1, 1000)
    .pipe(
      finalize(() => {
        this.cargandoHorarios = false;
      })
    )
    .subscribe({

      next: resultado => {

        this.horarios.set(resultado.data ?? []);

      },

      error: error => {

        console.error('Error cargando horarios:', error);

      }

    });
  }

  // CARGAR MÉDICOS

  cargarMedicos(): void {

    if (this.cargandoMedicos) return;

    this.cargandoMedicos = true;

    this.medicosService.getMedicos()
    .pipe(
      finalize(() => {
        this.cargandoMedicos = false;
      })
    )
    .subscribe({

      next: resultado => {

        this.medicos.set(resultado ?? []);

      },

      error: error => {

        console.error('Error cargando médicos:', error);

      }

    });
  }

  // NOMBRE DEL PACIENTE

  obtenerNombrePaciente(idPaciente: number): string {

    const paciente = this.pacientes().find(
      p => p.idPaciente === idPaciente
    );

    return paciente
      ? `${paciente.nombres} ${paciente.apellidos}`
      : `Paciente #${idPaciente}`;
  }

  // NOMBRE DEL MÉDICO

  obtenerNombreMedico(idMedico: number): string {

    const medico = this.medicos().find(
      m => m.idMedico === idMedico
    );

    return medico
      ? `${medico.nombres} ${medico.apellidos}`
      : `Médico #${idMedico}`;
  }

  // DESCRIPCIÓN DEL HORARIO

  obtenerDescripcionHorario(idHorario: number): string {

    const horario = this.horarios().find(
      h => h.idHorario === idHorario
    );

    if (!horario) {
      return `Horario #${idHorario}`;
    }

    return `${horario.diaSemana} | ${
      this.formatearHora(horario.horaInicio)
    } - ${this.formatearHora(horario.horaFin)}`;
  }

  // REGISTRAR LLEGADA


registrarLlegada(cita: CitaPendienteTurno): void {
  if (this.registrandoLlegada) return;

  const paciente = this.obtenerNombrePaciente(cita.idPaciente);
  const medico = this.obtenerNombreMedico(cita.idMedico);

  const confirmar = confirm(
    `¿Confirmar la llegada de ${paciente}?\n\n` +
    `Cita: #${cita.idCita}\n` +
    `Médico: ${medico}\n` +
    `Hora: ${this.formatearHora(cita.horaInicio)}`
  );

  if (!confirmar) return;

  this.registrandoLlegada = true;

  this.turnosService.registrarLlegada(cita.idCita)
    .pipe(
      finalize(() => {
        this.registrandoLlegada = false;
      })
    )
    .subscribe({
      next: resultado => {

        // 1. Quitar inmediatamente la cita de recepción.
        this.citasPendientes = this.citasPendientes.filter(
          c => c.idCita !== cita.idCita
        );

        // 2. Mostrar confirmación.
        alert(
          `${resultado.mensaje}\n\n` +
          `Número de turno: ${
            this.formatearNumeroTurno(resultado.numeroTurno)
          }`
        );

        // 3. Recargar turnos desde el servidor.
        this.paginaActual = 1;
        this.cargarTurnos();

        // 4. Actualizar recepción con los datos del servidor.
        this.cargarCitasPendientes();
      },
      error: error => {
        console.error('Error registrando llegada:', error);

        alert(
          error.error?.mensaje ||
          error.error?.message ||
          'No se pudo registrar la llegada.'
        );
      }
    });
}


  // CAMBIAR ESTADO

  cambiarEstadoTurno(turno: Turno, estado: string): void {

    if (estado === turno.estado) return;

    this.turnosService.actualizarEstado(
      turno.idTurno,
      estado
    ).subscribe({

      next: () => {

        this.turnos.update(lista =>
          lista.map(t =>
            t.idTurno === turno.idTurno
              ? { ...t, estado }
              : t
          )
        );

      },

      error: error => {

        console.error('Error actualizando turno:', error);

        alert('No se pudo actualizar el estado.');

        this.cargarTurnos();

      }

    });
  }

  // ELIMINAR TURNO

  eliminarTurno(turno: Turno): void {

    const paciente = this.obtenerNombrePaciente(turno.idPaciente);

    if (!confirm(
      `¿Desea eliminar el turno #${turno.numeroTurno} de ${paciente}?`
    )) {
      return;
    }

    this.turnosService.deleteTurno(turno.idTurno).subscribe({

      next: () => {

        if (
          this.turnos().length === 1 &&
          this.paginaActual > 1
        ) {
          this.paginaActual--;
        }

        this.cargarTurnos();
        this.cargarCitasPendientes();

      },

      error: error => {

        console.error('Error eliminando turno:', error);

        alert('No se pudo eliminar el turno.');

      }

    });
  }

  // PAGINACIÓN

  cambiarPagina(pagina: number): void {

    if (
      pagina < 1 ||
      pagina > this.totalPaginas ||
      pagina === this.paginaActual
    ) {
      return;
    }

    this.paginaActual = pagina;

    this.cargarTurnos();
  }

  cambiarTamanoPagina(tamano: number): void {

    this.tamanoPagina = tamano;
    this.paginaActual = 1;

    this.cargarTurnos();
  }

  // FORMATO

  formatearHora(hora: string): string {

    return hora ? hora.substring(0, 5) : '';
  }

  formatearNumeroTurno(numero: number): string {

    return String(numero).padStart(3, '0');
  }

}

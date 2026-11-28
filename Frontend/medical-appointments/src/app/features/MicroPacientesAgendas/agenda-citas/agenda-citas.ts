import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, of, timer } from 'rxjs';
import {
  switchMap,
  catchError,
  finalize,
  distinctUntilChanged
} from 'rxjs/operators';

import {
  CitasService,
  Cita
} from '../../../core/services/MicroPacientesAgendas/citas';

import {
  PacientesService,
  Paciente
} from '../../../core/services/MicroPacientesAgendas/pacientes';

import {
  ConsultoriosService,
  Consultorio
} from '../../../core/services/MicroPacientesAgendas/consultorios';

import {
  EstadoCitasService,
  EstadoCita
} from '../../../core/services/MicroPacientesAgendas/estado-citas';

import {
  HorariosMedicosService,
  HorarioDisponible
} from '../../../core/services/MicroIdentidadUsuarios/horarios-medicos';

import {
  MedicosService,
  Medico
} from '../../../core/services/MicroIdentidadUsuarios/medicos';

import {
  EspecialidadesService,
  Especialidad
} from '../../../core/services/MicroIdentidadUsuarios/especialidades';

import {
  MedicoEspecialidadesService,
  MedicoEspecialidad
} from '../../../core/services/MicroIdentidadUsuarios/medico-especialidad';

import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';
import { BusquedaService } from '../../../core/services/Compartidos/busqueda';

@Component({
  selector: 'app-agenda-citas',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion
  ],
  templateUrl: './agenda-citas.html',
  styleUrl: './agenda-citas.css'
})
export class AgendaCitas implements OnInit {

  citas = signal<Cita[]>([]);
  pacientes = signal<Paciente[]>([]);
  medicos = signal<Medico[]>([]);
  consultorios = signal<Consultorio[]>([]);
  estadosCitas = signal<EstadoCita[]>([]);

  especialidades = signal<Especialidad[]>([]);
  relacionesMedicoEspecialidad = signal<MedicoEspecialidad[]>([]);
  especialidadesDisponibles = signal<Especialidad[]>([]);

  horariosDisponibles = signal<HorarioDisponible[]>([]);

  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  mostrarFormulario = false;
  guardando = false;
  cargandoHorarios = false;
  errorHorarios = '';

  horarioPrecargado = false;
  diaSemanaPrecargado: string | null = null;

  fechaMinima = this.obtenerFechaLocal();

  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';

  citaSeleccionada: Cita | null = null;
  nuevaCita: Cita = this.crearCitaVacia();

  private solicitudesHorarios$ = new Subject<{
    idMedico: number;
    idEspecialidad: number;
    fecha: string;
  } | null>();

  constructor(
    private citasService: CitasService,
    private pacientesService: PacientesService,
    private medicosService: MedicosService,
    private consultoriosService: ConsultoriosService,
    private estadoCitasService: EstadoCitasService,
    private especialidadesService: EspecialidadesService,
    private medicoEspecialidadesService: MedicoEspecialidadesService,
    private horariosMedicosService: HorariosMedicosService,
    private busquedaService: BusquedaService
  ) {
    this.configurarConsultaHorarios();
  }

  ngOnInit(): void {
    this.busquedaService.configurar('Buscar citas...');

    this.cargarCitas();
    this.cargarPacientes();
    this.cargarMedicos();
    this.cargarConsultorios();
    this.cargarEstadosCitas();
    this.cargarEspecialidades();
    this.cargarRelacionesMedicoEspecialidad();

    this.recibirHorarioDesdeNavegacion();
  }

  private recibirHorarioDesdeNavegacion(): void {
    const estado = history.state;

    if (!estado?.agendarCita || !estado?.horario) {
      return;
    }

    const horario = estado.horario;

    this.modoFormulario = 'nuevo';
    this.citaSeleccionada = null;
    this.nuevaCita = this.crearCitaVacia();

    this.nuevaCita.idMedico = Number(horario.idMedico);
    this.nuevaCita.idEspecialidad = Number(horario.idEspecialidad);
    this.nuevaCita.idConsultorio = Number(horario.idConsultorio);

    this.horarioPrecargado = true;
    this.diaSemanaPrecargado = horario.diaSemana ?? null;

    this.fechaMinima = this.obtenerFechaLocal();

    this.actualizarEspecialidadesDisponibles();
    this.asignarEstadoConfirmada();
    this.limpiarHorarios();

    this.mostrarFormulario = true;

    history.replaceState(
      { ...history.state, agendarCita: false },
      ''
    );
  }

  private normalizarDia(dia: string): string {
    return dia
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .trim()
      .toLowerCase();
  }

  private configurarConsultaHorarios(): void {
    this.solicitudesHorarios$
      .pipe(
        distinctUntilChanged((a, b) =>
          a === b || (
            !!a && !!b &&
            a.idMedico === b.idMedico &&
            a.idEspecialidad === b.idEspecialidad &&
            a.fecha === b.fecha
          )
        ),
        switchMap(parametros => {
          if (!parametros) {
            this.cargandoHorarios = false;
            return of([]);
          }

          this.cargandoHorarios = true;
          this.errorHorarios = '';

          return timer(180).pipe(
            switchMap(() =>
              this.horariosMedicosService.getHorariosDisponibles(
                parametros.idMedico,
                parametros.idEspecialidad,
                parametros.fecha
              )
            ),
            catchError(error => {
              console.error('Error consultando horarios:', error);

              this.errorHorarios =
                'No fue posible consultar los horarios disponibles.';

              return of([]);
            }),
            finalize(() => {
              this.cargandoHorarios = false;
            })
          );
        })
      )
      .subscribe(horarios => {
        this.horariosDisponibles.set(horarios ?? []);
      });
  }

  cargarCitas(): void {
    this.citasService.getCitas(
      this.paginaActual,
      this.tamanoPagina
    ).subscribe({
      next: resultado => {
        this.citas.set(resultado.data ?? []);
        this.totalRegistros = resultado.totalRecords;
        this.totalPaginas = resultado.totalPages;
        this.paginaActual = resultado.currentPage;
      },
      error: error => console.error('Error cargando citas:', error)
    });
  }

  cargarPacientes(): void {
    this.pacientesService.getPacientes(1, 1000).subscribe({
      next: resultado => this.pacientes.set(resultado.data ?? []),
      error: error => console.error('Error cargando pacientes:', error)
    });
  }

  cargarMedicos(): void {
    this.medicosService.getMedicos().subscribe({
      next: resultado => {
        this.medicos.set(
          resultado.filter(medico => medico.estado)
        );
      },
      error: error => {
        console.error('Error cargando médicos:', error);
        this.medicos.set([]);
      }
    });
  }

  cargarConsultorios(): void {
    this.consultoriosService.getConsultorios(1, 1000).subscribe({
      next: resultado => this.consultorios.set(resultado.data ?? []),
      error: error => console.error('Error cargando consultorios:', error)
    });
  }

  cargarEstadosCitas(): void {
    this.estadoCitasService.getEstadosCitas(1, 1000).subscribe({
      next: resultado => {
        this.estadosCitas.set(resultado.data ?? []);

        if (this.modoFormulario === 'nuevo') {
          this.asignarEstadoConfirmada();
        }
      },
      error: error => console.error('Error cargando estados:', error)
    });
  }

  cargarEspecialidades(): void {
    this.especialidadesService.getEspecialidades(1, 1000).subscribe({
      next: resultado => {
        this.especialidades.set(resultado.data ?? []);
        this.actualizarEspecialidadesDisponibles();
      },
      error: error => {
        console.error('Error cargando especialidades:', error);
        this.especialidades.set([]);
      }
    });
  }

  cargarRelacionesMedicoEspecialidad(): void {
    this.medicoEspecialidadesService
      .getMedicoEspecialidades(1, 1000)
      .subscribe({
        next: resultado => {
          this.relacionesMedicoEspecialidad.set(resultado.data ?? []);
          this.actualizarEspecialidadesDisponibles();
        },
        error: error => {
          console.error('Error cargando relaciones:', error);
          this.relacionesMedicoEspecialidad.set([]);
        }
      });
  }

  actualizarEspecialidadesDisponibles(): void {
    const idMedico = Number(this.nuevaCita.idMedico);

    if (idMedico <= 0) {
      this.especialidadesDisponibles.set([]);
      return;
    }

    const ids = this.relacionesMedicoEspecialidad()
      .filter(r => Number(r.idMedico) === idMedico)
      .map(r => Number(r.idEspecialidad));

    this.especialidadesDisponibles.set(
      this.especialidades().filter(e =>
        ids.includes(Number(e.idEspecialidad))
      )
    );
  }

  cambiarMedico(idMedico: number): void {
    this.nuevaCita.idMedico = Number(idMedico);
    this.nuevaCita.idEspecialidad = 0;
    this.nuevaCita.idConsultorio = 0;
    this.nuevaCita.horaInicio = '';
    this.nuevaCita.horaFin = '';

    this.especialidadesDisponibles.set([]);
    this.limpiarHorarios();

    this.actualizarEspecialidadesDisponibles();
  }

  cambiarEspecialidad(): void {
    this.nuevaCita.idConsultorio = 0;
    this.nuevaCita.horaInicio = '';
    this.nuevaCita.horaFin = '';

    this.limpiarHorarios();
    this.consultarHorariosDisponibles();
  }

  cambiarFecha(): void {
    this.nuevaCita.idConsultorio = 0;
    this.nuevaCita.horaInicio = '';
    this.nuevaCita.horaFin = '';

    this.limpiarHorarios();

    if (this.nuevaCita.fechaCita < this.obtenerFechaLocal()) {
      this.errorHorarios = 'No puede seleccionar una fecha anterior a hoy.';
      return;
    }

    if (this.diaSemanaPrecargado) {
      const fecha = new Date(`${this.nuevaCita.fechaCita}T12:00:00`);

      const dias = [
        'domingo',
        'lunes',
        'martes',
        'miercoles',
        'jueves',
        'viernes',
        'sabado'
      ];

      const diaSeleccionado = dias[fecha.getDay()];

      if (
        this.normalizarDia(diaSeleccionado) !==
        this.normalizarDia(this.diaSemanaPrecargado)
      ) {
        this.errorHorarios =
          `Este horario solamente atiende los días ${this.diaSemanaPrecargado}.`;
        return;
      }
    }

    this.consultarHorariosDisponibles();
  }

  obtenerNombreEspecialidad(id: number): string {
    const especialidad = this.especialidades().find(
      e => Number(e.idEspecialidad) === Number(id)
    );

    return especialidad?.nombreEspecialidad
      || especialidad?.nombre
      || `Especialidad #${id}`;
  }

  consultarHorariosDisponibles(): void {
    const idMedico = Number(this.nuevaCita.idMedico);
    const idEspecialidad = Number(this.nuevaCita.idEspecialidad);
    const fecha = this.nuevaCita.fechaCita;

    if (
      idMedico <= 0 ||
      idEspecialidad <= 0 ||
      !fecha ||
      fecha < this.obtenerFechaLocal()
    ) {
      this.limpiarHorarios();
      return;
    }

    this.horariosDisponibles.set([]);

    this.solicitudesHorarios$.next({
      idMedico,
      idEspecialidad,
      fecha
    });
  }

  seleccionarHorario(horario: HorarioDisponible): void {
    if (this.modoFormulario === 'ver' || this.guardando) {
      return;
    }

    this.nuevaCita.horaInicio =
      this.formatearHora(horario.horaInicio);

    this.nuevaCita.horaFin =
      this.formatearHora(horario.horaFin);

    this.nuevaCita.idConsultorio = horario.idConsultorio;
  }

  esHorarioSeleccionado(horario: HorarioDisponible): boolean {
    return (
      this.nuevaCita.horaInicio ===
        this.formatearHora(horario.horaInicio) &&
      this.nuevaCita.horaFin ===
        this.formatearHora(horario.horaFin) &&
      Number(this.nuevaCita.idConsultorio) ===
        Number(horario.idConsultorio)
    );
  }

  limpiarHorarios(): void {
    this.solicitudesHorarios$.next(null);

    this.horariosDisponibles.set([]);
    this.cargandoHorarios = false;
    this.errorHorarios = '';
  }

  private obtenerFechaLocal(): string {
    const fecha = new Date();

    const anio = fecha.getFullYear();
    const mes = String(fecha.getMonth() + 1).padStart(2, '0');
    const dia = String(fecha.getDate()).padStart(2, '0');

    return `${anio}-${mes}-${dia}`;
  }

  private asignarEstadoConfirmada(): boolean {
    const estado = this.estadosCitas().find(
      e => e.nombreEstado?.trim().toLowerCase() === 'confirmada'
    );

    if (!estado) {
      return false;
    }

    this.nuevaCita.idEstadoCita = estado.idEstadoCita;
    return true;
  }

  obtenerNombrePaciente(id: number): string {
    const paciente = this.pacientes().find(
      p => p.idPaciente === id
    );

    return paciente
      ? `${paciente.nombres} ${paciente.apellidos}`
      : `Paciente #${id}`;
  }

  obtenerNombreMedico(id: number): string {
    const medico = this.medicos().find(
      m => m.idMedico === id
    );

    return medico
      ? `${medico.nombres} ${medico.apellidos}`
      : `Médico #${id}`;
  }

  obtenerNombreConsultorio(id: number): string {
    const consultorio = this.consultorios().find(
      c => c.idConsultorio === id
    );

    return consultorio
      ? consultorio.nombre
      : `Consultorio #${id}`;
  }

  obtenerNombreEstado(id: number): string {
    const estado = this.estadosCitas().find(
      e => e.idEstadoCita === id
    );

    return estado
      ? estado.nombreEstado
      : `Estado #${id}`;
  }

  abrirNuevaCita(): void {
    this.modoFormulario = 'nuevo';
    this.citaSeleccionada = null;
    this.nuevaCita = this.crearCitaVacia();

    this.horarioPrecargado = false;
    this.diaSemanaPrecargado = null;

    this.fechaMinima = this.obtenerFechaLocal();

    this.asignarEstadoConfirmada();

    this.especialidadesDisponibles.set([]);
    this.limpiarHorarios();

    this.mostrarFormulario = true;
  }

  verCita(cita: Cita): void {
    this.citasService.getCitaById(cita.idCita).subscribe({
      next: resultado => {
        this.limpiarHorarios();

        this.modoFormulario = 'ver';
        this.citaSeleccionada = resultado;

        this.nuevaCita = {
          ...resultado,
          horaInicio: this.formatearHora(resultado.horaInicio),
          horaFin: this.formatearHora(resultado.horaFin)
        };

        this.actualizarEspecialidadesDisponibles();
        this.mostrarFormulario = true;
      },
      error: error => console.error('Error obteniendo cita:', error)
    });
  }

  editarCita(cita: Cita): void {
    this.citasService.getCitaById(cita.idCita).subscribe({
      next: resultado => {
        this.limpiarHorarios();

        this.modoFormulario = 'editar';
        this.citaSeleccionada = resultado;

        this.nuevaCita = {
          ...resultado,
          horaInicio: this.formatearHora(resultado.horaInicio),
          horaFin: this.formatearHora(resultado.horaFin)
        };

        this.actualizarEspecialidadesDisponibles();
        this.mostrarFormulario = true;

        this.consultarHorariosDisponibles();
      },
      error: error => console.error('Error obteniendo cita:', error)
    });
  }

  cerrarFormulario(): void {
    if (this.guardando) return;

    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.citaSeleccionada = null;
    this.nuevaCita = this.crearCitaVacia();

    this.horarioPrecargado = false;
    this.diaSemanaPrecargado = null;

    this.especialidadesDisponibles.set([]);
    this.limpiarHorarios();
  }

  guardarCita(): void {
    if (this.guardando) return;

    if (this.nuevaCita.idPaciente <= 0) {
      alert('Seleccione un paciente.');
      return;
    }

    if (this.nuevaCita.idMedico <= 0) {
      alert('Seleccione un médico.');
      return;
    }

    if (this.nuevaCita.idEspecialidad <= 0) {
      alert('Seleccione una especialidad.');
      return;
    }

    if (!this.especialidadesDisponibles().some(
      e => Number(e.idEspecialidad) ===
        Number(this.nuevaCita.idEspecialidad)
    )) {
      alert('La especialidad no corresponde al médico seleccionado.');
      return;
    }

    if (!this.nuevaCita.fechaCita) {
      alert('Seleccione la fecha de la cita.');
      return;
    }

    this.fechaMinima = this.obtenerFechaLocal();

    if (this.nuevaCita.fechaCita < this.fechaMinima) {
      alert('No puede programar citas en fechas anteriores a hoy.');
      return;
    }

    if (!this.nuevaCita.horaInicio || !this.nuevaCita.horaFin) {
      alert('Seleccione un horario disponible.');
      return;
    }

    if (this.nuevaCita.idConsultorio <= 0) {
      alert('No se ha asignado un consultorio.');
      return;
    }

    if (!this.nuevaCita.motivoConsulta?.trim()) {
      alert('Ingrese el motivo de consulta.');
      return;
    }

    if (this.modoFormulario === 'nuevo') {
      if (!this.asignarEstadoConfirmada()) {
        alert('No se encontró el estado Confirmada en el catálogo.');
        return;
      }

      this.agregarCita();
    } else if (this.modoFormulario === 'editar') {
      this.actualizarCita();
    }
  }

  private agregarCita(): void {
    this.guardando = true;

    const cita = this.prepararCitaParaApi(this.nuevaCita);

    this.citasService.addCita(cita).subscribe({
      next: () => {
        this.guardando = false;
        this.cerrarFormulario();
        this.paginaActual = 1;
        this.cargarCitas();
      },
      error: error => {
        this.guardando = false;
        console.error('Error agregando cita:', error);
        alert(
          error.error?.mensaje ||
          error.error?.message ||
          'No se pudo registrar la cita.'
        );
      }
    });
  }

  private actualizarCita(): void {
    this.guardando = true;

    const cita = this.prepararCitaParaApi(this.nuevaCita);

    this.citasService.updateCita(cita).subscribe({
      next: () => {
        this.guardando = false;
        this.cerrarFormulario();
        this.cargarCitas();
      },
      error: error => {
        this.guardando = false;
        console.error('Error actualizando cita:', error);
        alert(
          error.error?.mensaje ||
          error.error?.message ||
          'No se pudo actualizar la cita.'
        );
      }
    });
  }

  private prepararCitaParaApi(cita: Cita): Cita {
    return {
      ...cita,
      idMedico: Number(cita.idMedico),
      idEspecialidad: Number(cita.idEspecialidad),
      idConsultorio: Number(cita.idConsultorio),
      horaInicio: this.convertirHoraParaApi(cita.horaInicio),
      horaFin: this.convertirHoraParaApi(cita.horaFin)
    };
  }

  eliminarCita(cita: Cita): void {
    const paciente = this.obtenerNombrePaciente(cita.idPaciente);

    if (!confirm(
      `¿Desea eliminar la cita #${cita.idCita} de ${paciente}?`
    )) {
      return;
    }

    this.citasService.deleteCita(cita.idCita).subscribe({
      next: () => {
        if (this.citas().length === 1 && this.paginaActual > 1) {
          this.paginaActual--;
        }

        this.cargarCitas();
      },
      error: error => console.error('Error eliminando cita:', error)
    });
  }

  private convertirHoraParaApi(hora: string): string {
    if (!hora) return '';
    return hora.length === 5 ? `${hora}:00` : hora;
  }

  formatearHora(hora: string): string {
    if (!hora) return '';
    return hora.substring(0, 5);
  }

  cambiarPagina(pagina: number): void {
    if (
      pagina < 1 ||
      pagina > this.totalPaginas ||
      pagina === this.paginaActual
    ) {
      return;
    }

    this.paginaActual = pagina;
    this.cargarCitas();
  }

  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = tamano;
    this.paginaActual = 1;
    this.cargarCitas();
  }

  private crearCitaVacia(): Cita {
    return {
      idCita: 0,
      idPaciente: 0,
      idMedico: 0,
      idEspecialidad: 0,
      idConsultorio: 0,
      idEstadoCita: 0,
      fechaCita: '',
      horaInicio: '',
      horaFin: '',
      motivoConsulta: '',
      observaciones: '',
      fechaRegistro: new Date().toISOString()
    };
  }
}
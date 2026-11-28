
using Core.Services;
using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.Citas.Commands
{
    public class UpdateCitasCommand : IRequest<bool>
    {
        public int IdCita { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public int IdConsultorio { get; set; }
        public int IdEstadoCita { get; set; }
        public DateOnly FechaCita { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public string? MotivoConsulta { get; set; }
        public string? Observaciones { get; set; }
    }

    public class CitaConflictoUpdateDto
    {
        public int IdCita { get; set; }
        public int IdMedico { get; set; }
        public int IdConsultorio { get; set; }
        public int IdEstadoCita { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }

    public class UpdateCitasCommandHandler
        : IRequestHandler<UpdateCitasCommand, bool>
    {
        private readonly IGenericRepository<Cita> _citaRepository;
        private readonly IGenericRepository<Pacientes> _pacienteRepository;
        private readonly IGenericRepository<Consultorio> _consultorioRepository;
        private readonly IGenericRepository<HorariosMedico> _horarioRepository;
        private readonly IGenericRepository<EstadoCitas> _estadoRepository;
        private readonly IIdentidadService _identidadService;

        public UpdateCitasCommandHandler(
            IGenericRepository<Cita> citaRepository,
            IGenericRepository<Pacientes> pacienteRepository,
            IGenericRepository<Consultorio> consultorioRepository,
            IGenericRepository<HorariosMedico> horarioRepository,
            IGenericRepository<EstadoCitas> estadoRepository,
            IIdentidadService identidadService)
        {
            _citaRepository = citaRepository;
            _pacienteRepository = pacienteRepository;
            _consultorioRepository = consultorioRepository;
            _horarioRepository = horarioRepository;
            _estadoRepository = estadoRepository;
            _identidadService = identidadService;
        }

        public async Task<bool> Handle(
            UpdateCitasCommand request,
            CancellationToken cancellationToken)
        {
            var cita = await _citaRepository.GetByIdAsync(
                request.IdCita,
                cancellationToken);

            if (cita == null)
                return false;

            if (request.IdPaciente <= 0)
                throw new ArgumentException("Debe seleccionar un paciente.");

            if (request.IdMedico <= 0)
                throw new ArgumentException("Debe seleccionar un médico.");

            if (request.IdEspecialidad <= 0)
                throw new ArgumentException("Debe seleccionar una especialidad.");

            if (request.IdConsultorio <= 0)
                throw new ArgumentException("Debe seleccionar un consultorio.");

            var paciente = await _pacienteRepository.GetByIdAsync(
                request.IdPaciente, cancellationToken);

            if (paciente == null)
                throw new ArgumentException("El paciente seleccionado no existe.");

            var consultorio = await _consultorioRepository.GetByIdAsync(
                request.IdConsultorio, cancellationToken);

            if (consultorio == null)
                throw new ArgumentException("El consultorio seleccionado no existe.");

            var estados = await _estadoRepository.GetAllAsync(
                cancellationToken);

            var estadoSeleccionado = estados.FirstOrDefault(
                e => e.IdEstadoCita == request.IdEstadoCita);

            if (estadoSeleccionado == null)
                throw new ArgumentException(
                    "El estado de la cita seleccionado no existe.");

            var estadosCancelados = estados
                .Where(e =>
                    !string.IsNullOrWhiteSpace(e.NombreEstado) &&
                    (
                        e.NombreEstado.Trim().Equals(
                            "Cancelada", StringComparison.OrdinalIgnoreCase) ||
                        e.NombreEstado.Trim().Equals(
                            "Cancelado", StringComparison.OrdinalIgnoreCase) ||
                        e.NombreEstado.Trim().Equals(
                            "Anulada", StringComparison.OrdinalIgnoreCase) ||
                        e.NombreEstado.Trim().Equals(
                            "Anulado", StringComparison.OrdinalIgnoreCase)
                    ))
                .Select(e => e.IdEstadoCita)
                .ToHashSet();

            // Identificar si la cita se está cancelando.
            bool seEstaCancelando =
                estadosCancelados.Contains(request.IdEstadoCita);

            // Validar médico y especialidad.
            var medicoValido =
                await _identidadService.MedicoValidoAsync(
                    request.IdMedico,
                    request.IdEspecialidad,
                    cancellationToken);

            if (!medicoValido)
                throw new ArgumentException(
                    "El médico no existe, está inactivo o no tiene asignada la especialidad.");

            // Las validaciones de disponibilidad solamente
            // se aplican cuando la cita no está cancelada.
            if (!seEstaCancelando)
            {
                var hoy = DateOnly.FromDateTime(DateTime.Today);

                if (request.FechaCita < hoy)
                    throw new ArgumentException(
                        "No se permiten registrar citas en fechas anteriores a hoy.");

                if (request.HoraInicio >= request.HoraFin)
                    throw new ArgumentException(
                        "La hora de inicio debe ser menor que la hora de fin.");

                if (request.HoraInicio.AddMinutes(30) != request.HoraFin)
                    throw new ArgumentException(
                        "Las citas deben tener una duración exacta de 30 minutos.");

                if (request.FechaCita == hoy &&
                    request.HoraInicio <= TimeOnly.FromDateTime(DateTime.Now))
                    throw new ArgumentException(
                        "No se pueden reservar horarios que ya han pasado.");

                // Verificar que exista el horario correspondiente.
                var diaSemana = ObtenerDiaSemana(request.FechaCita);

                var horario = await _horarioRepository.GetOneByAsync(
                    h =>
                        h.IdMedico == request.IdMedico &&
                        h.IdEspecialidad == request.IdEspecialidad &&
                        h.IdConsultorio == request.IdConsultorio &&
                        h.DiaSemana == diaSemana &&
                        h.Estado &&
                        h.HoraInicio <= request.HoraInicio.ToTimeSpan() &&
                        h.HoraFin >= request.HoraFin.ToTimeSpan(),
                    true,
                    cancellationToken);

                if (horario == null)
                    throw new ArgumentException(
                        "El horario seleccionado no pertenece a la disponibilidad del médico.");

                // Consultar las citas de la fecha.
                var citasDelDia =
                    await _citaRepository.GetProjectedAsync(
                        c => c.FechaCita == request.FechaCita,

                        c => new CitaConflictoUpdateDto
                        {
                            IdCita = c.IdCita,
                            IdMedico = c.IdMedico,
                            IdConsultorio = c.IdConsultorio,
                            IdEstadoCita = c.IdEstadoCita,
                            HoraInicio = c.HoraInicio,
                            HoraFin = c.HoraFin
                        },

                        cancellationToken);

                // Excluir la propia cita y las canceladas.
                var conflicto = citasDelDia.Any(c =>
                    c.IdCita != request.IdCita &&
                    !estadosCancelados.Contains(c.IdEstadoCita) &&
                    (
                        c.IdMedico == request.IdMedico ||
                        c.IdConsultorio == request.IdConsultorio
                    ) &&
                    request.HoraInicio < c.HoraFin &&
                    request.HoraFin > c.HoraInicio);

                if (conflicto)
                    throw new ArgumentException(
                        "El médico o consultorio ya tiene una cita asignada en ese horario.");
            }

            // Actualizar la cita.
            cita.IdPaciente = request.IdPaciente;
            cita.IdMedico = request.IdMedico;
            cita.IdEspecialidad = request.IdEspecialidad;
            cita.IdConsultorio = request.IdConsultorio;
            cita.IdEstadoCita = request.IdEstadoCita;
            cita.FechaCita = request.FechaCita;
            cita.HoraInicio = request.HoraInicio;
            cita.HoraFin = request.HoraFin;
            cita.MotivoConsulta = request.MotivoConsulta;
            cita.Observaciones = request.Observaciones;

            await _citaRepository.UpdateAsync(
                cita,
                cancellationToken);

            return true;
        }

        private static string ObtenerDiaSemana(DateOnly fecha)
        {
            return fecha.DayOfWeek switch
            {
                DayOfWeek.Monday => "Lunes",
                DayOfWeek.Tuesday => "Martes",
                DayOfWeek.Wednesday => "Miércoles",
                DayOfWeek.Thursday => "Jueves",
                DayOfWeek.Friday => "Viernes",
                DayOfWeek.Saturday => "Sábado",
                DayOfWeek.Sunday => "Domingo",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}

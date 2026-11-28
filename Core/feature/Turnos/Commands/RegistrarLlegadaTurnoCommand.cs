
using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.Turnos.Commands
{
    public class RegistrarLlegadaTurnoCommand : IRequest<int>
    {
        public int IdCita { get; set; }
    }

    public class RegistrarLlegadaTurnoCommandHandler
        : IRequestHandler<RegistrarLlegadaTurnoCommand, int>
    {
        private readonly IGenericRepository<Cita> _citaRepository;
        private readonly IGenericRepository<Turno> _turnoRepository;
        private readonly IGenericRepository<EstadoCitas> _estadoRepository;
        private readonly IGenericRepository<HorariosMedico> _horarioRepository;

        public RegistrarLlegadaTurnoCommandHandler(
            IGenericRepository<Cita> citaRepository,
            IGenericRepository<Turno> turnoRepository,
            IGenericRepository<EstadoCitas> estadoRepository,
            IGenericRepository<HorariosMedico> horarioRepository)
        {
            _citaRepository = citaRepository;
            _turnoRepository = turnoRepository;
            _estadoRepository = estadoRepository;
            _horarioRepository = horarioRepository;
        }

        public async Task<int> Handle(
            RegistrarLlegadaTurnoCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Validar cita.
            if (request.IdCita <= 0)
                throw new ArgumentException(
                    "Seleccione una cita válida.");

            var cita = await _citaRepository.GetByIdAsync(
                request.IdCita,
                cancellationToken);

            if (cita == null)
                throw new ArgumentException(
                    "La cita no existe.");

            // 2. Validar fecha.
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            if (cita.FechaCita != hoy)
                throw new ArgumentException(
                    "Solamente se pueden registrar llegadas de citas del día actual.");

            // 3. Validar estado de la cita.
            var estados = await _estadoRepository.GetAllAsync(
                cancellationToken);

            var confirmada = estados.FirstOrDefault(e =>
                e.NombreEstado.Trim().Equals(
                    "Confirmada",
                    StringComparison.OrdinalIgnoreCase));

            if (confirmada == null)
                throw new InvalidOperationException(
                    "No existe el estado Confirmada.");

            if (cita.IdEstadoCita != confirmada.IdEstadoCita)
                throw new ArgumentException(
                    "La cita no está confirmada y no puede generar un turno.");

            // 4. Comprobar que la cita no tenga turno.
            var turnoExistente = await _turnoRepository.GetOneByAsync(
                t => t.IdCita == cita.IdCita,
                true,
                cancellationToken);

            if (turnoExistente != null)
                throw new ArgumentException(
                    "Esta cita ya tiene un turno registrado.");

            // 5. Buscar el horario médico correspondiente.
            var diaSemana = ObtenerDiaSemana(cita.FechaCita);

            var horario = await _horarioRepository.GetOneByAsync(
                h =>
                    h.IdMedico == cita.IdMedico &&
                    h.IdEspecialidad == cita.IdEspecialidad &&
                    h.IdConsultorio == cita.IdConsultorio &&
                    h.DiaSemana == diaSemana &&
                    h.Estado &&
                    h.HoraInicio <= cita.HoraInicio.ToTimeSpan() &&
                    h.HoraFin >= cita.HoraFin.ToTimeSpan(),
                true,
                cancellationToken);

            if (horario == null)
                throw new ArgumentException(
                    "No se encontró el horario médico de la cita.");

            // =========================================
            // 6. NUMERACIÓN POR MÉDICO Y FECHA
            // =========================================

            // Obtener todos los horarios del médico.
            // Así no se mezclan sus turnos con los de
            // otros médicos ni se reinicia el contador
            // al cambiar de consultorio.
            var horarios = await _horarioRepository.GetAllAsync(
                cancellationToken);

            var idsHorariosMedico = horarios
                .Where(h => h.IdMedico == cita.IdMedico)
                .Select(h => h.IdHorario)
                .ToArray();

            // Consultar los turnos del médico en el día actual.
            var turnosDelMedico = await _turnoRepository.GetPagedAsync(
                pageNumber: 1,
                pageSize: 10000,
                filter: t =>
                    t.FechaTurno == hoy &&
                    idsHorariosMedico.Contains(t.IdHorario),
                cancellationToken: cancellationToken);

            // Obtener el siguiente número disponible.
            var numeroTurno = turnosDelMedico.Data
                .Select(t => t.NumeroTurno)
                .DefaultIfEmpty(0)
                .Max() + 1;

            // 7. Crear turno.
            var turno = new Turno
            {
                IdCita = cita.IdCita,
                IdHorario = horario.IdHorario,
                IdPaciente = cita.IdPaciente,
                NumeroTurno = numeroTurno,
                FechaTurno = hoy,
                Estado = "En espera",
                FechaRegistro = DateTime.UtcNow
            };

            await _turnoRepository.AddAsync(
                turno,
                cancellationToken);

            return turno.NumeroTurno;
        }

        // Convertir fecha a día de la semana en español.
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


using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.Turnos.Queries
{
    public class GetCitasPendientesTurnoQuery
        : IRequest<List<CitaPendienteTurnoDto>>
    {
    }

    public class CitaPendienteTurnoDto
    {
        public int IdCita { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public int IdConsultorio { get; set; }
        public DateOnly FechaCita { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public string? MotivoConsulta { get; set; }
    }

    public class GetCitasPendientesTurnoQueryHandler
        : IRequestHandler<
            GetCitasPendientesTurnoQuery,
            List<CitaPendienteTurnoDto>>
    {
        private readonly IGenericRepository<Cita> _citaRepository;
        private readonly IGenericRepository<Turno> _turnoRepository;
        private readonly IGenericRepository<EstadoCitas> _estadoRepository;

        public GetCitasPendientesTurnoQueryHandler(
            IGenericRepository<Cita> citaRepository,
            IGenericRepository<Turno> turnoRepository,
            IGenericRepository<EstadoCitas> estadoRepository)
        {
            _citaRepository = citaRepository;
            _turnoRepository = turnoRepository;
            _estadoRepository = estadoRepository;
        }

        public async Task<List<CitaPendienteTurnoDto>> Handle(
            GetCitasPendientesTurnoQuery request,
            CancellationToken cancellationToken)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            var estados = await _estadoRepository.GetAllAsync(
                cancellationToken);

            var confirmada = estados.FirstOrDefault(e =>
                e.NombreEstado.Trim().Equals(
                    "Confirmada",
                    StringComparison.OrdinalIgnoreCase));

            if (confirmada == null)
                return new List<CitaPendienteTurnoDto>();

            var citas = await _citaRepository.GetPagedAsync(
                pageNumber: 1,
                pageSize: 10000,
                filter: c =>
                    c.FechaCita == hoy &&
                    c.IdEstadoCita == confirmada.IdEstadoCita,
                cancellationToken: cancellationToken);

            var turnos = await _turnoRepository.GetPagedAsync(
                pageNumber: 1,
                pageSize: 10000,
                filter: t => t.FechaTurno == hoy,
                cancellationToken: cancellationToken);

            var citasConTurno = turnos.Data
                .Where(t => t.IdCita.HasValue)
                .Select(t => t.IdCita!.Value)
                .ToHashSet();

            return citas.Data
                .Where(c => !citasConTurno.Contains(c.IdCita))
                .OrderBy(c => c.HoraInicio)
                .Select(c => new CitaPendienteTurnoDto
                {
                    IdCita = c.IdCita,
                    IdPaciente = c.IdPaciente,
                    IdMedico = c.IdMedico,
                    IdEspecialidad = c.IdEspecialidad,
                    IdConsultorio = c.IdConsultorio,
                    FechaCita = c.FechaCita,
                    HoraInicio = c.HoraInicio,
                    HoraFin = c.HoraFin,
                    MotivoConsulta = c.MotivoConsulta
                })
                .ToList();
        }
    }
}

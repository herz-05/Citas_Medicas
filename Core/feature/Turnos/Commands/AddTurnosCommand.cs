
using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.Turnos.Commands
{
    public class AddTurnosCommand : IRequest<bool>
    {
        public int IdHorario { get; set; }
        public int IdPaciente { get; set; }
        public DateOnly FechaTurno { get; set; }
        public string Estado { get; set; } = "En espera";
    }

    public class AddTurnosCommandHandler
        : IRequestHandler<AddTurnosCommand, bool>
    {
        private readonly IGenericRepository<Turno> _repository;

        private static readonly string[] EstadosPermitidos =
        {
            "En espera",
            "Llamado",
            "Atendiendo",
            "Atendido",
            "Ausente",
            "Cancelado"
        };

        public AddTurnosCommandHandler(
            IGenericRepository<Turno> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddTurnosCommand request,
            CancellationToken cancellationToken)
        {
            if (request.IdHorario <= 0)
                throw new ArgumentException(
                    "Debe seleccionar un horario.");

            if (request.IdPaciente <= 0)
                throw new ArgumentException(
                    "Debe seleccionar un paciente.");

            if (request.FechaTurno == default)
                throw new ArgumentException(
                    "Debe seleccionar una fecha.");

            if (!EstadosPermitidos.Contains(
                request.Estado,
                StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "El estado del turno no es válido.");
            }

            var turnos = await _repository.GetAllAsync(
                cancellationToken);

            var numeroTurno = turnos
                .Where(t =>
                    t.IdHorario == request.IdHorario &&
                    t.FechaTurno == request.FechaTurno)
                .Select(t => t.NumeroTurno)
                .DefaultIfEmpty(0)
                .Max() + 1;

            var turno = new Turno
            {
                IdHorario = request.IdHorario,
                IdPaciente = request.IdPaciente,
                NumeroTurno = numeroTurno,
                FechaTurno = request.FechaTurno,
                Estado = request.Estado,
                FechaRegistro = DateTime.UtcNow
            };

            await _repository.AddAsync(
                turno,
                cancellationToken);

            return true;
        }
    }
}

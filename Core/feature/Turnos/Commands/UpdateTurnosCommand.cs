
using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.Turnos.Commands
{
    public class UpdateTurnosCommand : IRequest<bool>
    {
        public int IdTurno { get; set; }
        public int IdHorario { get; set; }
        public int IdPaciente { get; set; }
        public DateOnly FechaTurno { get; set; }
        public string Estado { get; set; } = "En espera";
    }

    public class UpdateTurnosCommandHandler
        : IRequestHandler<UpdateTurnosCommand, bool>
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

        public UpdateTurnosCommandHandler(
            IGenericRepository<Turno> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateTurnosCommand request,
            CancellationToken cancellationToken)
        {
            if (request.IdTurno <= 0)
                throw new ArgumentException(
                    "Debe seleccionar un turno válido.");

            var turno = await _repository.GetByIdAsync(
                request.IdTurno,
                cancellationToken);

            if (turno == null)
                return false;

            if (request.IdHorario <= 0)
                throw new ArgumentException(
                    "Debe seleccionar un horario.");

            if (request.IdPaciente <= 0)
                throw new ArgumentException(
                    "Debe seleccionar un paciente.");

            if (request.FechaTurno == default)
                throw new ArgumentException(
                    "Debe seleccionar una fecha.");

            var estadoValido = EstadosPermitidos.FirstOrDefault(
                e => e.Equals(
                    request.Estado?.Trim(),
                    StringComparison.OrdinalIgnoreCase));

            if (estadoValido == null)
                throw new ArgumentException(
                    "El estado del turno no es válido.");

            turno.IdHorario = request.IdHorario;
            turno.IdPaciente = request.IdPaciente;
            turno.FechaTurno = request.FechaTurno;
            turno.Estado = estadoValido;

            await _repository.UpdateAsync(
                turno,
                cancellationToken);

            return true;
        }
    }
}

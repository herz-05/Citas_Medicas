
using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.Turnos.Commands
{
    public class UpdateEstadoTurnoCommand : IRequest<bool>
    {
        public int IdTurno { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class UpdateEstadoTurnoCommandHandler
        : IRequestHandler<UpdateEstadoTurnoCommand, bool>
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

        public UpdateEstadoTurnoCommandHandler(
            IGenericRepository<Turno> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateEstadoTurnoCommand request,
            CancellationToken cancellationToken)
        {
            if (!EstadosPermitidos.Contains(
                request.Estado,
                StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "El estado del turno no es válido.");
            }

            var turno = await _repository.GetByIdAsync(
                request.IdTurno,
                cancellationToken);

            if (turno == null)
                return false;

            turno.Estado = request.Estado;

            await _repository.UpdateAsync(
                turno,
                cancellationToken);

            return true;
        }
    }
}

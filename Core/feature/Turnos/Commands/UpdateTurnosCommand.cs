using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.Turnos.Commands
{
    public class UpdateTurnosCommand : IRequest<bool>
    {
        public int IdTurno { get; set; }
        public int IdHorario { get; set; }
        public int IdPaciente { get; set; }
        public int NumeroTurno { get; set; }
        public DateOnly FechaTurno { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class UpdateTurnosCommandHandler
        : IRequestHandler<UpdateTurnosCommand, bool>
    {
        private readonly IGenericRepository<Turno> _repository;

        public UpdateTurnosCommandHandler(
            IGenericRepository<Turno> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateTurnosCommand request,
            CancellationToken cancellationToken)
        {
            var turno = await _repository.GetByIdAsync(request.IdTurno);

            if (turno == null)
            {
                return false;
            }

            turno.IdHorario = request.IdHorario;
            turno.IdPaciente = request.IdPaciente;
            turno.NumeroTurno = request.NumeroTurno;
            turno.FechaTurno = request.FechaTurno;
            turno.Estado = request.Estado;
            turno.FechaRegistro = request.FechaRegistro;

            await _repository.UpdateAsync(turno);

            return true;
        }
    }
}
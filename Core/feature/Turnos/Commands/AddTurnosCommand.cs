using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.Turnos.Commands
{
    public class AddTurnosCommand : IRequest<bool>
    {
        public int IdTurno { get; set; }
        public int IdHorario { get; set; }
        public int IdPaciente { get; set; }
        public int NumeroTurno { get; set; }
        public DateOnly FechaTurno { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class AddTurnosCommandHandler
        : IRequestHandler<AddTurnosCommand, bool>
    {
        private readonly IGenericRepository<Turno> _repository;

        public AddTurnosCommandHandler(
            IGenericRepository<Turno> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddTurnosCommand request,
            CancellationToken cancellationToken)
        {
            var turno = new Turno
            {
                IdHorario = request.IdHorario,
                IdPaciente = request.IdPaciente,
                NumeroTurno = request.NumeroTurno,
                FechaTurno = request.FechaTurno,
                Estado = request.Estado,
                FechaRegistro = request.FechaRegistro
            };

            await _repository.AddAsync(turno);

            return true;
        }
    }
}
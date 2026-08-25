using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.Commands
{
    public class AddHorarioMedicoCommand : IRequest<bool>
    {
        public int IdMedico { get; set; }

        public int IdConsultorio { get; set; }

        public string DiaSemana { get; set; } = string.Empty;

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public bool Estado { get; set; }
    }

    public class AddHorarioMedicoCommandHandler
        : IRequestHandler<AddHorarioMedicoCommand, bool>
    {
        private readonly IHorariosMedicos _repository;

        public AddHorarioMedicoCommandHandler(
            IHorariosMedicos repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddHorarioMedicoCommand request,
            CancellationToken cancellationToken)
        {
            var horario = new HorarioMedico
            {
                IdMedico = request.IdMedico,
                IdConsultorio = request.IdConsultorio,
                DiaSemana = request.DiaSemana,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                Estado = request.Estado
            };

            await _repository.AddHorarioMedico(horario);

            return true;
        }
    }
}
using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.HorarioMedico.Commands
{
    public class UpdateHorarioMedicoCommand : IRequest<bool>
    {
        public int IdHorario { get; set; }
        public int IdMedico { get; set; }
        public int IdConsultorio { get; set; }
        public string DiaSemana { get; set; } = string.Empty;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public bool Estado { get; set; }
    }

    public class UpdateHorarioMedicoCommandHandler
        : IRequestHandler<UpdateHorarioMedicoCommand, bool>
    {
        private readonly IGenericRepository<HorariosMedico> _repository;

        public UpdateHorarioMedicoCommandHandler(
            IGenericRepository<HorariosMedico> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateHorarioMedicoCommand request,
            CancellationToken cancellationToken)
        {
            var horario = await _repository.GetByIdAsync(
                request.IdHorario);

            if (horario == null)
            {
                return false;
            }

            horario.IdMedico = request.IdMedico;
            horario.IdConsultorio = request.IdConsultorio;
            horario.DiaSemana = request.DiaSemana;
            horario.HoraInicio = request.HoraInicio;
            horario.HoraFin = request.HoraFin;
            horario.Estado = request.Estado;

            await _repository.UpdateAsync(horario);

            return true;
        }
    }
}
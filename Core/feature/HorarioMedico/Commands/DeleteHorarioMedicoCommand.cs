using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.HorariosMedicos.Commands
{
    public class DeleteHorarioMedicoCommand : IRequest<bool>
    {
        public int IdHorario { get; set; }
    }

    public class DeleteHorarioMedicoCommandHandler
        : IRequestHandler<DeleteHorarioMedicoCommand, bool>
    {
        private readonly IGenericRepository<HorarioMedico> _repository;

        public DeleteHorarioMedicoCommandHandler(
            IGenericRepository<HorarioMedico> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteHorarioMedicoCommand request,
            CancellationToken cancellationToken)
        {
            var horario = await _repository.GetByIdAsync(
                request.IdHorario);

            if (horario == null)
            {
                return false;
            }

            await _repository.DeleteAsync(horario);

            return true;
        }
    }
}
using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.HorarioMedico.Commands
{
    public class DeleteHorarioMedicoCommand: IRequest<bool>
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

            await _repository.AddAsync(horario);

            return true;
        }
    }
}

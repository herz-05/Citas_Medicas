using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Citas.Commands
{
    public class DeleteCitasCommand: IRequest<bool>
    {
        public int IdCita { get; set; }
    }

    public class DeleteCitasCommandHandler
       : IRequestHandler<DeleteCitasCommand, bool>
    {
        private readonly IGenericRepository<Cita> _repository;

        public DeleteCitasCommandHandler(
            IGenericRepository<Cita> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteCitasCommand request,
            CancellationToken cancellationToken)
        {
            var citas = await _repository.GetByIdAsync(
                request.IdCita);

            if (citas == null)
            {

                return false;
            }

            await _repository.AddAsync(citas);

            return true;
        }
    }
}

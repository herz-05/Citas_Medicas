using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Paciente.Commands
{
    public class DeletePacienteCommand: IRequest<bool>
    {
        public int IdPaciente { get; set; }
    }

    public class DeletePacienteCommandHandler
         : IRequestHandler<DeletePacienteCommand, bool>
    {
        private readonly IGenericRepository<Pacientes> _repository;

        public DeletePacienteCommandHandler(
            IGenericRepository<Pacientes> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeletePacienteCommand request,
            CancellationToken cancellationToken)
        {
            var paciente = await _repository.GetByIdAsync(
                request.IdPaciente);

            if (paciente == null)
            {

                return false;
            }

            await _repository.AddAsync(paciente);

            return true;
        }
    }
}

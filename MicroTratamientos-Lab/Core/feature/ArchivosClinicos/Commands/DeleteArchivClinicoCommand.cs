using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.ArchivosClinicos.Commands
{
    public class DeleteArchivClinicoCommand: IRequest<bool>
    {
        public int IdArchivoClinico { get; set; }
    }

    public class DeleteArchivClinicoCommandHandler
        : IRequestHandler<DeleteArchivClinicoCommand, bool>
    {
        private readonly IGenericRepository<ArchivoClinico> _repository;

        public DeleteArchivClinicoCommandHandler(
            IGenericRepository<ArchivoClinico> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteArchivClinicoCommand request,
            CancellationToken cancellationToken)
        {
            var archivo = await _repository.GetByIdAsync(request.IdArchivoClinico);

            if (archivo == null)
            {
                return false;
            }

            await _repository.DeleteAsync(archivo);

            return true;
        }
    }
}

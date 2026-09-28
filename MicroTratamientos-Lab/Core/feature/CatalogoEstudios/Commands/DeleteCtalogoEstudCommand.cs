using Core.feature.ArchivosClinicos.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.CatalogoEstudios.Commands
{
    public class DeleteCtalogoEstudCommand: IRequest<bool>
    {
        public int IdCatalogoEstudio { get; set; }
    }

    public class DeleteCtalogoEstudCommandHandler
       : IRequestHandler<DeleteCtalogoEstudCommand, bool>
    {
        private readonly IGenericRepository<CatalogoEstudio> _repository;

        public DeleteCtalogoEstudCommandHandler(
            IGenericRepository<CatalogoEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteCtalogoEstudCommand request,
            CancellationToken cancellationToken)
        {
            var catalogo = await _repository.GetByIdAsync(request.IdCatalogoEstudio);

            if (catalogo == null)
            {
                return false;
            }

            await _repository.DeleteAsync(catalogo);

            return true;
        }
    }
}

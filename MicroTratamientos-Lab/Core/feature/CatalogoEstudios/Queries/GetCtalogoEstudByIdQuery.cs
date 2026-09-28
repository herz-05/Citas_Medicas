using Core.feature.ArchivosClinicos.Queries;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.CatalogoEstudios.Queries
{
    public class GetCtalogoEstudByIdQuery: IRequest<CatalogoEstudio>
    {
        public int IdCatalogoEstudio { get; set; }
        
    }

    public class GetCtalogoEstudByIdQueryHandler
      : IRequestHandler<GetCtalogoEstudByIdQuery, CatalogoEstudio>
    {
        private readonly IGenericRepository<CatalogoEstudio> _repository;

        public GetCtalogoEstudByIdQueryHandler(
            IGenericRepository<CatalogoEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<CatalogoEstudio> Handle(
            GetCtalogoEstudByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.IdCatalogoEstudio);
        }
    }
}

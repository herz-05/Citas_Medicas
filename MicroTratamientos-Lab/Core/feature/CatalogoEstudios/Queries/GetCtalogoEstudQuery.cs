using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using Generics.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.CatalogoEstudios.Queries
{
    public class GetCtalogoEstudQuery : RequestParametersGets,
         IRequest<PagedResult<CatalogoEstudio>>
    {
    }

    public class GetCtalogoEstudQueryHandler
        : IRequestHandler<GetCtalogoEstudQuery, PagedResult<CatalogoEstudio>>
    {
        private readonly IGenericRepository<CatalogoEstudio> _repository;

        public GetCtalogoEstudQueryHandler(
            IGenericRepository<CatalogoEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<CatalogoEstudio>> Handle(
            GetCtalogoEstudQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<CatalogoEstudio>(request.Filter)
                : null;

            return await _repository.GetPagedAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                filter: filter,
                cancellationToken: cancellationToken
            );
        }
    }
}

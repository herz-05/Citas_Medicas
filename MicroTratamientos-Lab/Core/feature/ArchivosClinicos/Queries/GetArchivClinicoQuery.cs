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

namespace Core.feature.ArchivosClinicos.Queries
{
    public class GetArchivClinicoQuery : RequestParametersGets,
         IRequest<PagedResult<ArchivoClinico>>
    {
    }

   
    

    public class GetArchivClinicoQueryHandler
        : IRequestHandler<GetArchivClinicoQuery, PagedResult<ArchivoClinico>>
    {
        private readonly IGenericRepository<ArchivoClinico> _repository;

        public GetArchivClinicoQueryHandler(
            IGenericRepository<ArchivoClinico> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ArchivoClinico>> Handle(
            GetArchivClinicoQuery request,
            CancellationToken cancellationToken)
        {
            var filter = !string.IsNullOrWhiteSpace(request.Filter)
                ? Filter.FromStringExpression<ArchivoClinico>(request.Filter)
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

using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.ArchivosClinicos.Queries
{
    public class GetArchivClinicoByIdQuery: IRequest<ArchivoClinico>
    {
        public int IdArchivoClinico { get; set; }
    }

    public class GetArchivClinicoByIdQueryHandler
       : IRequestHandler<GetArchivClinicoByIdQuery, ArchivoClinico>
    {
        private readonly IGenericRepository<ArchivoClinico> _repository;

        public GetArchivClinicoByIdQueryHandler(
            IGenericRepository<ArchivoClinico> repository)
        {
            _repository = repository;
        }

        public async Task<ArchivoClinico> Handle(
            GetArchivClinicoByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.IdArchivoClinico);
        }
    }
}

using Domain.Models;
using Generics.Helpers;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Consultorios.Queries
{
    public class GetConsultorioByOneQuery: IRequest<Consultorio>
    {
        public string? Filter { get; set; }
    }

    public class GetConsultorioByOneQueryHandler
       : IRequestHandler<GetConsultorioByOneQuery, Consultorio?>
    {
        private readonly IGenericRepository<Consultorio> _repository;

        public GetConsultorioByOneQueryHandler(
            IGenericRepository<Consultorio> repository)
        {
            _repository = repository;
        }

        public async Task<Consultorio?> Handle(
            GetConsultorioByOneQuery request,
            CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.Filter))
            {
                throw new ArgumentException(
                    "Debe proporcionar un filtro.",
                    nameof(request.Filter));
            }

            var expression =
                Filter.FromStringExpression<Consultorio>(request.Filter);

            return await _repository.GetOneByAsync(
                expression,
                cancellationToken: cancellationToken);
        }
    }
}

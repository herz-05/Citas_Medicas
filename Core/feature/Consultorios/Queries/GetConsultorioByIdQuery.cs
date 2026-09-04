using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Consultorios.Queries
{
    public class GetConsultorioByIdQuery: IRequest<Consultorio?>
    {
        public int IdConsultorio { get; set; }
    }

    public class GetConsultorioByIdQueryHandler
       : IRequestHandler<GetConsultorioByIdQuery, Consultorio?>
    {
        private readonly IGenericRepository<Consultorio> _repository;

        public GetConsultorioByIdQueryHandler(
            IGenericRepository<Consultorio> repository)
        {
            _repository = repository;
        }

        public async Task<Consultorio?> Handle(
            GetConsultorioByIdQuery request,
            CancellationToken cancellationToken)
        {

            return await _repository.GetByIdAsync(
               request.IdConsultorio);
        }
    }
}

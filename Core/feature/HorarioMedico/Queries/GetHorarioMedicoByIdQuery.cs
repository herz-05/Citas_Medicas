using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.HorariosMedicos.Queries
{
    public class GetHorarioMedicoByIdQuery: IRequest<HorariosMedico>
    {
        public int IdHorario {  get; set; }
    }

    public class GetHorarioMedicoByIdQueryHandler
       : IRequestHandler<GetHorarioMedicoByIdQuery, HorariosMedico>
    {
        private readonly IGenericRepository<HorariosMedico> _repository;

        public GetHorarioMedicoByIdQueryHandler(
            IGenericRepository<HorariosMedico> repository)
        {
            _repository = repository;
        }

        public async Task<HorariosMedico> Handle(
            GetHorarioMedicoByIdQuery request,
            CancellationToken cancellationToken)
        {

            return await _repository.GetByIdAsync(
               request.IdHorario);
        }
    }
}

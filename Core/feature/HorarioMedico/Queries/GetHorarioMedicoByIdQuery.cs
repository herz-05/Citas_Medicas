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
    public class GetHorarioMedicoByIdQuery: IRequest<HorarioMedico>
    {
        public int IdHorario {  get; set; }
    }

    public class GetHorarioMedicoByIdQueryHandler
       : IRequestHandler<GetHorarioMedicoByIdQuery, HorarioMedico>
    {
        private readonly IGenericRepository<HorarioMedico> _repository;

        public GetHorarioMedicoByIdQueryHandler(
            IGenericRepository<HorarioMedico> repository)
        {
            _repository = repository;
        }

        public async Task<HorarioMedico> Handle(
            GetHorarioMedicoByIdQuery request,
            CancellationToken cancellationToken)
        {

            return await _repository.GetByIdAsync(
               request.IdHorario);
        }
    }
}

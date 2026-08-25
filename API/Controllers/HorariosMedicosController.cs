using Core.feature.Commands;
using Core.feature.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HorariosMedicosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HorariosMedicosController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<HorarioMedico>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(
                new GetHorarioMedicoQuery
                {
                    TotalRegistros = totalRegistros
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddHorarioMedicoCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
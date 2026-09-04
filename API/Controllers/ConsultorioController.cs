using Core.feature.Consultorios.Commands;
using Core.feature.Consultorios.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsultoriosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConsultoriosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<HorarioMedico>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetConsultorioQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddConsultorioCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
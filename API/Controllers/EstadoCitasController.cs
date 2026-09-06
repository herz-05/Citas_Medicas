
using Core.feature.EstadosCitas.Commands;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstadoCitasontroller : ControllerBase
    {
        private readonly IMediator _mediator;

        public EstadoCitasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<EstadoCitas>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetEstadosCitasQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<EstadoCitas?> GetById(int id)
        {
            return await _mediator.Send(
                new GetEstadosCitasByIdQuery
                {
                    IdPaciente = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddEstadosCitasCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
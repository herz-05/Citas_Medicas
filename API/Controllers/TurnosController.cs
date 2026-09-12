using Core.feature.Turnos.Commands;
using Core.feature.Turnos.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TurnosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TurnosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<Turno>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetTurnosQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<Turno?> GetById(int id)
        {
            return await _mediator.Send(
                new GetTurnosByIdQuery
                {
                    IdTurno = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddTurnosCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
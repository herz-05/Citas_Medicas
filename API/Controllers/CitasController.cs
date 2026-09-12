using Core.feature.Citas.Commands;
using Core.feature.Citas.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CitasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<Cita>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetCitasQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<Cita?> GetById(int id)
        {
            return await _mediator.Send(
                new GetCitasByIdQuery
                {
                    IdCitas = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddCitasCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
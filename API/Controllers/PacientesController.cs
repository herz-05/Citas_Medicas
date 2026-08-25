using Core.feature.Commands;
using Core.feature.Queries;
using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PacientesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<Pacientes>> Get([FromQuery] GetPacienteQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] AddPacienteCommand query)
        {
            return await _mediator.Send(query);
        }
    }
}

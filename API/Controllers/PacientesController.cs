using Core.feature.Consultorios.Queries;
using Core.feature.Paciente.Commands;
using Core.feature.Paciente.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<List<Pacientes>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(new GetPacienteQuery
            {
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<Pacientes?> GetById(int id)
        {
            return await _mediator.Send(
                new GetPacienteByIdQuery
                {
                    IdPaciente = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddPacienteCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
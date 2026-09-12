using Core.feature.ContactosEmergencia.Commands;
using Core.feature.ContactosEmergencia.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactosEmergenciaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactosEmergenciaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<ContactoEmergencia>> Get(
            [FromQuery] int totalRegistros = 0)
        {
            return await _mediator.Send(
                new GetContactosQuery
                {
                    TotalRegistros = totalRegistros
                });
        }

        [HttpGet("{id}")]
        public async Task<ContactoEmergencia?> GetById(int id)
        {
            return await _mediator.Send(
                new GetContactosByIdQuery
                {
                    IdContacto = id
                });
        }

        [HttpPost]
        public async Task<bool> Post(
            [FromBody] AddContactosCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
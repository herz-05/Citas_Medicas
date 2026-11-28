
using Core.feature.Turnos.Commands;
using Core.feature.Turnos.Queries;
using Domain.Models;
using Generics.Models;
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

        // GET - LISTAR TURNOS
        [HttpGet]
        public async Task<ActionResult<PagedResult<Turno>>> Get(
            [FromQuery] GetTurnosQuery query)
        {
            var resultado = await _mediator.Send(
                query,
                HttpContext.RequestAborted);

            return Ok(resultado);
        }

        // GET - CITAS PENDIENTES DE LLEGADA
        [HttpGet("citas-pendientes")]
        public async Task<IActionResult> GetCitasPendientes()
        {
            var resultado = await _mediator.Send(
                new GetCitasPendientesTurnoQuery(),
                HttpContext.RequestAborted);

            return Ok(resultado);
        }

        // GET - TURNO POR ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Turno>> GetById(int id)
        {
            var turno = await _mediator.Send(
                new GetTurnosByIdQuery
                {
                    IdTurno = id
                },
                HttpContext.RequestAborted);

            if (turno == null)
                return NotFound();

            return Ok(turno);
        }

        // POST - REGISTRAR LLEGADA
        [HttpPost("registrar-llegada/{idCita:int}")]
        public async Task<IActionResult> RegistrarLlegada(int idCita)
        {
            var numeroTurno = await _mediator.Send(
                new RegistrarLlegadaTurnoCommand
                {
                    IdCita = idCita
                },
                HttpContext.RequestAborted);

            return Ok(new
            {
                mensaje = "Llegada registrada correctamente.",
                numeroTurno
            });
        }

        // PATCH - CAMBIAR ESTADO
        [HttpPatch("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstado(
            int id,
            [FromBody] UpdateEstadoTurnoCommand command)
        {
            command.IdTurno = id;

            var resultado = await _mediator.Send(
                command,
                HttpContext.RequestAborted);

            if (!resultado)
                return NotFound();

            return Ok(true);
        }
    }
}

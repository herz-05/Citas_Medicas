using Core.feature.ArchivosClinicos.Commands;
using Core.feature.ArchivosClinicos.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArchClinicoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArchClinicoController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR Archivos Clinicos PAGINADAS
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<ArchivoClinico>>> Get(
            [FromQuery] GetArchivClinicoQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }


        // =========================================
        // GET - Archivos Clinicos ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<ArchivoClinico>> GetById(int id)
        {
            var archivo = await _mediator.Send(
                new GetArchivClinicoByIdQuery
                {
                    IdArchivoClinico = id
                }
            );

            if (archivo == null)
            {
                return NotFound();
            }

            return Ok(archivo);
        }


        // =========================================
        // POST - CREAR Archivos Clinicos
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddArchivClinicoCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR Archivos Clinicos
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdateArchivClinicoCommand command)
        {
            command.IdArchivoClinico = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }


        // =========================================
        // DELETE - ELIMINAR Archivos Clinicos
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeleteArchivClinicoCommand
                {
                    IdArchivoClinico = id
                }
            );

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }





    }
}

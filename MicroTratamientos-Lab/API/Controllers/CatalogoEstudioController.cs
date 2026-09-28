using Core.feature.ArchivosClinicos.Commands;
using Core.feature.ArchivosClinicos.Queries;
using Core.feature.CatalogoEstudios.Commands;
using Core.feature.CatalogoEstudios.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogoEstudioController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CatalogoEstudioController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // =========================================
        // GET - LISTAR Catalogo Estudio PAGINADAS
        // =========================================

        [HttpGet]
        public async Task<ActionResult<PagedResult<CatalogoEstudio>>> Get(
            [FromQuery] GetCtalogoEstudQuery query)
        {
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }


        // =========================================
        // GET - Catalogo Estudio ID
        // =========================================

        [HttpGet("{id}")]
        public async Task<ActionResult<ArchivoClinico>> GetById(int id)
        {
            var catalogo = await _mediator.Send(
                new GetCtalogoEstudByIdQuery
                {
                    IdCatalogoEstudio = id
                }
            );

            if (catalogo == null)
            {
                return NotFound();
            }

            return Ok(catalogo);
        }


        // =========================================
        // POST - CREAR Catalogo Estudio
        // =========================================

        [HttpPost]
        public async Task<ActionResult<bool>> Post(
            [FromBody] AddCtalogoEstudCommand command)
        {
            var resultado = await _mediator.Send(command);

            return Ok(resultado);
        }


        // =========================================
        // PUT - ACTUALIZAR Catalogo Estudio
        // =========================================

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(
            int id,
            [FromBody] UpdateCtalogoEstudCommand command)
        {
            command.IdCatalogoEstudio = id;

            var resultado = await _mediator.Send(command);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok(true);
        }


        // =========================================
        // DELETE - ELIMINAR Catalogo Estudio
        // =========================================

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _mediator.Send(
                new DeleteCtalogoEstudCommand
                {
                    IdCatalogoEstudio = id
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

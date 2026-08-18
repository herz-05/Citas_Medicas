using Core.Interface.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly IPacientes _pacientesRepository;

        public PacientesController(IPacientes pacientesRespository)
        {
            _pacientesRepository = pacientesRespository;
        }

        [HttpGet]
        public async Task<List<Pacientes>> Get()
        {
            return await _pacientesRepository.GetPacientesAsync();
        }
    }
}

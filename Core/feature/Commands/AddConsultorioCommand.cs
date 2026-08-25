using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.Commands
{
    public class AddConsultorioCommand : IRequest<bool>
    {
        public int IdConsultorio { get; set; }

        public string? Nombre { get; set; }

        public string? NumeroConsultorio { get; set; }

        public string? Piso { get; set; }

        public string? Ubicacion { get; set; }

        public bool Estado { get; set; }
    }

    public class AddConsultorioCommandHandler
        : IRequestHandler<AddConsultorioCommand, bool>
    {
        private readonly IConsultorios _consultoriosRepository;

        public AddConsultorioCommandHandler(
            IConsultorios consultoriosRepository)
        {
            _consultoriosRepository = consultoriosRepository;
        }

        public async Task<bool> Handle(
            AddConsultorioCommand request,
            CancellationToken cancellationToken)
        {
            Consultorio consultorio = new Consultorio();

            consultorio.IdConsultorio = request.IdConsultorio;
            consultorio.Nombre = request.Nombre ?? string.Empty;
            consultorio.NumeroConsultorio = request.NumeroConsultorio;
            consultorio.Piso = request.Piso;
            consultorio.Ubicacion = request.Ubicacion;
            consultorio.Estado = request.Estado;

            await _consultoriosRepository.AddConsultorio(consultorio);

            return true;
        }
    }
}
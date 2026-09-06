using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.EstadosCitas.Commands
{
    public class AddEstadosCitasCommand: IRequest<bool>
    {
        public int IdEstadoCita { get; set; }
        public string NombreEstado { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class AddEstadosCitasCommandHandler
     : IRequestHandler<AddEstadosCitasCommand, bool>
    {
        private readonly IGenericRepository<EstadoCitas> _repository;

        public AddEstadosCitasCommandHandler(
            IGenericRepository<EstadoCitas> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddEstadosCitasCommand request,
            CancellationToken cancellationToken)
        {
            var estadosCitas = new EstadoCitas
            {
                NombreEstado = request.NombreEstado,
                Descripcion = request.Descripcion,
            };

            await _repository.AddAsync(estadosCitas);

            return true;
        }
    }
}

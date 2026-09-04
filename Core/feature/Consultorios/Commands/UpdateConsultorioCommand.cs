using Core.Interface.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Consultorios.Commands
{
    public class UpdateConsultorioCommand: IRequest<bool>
    {
        public int IdConsultorio { get; set; }

        public string? Nombre { get; set; }

        public string? NumeroConsultorio { get; set; }

        public string? Piso { get; set; }

        public string? Ubicacion { get; set; }

        public bool Estado { get; set; }
    }

    public class UpdateConsultorioCommandHandler
       : IRequestHandler<UpdateConsultorioCommand, bool>
    {
        private readonly IGenericRepository<Consultorio> _repository;

        public UpdateConsultorioCommandHandler(
            IGenericRepository<Consultorio> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateConsultorioCommand request,
            CancellationToken cancellationToken)
        {
            var consultorio = await _repository.GetByIdAsync(
                request.IdConsultorio);

            if (consultorio == null)
            {
                return false;
            }

            consultorio.Nombre = request.Nombre ?? string.Empty;
            consultorio.NumeroConsultorio = request.NumeroConsultorio;
            consultorio.Piso = request.Piso;
            consultorio.Ubicacion = request.Ubicacion;
            consultorio.Estado = request.Estado;

            await _repository.UpdateAsync(consultorio);

            return true;
        }
    }

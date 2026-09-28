using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.ArchivosClinicos.Commands
{
    public class UpdateArchivClinicoCommand: IRequest<bool>
    {
        public int IdArchivoClinico { get; set; }
        public int? IdResultadoEstudio { get; set; }
        public int? IdPaciente { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string TipoMime { get; set; }
        public string RutaAlmacenamiento { get; set; }
        public long TamanoBytes { get; set; }
        public DateTime FechaSubida { get; set; }
        public ResultadoEstudio ResultadoEstudio { get; set; }
    }

    public class UpdateArchivClinicoCommandHandler
        : IRequestHandler<UpdateArchivClinicoCommand, bool>
    {
        private readonly IGenericRepository<ArchivoClinico> _repository;

        public UpdateArchivClinicoCommandHandler(
            IGenericRepository<ArchivoClinico> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateArchivClinicoCommand request,
            CancellationToken cancellationToken)
        {
            var archivo = await _repository.GetByIdAsync(request.IdArchivoClinico);

            if (archivo == null)
            {
                return false;
            }

            archivo.IdResultadoEstudio = request.IdResultadoEstudio;
            archivo.IdPaciente = request.IdPaciente;
            archivo.NombreArchivo = request.NombreArchivo;
            archivo.Extension = request.Extension;
            archivo.TipoMime = request.TipoMime;
            archivo.RutaAlmacenamiento = request.RutaAlmacenamiento;
            archivo.FechaSubida = request.FechaSubida;
            archivo.ResultadoEstudio = request.ResultadoEstudio;

            await _repository.UpdateAsync(archivo);

            return true;
        }
    }
}

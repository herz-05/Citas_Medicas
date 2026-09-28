using Core.feature.ArchivosClinicos.Commands;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.CatalogoEstudios.Commands
{
    public class AddCtalogoEstudCommand: IRequest<bool>
    {
        public int IdCatalogoEstudio { get; set; }
        public string Codigo { get; set; }
        public string NombreEstudio { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public List<SolicitudEstudioDetalle> SolicitudEstudioDetalles { get; set; }
    }

    public class AddCtalogoEstudCommandHandler
        : IRequestHandler<AddCtalogoEstudCommand, bool>
    {
        private readonly IGenericRepository<CatalogoEstudio> _repository;

        public AddCtalogoEstudCommandHandler(
            IGenericRepository<CatalogoEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddCtalogoEstudCommand request,
            CancellationToken cancellationToken)
        {
            var catalogo = new CatalogoEstudio
            {
                Codigo = request.Codigo,
                NombreEstudio = request.NombreEstudio,
                Categoria = request.Categoria,
                Descripcion = request.Descripcion,
                Activo = request.Activo,
                SolicitudEstudioDetalles = request.SolicitudEstudioDetalles
                

            };

            await _repository.AddAsync(catalogo);

            return true;
        }
    }
}

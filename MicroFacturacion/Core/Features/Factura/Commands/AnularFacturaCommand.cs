using Core.Facturacion.Exceptions;
using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.Facturacion.Features.Facturas.Commands
{
    public class AnularFacturaCommand : IRequest<bool>
    {
        public int IdFactura { get; set; }
    }

    public class AnularFacturaCommandHandler
        : IRequestHandler<AnularFacturaCommand, bool>
    {
        private readonly IGenericRepository<Factura> _repository;

        public AnularFacturaCommandHandler(
            IGenericRepository<Factura> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AnularFacturaCommand request,
            CancellationToken cancellationToken)
        {
            var factura = await _repository.GetByIdAsync(
                request.IdFactura,
                cancellationToken
            );

            if (factura == null)
            {
                throw new DomainException(
                    Errores.FACTURA_NO_EXISTE,
                    "La factura indicada no existe."
                );
            }

            if (factura.Estado.Equals(
                "CANCELADA",
                StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            factura.Estado = "CANCELADA";

            await _repository.UpdateAsync(
                factura,
                cancellationToken
            );

            return true;
        }
    }
}
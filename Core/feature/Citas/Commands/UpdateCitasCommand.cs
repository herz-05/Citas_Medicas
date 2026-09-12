using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.Citas.Commands
{
    public class UpdateCitasCommand : IRequest<bool>
    {
        public int IdCita { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int IdConsultorio { get; set; }
        public int IdEstadoCita { get; set; }
        public DateOnly FechaCita { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public string? MotivoConsulta { get; set; }
        public string? Observaciones { get; set; }
    }

    public class UpdateCitasCommandHandler
        : IRequestHandler<UpdateCitasCommand, bool>
    {
        private readonly IGenericRepository<Cita> _repository;

        public UpdateCitasCommandHandler(
            IGenericRepository<Cita> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateCitasCommand request,
            CancellationToken cancellationToken)
        {
            var cita = await _repository.GetByIdAsync(request.IdCita);

            if (cita == null)
            {
                return false;
            }

            cita.IdPaciente = request.IdPaciente;
            cita.IdMedico = request.IdMedico;
            cita.IdConsultorio = request.IdConsultorio;
            cita.IdEstadoCita = request.IdEstadoCita;
            cita.FechaCita = request.FechaCita;
            cita.HoraInicio = request.HoraInicio;
            cita.HoraFin = request.HoraFin;
            cita.MotivoConsulta = request.MotivoConsulta;
            cita.Observaciones = request.Observaciones;

            await _repository.UpdateAsync(cita);

            return true;
        }
    }
}
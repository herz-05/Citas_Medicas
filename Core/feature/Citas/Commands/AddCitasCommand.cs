
using Core.Interface.Repositories;
using Domain.Models;
using MediatR;


namespace Core.feature.Citas.Commands
{
    public class AddCitasCommand: IRequest<bool>
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
        public DateTime FechaRegistro { get; set; }
    }


    public class AddCitasCommandHandler
       : IRequestHandler<AddCitasCommand, bool>
    {
        private readonly IGenericRepository<Cita> _repository;

        public AddCitasCommandHandler(
            IGenericRepository<Cita> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddCitasCommand request,
            CancellationToken cancellationToken)
        {
            var citas = new Cita
            {
                IdPaciente = request.IdPaciente,
                IdMedico = request.IdMedico,
                IdConsultorio = request.IdConsultorio,
                IdEstadoCita = request.IdEstadoCita,
                FechaCita = request.FechaCita,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                MotivoConsulta = request.MotivoConsulta,
                Observaciones = request.Observaciones,
                FechaRegistro = request.FechaRegistro,
            };

            await _repository.AddAsync(citas);

            return true;
        }
    }
}

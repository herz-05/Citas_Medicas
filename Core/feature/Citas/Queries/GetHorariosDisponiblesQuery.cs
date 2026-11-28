
using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.HorarioMedico.Queries
{
    public class GetHorariosDisponiblesQuery
        : IRequest<List<HorarioDisponibleDto>>
    {
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public DateOnly Fecha { get; set; }
    }

    public class HorarioDisponibleDto
    {
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public int IdConsultorio { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }

    public class CitaOcupacionDto
    {
        public int IdMedico { get; set; }
        public int IdConsultorio { get; set; }
        public int IdEstadoCita { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }

    public class GetHorariosDisponiblesQueryHandler
        : IRequestHandler<
            GetHorariosDisponiblesQuery,
            List<HorarioDisponibleDto>>
    {
        private const int DuracionCitaMinutos = 30;

        private readonly IGenericRepository<HorariosMedico>
            _horarioRepository;

        private readonly IGenericRepository<Cita>
            _citaRepository;

        private readonly IGenericRepository<EstadoCitas>
            _estadoRepository;

        public GetHorariosDisponiblesQueryHandler(
            IGenericRepository<HorariosMedico> horarioRepository,
            IGenericRepository<Cita> citaRepository,
            IGenericRepository<EstadoCitas> estadoRepository)
        {
            _horarioRepository = horarioRepository;
            _citaRepository = citaRepository;
            _estadoRepository = estadoRepository;
        }

        public async Task<List<HorarioDisponibleDto>> Handle(
            GetHorariosDisponiblesQuery request,
            CancellationToken cancellationToken)
        {
            if (request.IdMedico <= 0)
                throw new ArgumentException(
                    "Debe seleccionar un médico.");

            if (request.IdEspecialidad <= 0)
                throw new ArgumentException(
                    "Debe seleccionar una especialidad.");

            var hoy = DateOnly.FromDateTime(DateTime.Today);

            if (request.Fecha < hoy)
                throw new ArgumentException(
                    "No se permiten consultar fechas anteriores a hoy.");

            var diaSemana = ObtenerDiaSemana(request.Fecha);

            // Consultar horarios del médico y especialidad.
            var horariosResultado =
                await _horarioRepository.GetPagedAsync(
                    pageNumber: 1,
                    pageSize: 1000,
                    filter: h =>
                        h.IdMedico == request.IdMedico &&
                        h.IdEspecialidad == request.IdEspecialidad &&
                        h.Estado &&
                        h.DiaSemana == diaSemana,
                    cancellationToken: cancellationToken
                );

            var horarios = horariosResultado.Data.ToList();

            if (horarios.Count == 0)
                return new List<HorarioDisponibleDto>();

            // Obtener estados que liberan horarios.
            var estados = await _estadoRepository.GetAllAsync(
                cancellationToken);

            var estadosCancelados = estados
                .Where(e =>
                    !string.IsNullOrWhiteSpace(e.NombreEstado) &&
                    (
                        e.NombreEstado.Trim().Equals(
                            "Cancelada",
                            StringComparison.OrdinalIgnoreCase) ||
                        e.NombreEstado.Trim().Equals(
                            "Cancelado",
                            StringComparison.OrdinalIgnoreCase) ||
                        e.NombreEstado.Trim().Equals(
                            "Anulada",
                            StringComparison.OrdinalIgnoreCase) ||
                        e.NombreEstado.Trim().Equals(
                            "Anulado",
                            StringComparison.OrdinalIgnoreCase)
                    ))
                .Select(e => e.IdEstadoCita)
                .ToHashSet();

            // Recuperar únicamente las columnas necesarias.
            var citasDelDia = await _citaRepository.GetProjectedAsync(
                c => c.FechaCita == request.Fecha,

                c => new CitaOcupacionDto
                {
                    IdMedico = c.IdMedico,
                    IdConsultorio = c.IdConsultorio,
                    IdEstadoCita = c.IdEstadoCita,
                    HoraInicio = c.HoraInicio,
                    HoraFin = c.HoraFin
                },

                cancellationToken
            );

            // Excluir citas canceladas y anuladas.
            citasDelDia = citasDelDia
                .Where(c =>
                    !estadosCancelados.Contains(c.IdEstadoCita))
                .ToList();

            var disponibles = new List<HorarioDisponibleDto>();

            var horaActual = TimeOnly.FromDateTime(DateTime.Now);

            foreach (var horario in horarios)
            {
                var inicioJornada =
                    TimeOnly.FromTimeSpan(horario.HoraInicio);

                var finJornada =
                    TimeOnly.FromTimeSpan(horario.HoraFin);

                var horaInicio = inicioJornada;

                while (horaInicio.AddMinutes(DuracionCitaMinutos)
                       <= finJornada)
                {
                    var horaFin = horaInicio.AddMinutes(
                        DuracionCitaMinutos);

                    // Excluir horarios que ya pasaron.
                    if (request.Fecha == hoy &&
                        horaInicio <= horaActual)
                    {
                        horaInicio = horaFin;
                        continue;
                    }

                    // Verificar ocupación del médico o consultorio.
                    var ocupado = citasDelDia.Any(c =>
                        (
                            c.IdMedico == request.IdMedico ||
                            c.IdConsultorio == horario.IdConsultorio
                        ) &&
                        horaInicio < c.HoraFin &&
                        horaFin > c.HoraInicio
                    );

                    if (!ocupado)
                    {
                        disponibles.Add(
                            new HorarioDisponibleDto
                            {
                                IdMedico = horario.IdMedico,
                                IdEspecialidad = horario.IdEspecialidad,
                                IdConsultorio = horario.IdConsultorio,
                                Fecha = request.Fecha,
                                HoraInicio = horaInicio,
                                HoraFin = horaFin
                            });
                    }

                    horaInicio = horaFin;
                }
            }

            return disponibles
                .OrderBy(h => h.HoraInicio)
                .ToList();
        }

        private static string ObtenerDiaSemana(DateOnly fecha)
        {
            return fecha.DayOfWeek switch
            {
                DayOfWeek.Monday => "Lunes",
                DayOfWeek.Tuesday => "Martes",
                DayOfWeek.Wednesday => "Miércoles",
                DayOfWeek.Thursday => "Jueves",
                DayOfWeek.Friday => "Viernes",
                DayOfWeek.Saturday => "Sábado",
                DayOfWeek.Sunday => "Domingo",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}

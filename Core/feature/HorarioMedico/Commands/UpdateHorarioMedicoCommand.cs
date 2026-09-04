using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.HorariosMedicos.Commands
{
    public class UpdateHorarioMedicoCommand: IRequest<bool>
    {
        public int IdHorario { get; set; }

        public int IdMedico { get; set; }

        public int IdConsultorio { get; set; }

        public string DiaSemana { get; set; } = string.Empty;

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public bool Estado { get; set; }
    }

        public class UpdateHorarioMedicoCommandHandler
          : IRequestHandler<UpdateHorarioMedicoCommand, bool>
        {
            private readonly IGenericRepository<HorarioMedico> _repository;

            public UpdateHorarioMedicoCommandHandler(
                IGenericRepository<HorarioMedico> repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(
                UpdateHorarioMedicoCommand request,
                CancellationToken cancellationToken)
            {
                var horarios = await _repository.GetByIdAsync(
                    request.IdHorario);

                if (horarios == null)
                {
                    return false;
                }

                horarios.IdMedico = request.IdMedico;
                horarios.IdConsultorio = request.IdConsultorio;
                horarios.DiaSemana = request.DiaSemana;
                horarios.HoraInicio = request.HoraInicio;
                horarios.HoraFin = request.HoraFin;
                horarios.Estado = request.Estado;

                await _repository.UpdateAsync(horarios);

                return true;
            }
        }
    }

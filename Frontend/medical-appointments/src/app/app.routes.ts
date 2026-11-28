import { Routes } from '@angular/router';

import { MainLayout } from './layout/main-layout/main-layout';
import { Inicio } from './layout/inicio/inicio';
import { Login } from './features/Auth/login/login';

import { Pacientes } from './features/MicroPacientesAgendas/pacientes/pacientes';
import { Consultorios } from './features/MicroPacientesAgendas/consultorios/consultorios';
import { HorarioMedico } from './features/MicroPacientesAgendas/horario-medico/horario-medico';
import { AgendaCitas } from './features/MicroPacientesAgendas/agenda-citas/agenda-citas';
import { TurnosComponent } from './features/MicroPacientesAgendas/turnos/turnos/turnos';

import { Servicios } from './features/MicroFacturacion/servicios/servicios';
import { Facturas } from './features/MicroFacturacion/facturas/facturas';
import { Pagos } from './features/MicroFacturacion/pagos/pagos';

import { Medicamentos } from './features/MicroTratamiento-Lab/medicamentos/medicamentos';
//import { CatalogoEstudios } from './features/MicroTratamiento-Lab/catalogo-estudios/catalogo-estudios';
import { Recetas } from './features/MicroTratamiento-Lab/recetas/recetas';
//import { OrdenesLaboratorio } from './features/MicroTratamiento-Lab/ordenes-laboratorio/ordenes-laboratorio';
//import { Tratamientos } from './features/MicroTratamiento-Lab/tratamientos/tratamientos';

import { ExpedientesClinicos } from './features/MicroExpedientesClinicos/expediente-clinico/expediente-clinico';
import { Atenciones } from './features/MicroExpedientesClinicos/atenciones/atenciones';
import { Diagnosticos } from './features/MicroExpedientesClinicos/diagnosticos/diagnosticos';
import { Sintomas } from './features/MicroExpedientesClinicos/sintomas/sintomas';

import { authGuard, authChildGuard } from './core/Auth/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    component: Login
  },
  {
    path: '',
    component: MainLayout,
    canActivate: [authGuard],
    canActivateChild: [authChildGuard],
    children: [
      { path: 'inicio', component: Inicio },

      { path: 'pacientes', component: Pacientes },
      { path: 'consultorios', component: Consultorios },
      { path: 'horarios-medicos', component: HorarioMedico },
      { path: 'agenda-citas', component: AgendaCitas },
      { path: 'turnos', component: TurnosComponent },

      { path: 'servicios', component: Servicios },
      { path: 'facturas', component: Facturas },
      { path: 'pagos', component: Pagos },

      { path: 'medicamentos', component: Medicamentos },
      //{ path: 'catalogo-estudios', component: CatalogoEstudios },
      { path: 'prescripciones', component: Recetas },
      //{ path: 'ordenes-laboratorio', component: OrdenesLaboratorio },
      //{ path: 'tratamientos', component: Tratamientos },

      { path: 'expediente-clinico', component: ExpedientesClinicos },
      { path: 'atenciones', component: Atenciones },
      { path: 'diagnosticos', component: Diagnosticos },
      { path: 'sintomas', component: Sintomas },

      { path: '', redirectTo: 'inicio', pathMatch: 'full' }
    ]
  },
  {
    path: '**',
    redirectTo: 'inicio'
  }
];
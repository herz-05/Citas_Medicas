import { Routes } from '@angular/router';

import { MainLayout } from './layout/main-layout/main-layout';

import { Pacientes } from './pacientes/pacientes';
import { Consultorios } from './consultorios/consultorios';
import { HorarioMedico } from './horario-medico/horario-medico';

export const routes: Routes = [

  {
    path: '',
    component: MainLayout,

    children: [

      {
        path: 'pacientes',
        component: Pacientes
      },

      {
        path: 'consultorios',
        component: Consultorios
      },

      {
        path: 'horarios',
        component: HorarioMedico
      },

      {
        path: '',
        redirectTo: 'pacientes',
        pathMatch: 'full'
      }

    ]
  }

];
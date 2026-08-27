import { TestBed } from '@angular/core/testing';

import { HorarioMedico } from './horario-medico';

describe('HorarioMedico', () => {
  let service: HorarioMedico;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HorarioMedico);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

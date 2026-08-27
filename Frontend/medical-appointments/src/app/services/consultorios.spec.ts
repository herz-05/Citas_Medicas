import { TestBed } from '@angular/core/testing';

import { Consultorios } from './consultorios';

describe('Consultorios', () => {
  let service: Consultorios;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Consultorios);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

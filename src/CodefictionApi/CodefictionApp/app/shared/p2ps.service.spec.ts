import { TestBed, inject } from '@angular/core/testing';

import { P2psService } from './p2ps.service';

describe('P2psService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [P2psService]
    });
  });

  it('should be created', inject([P2psService], (service: P2psService) => {
    expect(service).toBeTruthy();
  }));
});

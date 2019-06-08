import { TestBed, async, inject } from '@angular/core/testing';

import { PetCareGuard } from './pet-care.guard';

describe('PetCareGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PetCareGuard]
    });
  });

  it('should ...', inject([PetCareGuard], (guard: PetCareGuard) => {
    expect(guard).toBeTruthy();
  }));
});

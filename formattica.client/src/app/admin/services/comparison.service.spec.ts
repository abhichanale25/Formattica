import { TestBed } from '@angular/core/testing';

import { ComparisionService } from './comparison.service';

describe('ComparisionService', () => {
  let service: ComparisionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ComparisionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

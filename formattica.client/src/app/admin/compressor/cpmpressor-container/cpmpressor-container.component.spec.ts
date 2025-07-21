import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CpmpressorContainerComponent } from './cpmpressor-container.component';

describe('CpmpressorContainerComponent', () => {
  let component: CpmpressorContainerComponent;
  let fixture: ComponentFixture<CpmpressorContainerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CpmpressorContainerComponent]
    });
    fixture = TestBed.createComponent(CpmpressorContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

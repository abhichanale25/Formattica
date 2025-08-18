import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PdfCompressorComponent } from './pdf-compressor.component';

describe('PdfCompressorComponent', () => {
  let component: PdfCompressorComponent;
  let fixture: ComponentFixture<PdfCompressorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PdfCompressorComponent]
    });
    fixture = TestBed.createComponent(PdfCompressorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

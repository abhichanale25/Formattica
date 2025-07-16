import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImgZipCompressorComponent } from './img-zip-compressor.component';

describe('ImgZipCompressorComponent', () => {
  let component: ImgZipCompressorComponent;
  let fixture: ComponentFixture<ImgZipCompressorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImgZipCompressorComponent]
    });
    fixture = TestBed.createComponent(ImgZipCompressorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

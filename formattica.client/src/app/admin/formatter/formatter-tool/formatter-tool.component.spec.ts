import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormatterToolComponent } from './formatter-tool.component';

describe('FormatterToolComponent', () => {
  let component: FormatterToolComponent;
  let fixture: ComponentFixture<FormatterToolComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FormatterToolComponent]
    });
    fixture = TestBed.createComponent(FormatterToolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

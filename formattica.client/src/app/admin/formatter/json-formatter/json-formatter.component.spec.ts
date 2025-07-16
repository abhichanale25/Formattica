import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JsonFormatterComponent } from './json-formatter.component';

describe('JsonFormatterComponent', () => {
  let component: JsonFormatterComponent;
  let fixture: ComponentFixture<JsonFormatterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [JsonFormatterComponent]
    });
    fixture = TestBed.createComponent(JsonFormatterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

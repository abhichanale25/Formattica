import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormatterRoutingModule } from './formatter-routing.module';
import { JsonFormatterComponent } from './json-formatter/json-formatter.component';


@NgModule({
  declarations: [
    JsonFormatterComponent
  ],
  imports: [
    CommonModule,
    FormatterRoutingModule
  ]
})
export class FormatterModule { }

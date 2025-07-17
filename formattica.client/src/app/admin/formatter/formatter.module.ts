import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormatterRoutingModule } from './formatter-routing.module';
import { JsonFormatterComponent } from './json-formatter/json-formatter.component';
import { FormatterToolComponent } from './formatter-tool/formatter-tool.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    JsonFormatterComponent,
    FormatterToolComponent,
    FormatterToolComponent
  ],
  imports: [
    CommonModule,
    FormatterRoutingModule,
    FormsModule
  ]
})
export class FormatterModule { }

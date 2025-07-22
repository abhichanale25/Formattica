import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormatterRoutingModule } from './formatter-routing.module';
import { FormatterToolComponent } from './formatter-tool/formatter-tool.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    FormatterToolComponent,
  ],
  imports: [
    CommonModule,
    FormatterRoutingModule,
    FormsModule
  ]
})
export class FormatterModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ComparisonRoutingModule } from './comparison-routing.module';
import { ComparisionComponent } from './comparision/comparision.component';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    ComparisionComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ComparisonRoutingModule
  ]
})
export class ComparisonModule { }

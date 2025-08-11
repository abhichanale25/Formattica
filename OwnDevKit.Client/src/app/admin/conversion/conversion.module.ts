import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConversionRoutingModule } from './conversion-routing.module';
import { ConversionComponent } from './conversion/conversion.component';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    ConversionComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ConversionRoutingModule
  ]
})
export class ConversionModule { }

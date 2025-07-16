import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CompressorRoutingModule } from './compressor-routing.module';
import { PdfCompressorComponent } from './pdf-compressor/pdf-compressor.component';
import { ImgZipCompressorComponent } from './img-zip-compressor/img-zip-compressor.component';


@NgModule({
  declarations: [
    PdfCompressorComponent,
    ImgZipCompressorComponent
  ],
  imports: [
    CommonModule,
    CompressorRoutingModule
  ]
})
export class CompressorModule { }

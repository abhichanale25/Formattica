import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CompressorRoutingModule } from './compressor-routing.module';
import { ImeZipComponent } from './ime-zip/ime-zip.component';
import { ImgZipComponent } from './img-zip/img-zip.component';
import { PdfComponent } from './pdf/pdf.component';
import { PdfCompressorComponent } from './pdf-compressor/pdf-compressor.component';
import { ImgZipCompressorComponent } from './img-zip-compressor/img-zip-compressor.component';


@NgModule({
  declarations: [
    ImeZipComponent,
    ImgZipComponent,
    PdfComponent,
    PdfCompressorComponent,
    ImgZipCompressorComponent
  ],
  imports: [
    CommonModule,
    CompressorRoutingModule
  ]
})
export class CompressorModule { }

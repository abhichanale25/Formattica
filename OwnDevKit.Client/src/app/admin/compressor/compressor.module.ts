import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CompressorRoutingModule } from './compressor-routing.module';
import { PdfCompressorComponent } from './pdf-compressor/pdf-compressor.component';
import { ImgZipCompressorComponent } from './img-zip-compressor/img-zip-compressor.component';
import { RouterModule } from '@angular/router';
import { CpmpressorContainerComponent } from './cpmpressor-container/cpmpressor-container.component';


@NgModule({
  declarations: [
    PdfCompressorComponent,
    ImgZipCompressorComponent,
    CpmpressorContainerComponent
  ],
  imports: [
    CommonModule,
    RouterModule, 
    CompressorRoutingModule
  ]
})
export class CompressorModule { }

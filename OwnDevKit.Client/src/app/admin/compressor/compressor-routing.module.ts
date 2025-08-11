import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ImgZipCompressorComponent } from './img-zip-compressor/img-zip-compressor.component';
import { PdfCompressorComponent } from './pdf-compressor/pdf-compressor.component';
import { CpmpressorContainerComponent } from './cpmpressor-container/cpmpressor-container.component';

const routes: Routes = [

  // { path:'', component:CpmpressorContainerComponent },
  // { path:"img-zip", component:ImgZipCompressorComponent },
  { path:"pdf", component:PdfCompressorComponent },

  {
    path: '',
    component: CpmpressorContainerComponent,
    children: [
      { path: 'img-zip', component: ImgZipCompressorComponent },
      { path: 'pdf', component: PdfCompressorComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CompressorRoutingModule { }

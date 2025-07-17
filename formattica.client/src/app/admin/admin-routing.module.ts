import { Component, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ImgZipCompressorComponent } from './compressor/img-zip-compressor/img-zip-compressor.component';

const routes: Routes = [
//   {
//     path: 'home',
//     component: ImgZipCompressorComponent,
//   },
  {
    path: 'compressor',
    loadChildren: () =>
      import('src/app/admin/compressor/compressor.module').then(
        (m) => m.CompressorModule
      )
  },
  {
    path:"conversion",
    loadChildren: () =>
        import("src/app/admin/conversion/conversion.module").then((m)=> m.ConversionModule)
  },
  {
    path:"comparison",
    loadChildren:() =>
        import("src/app/admin/comparison/comparison.module").then((m)=> m.ComparisonModule)
  },
  {path:"formatter",
    loadChildren:() =>
        import("src/app/admin/formatter/formatter.module").then((m) => m.FormatterModule)
  },
  { path:'', redirectTo:'compressor', pathMatch:'full'}
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}

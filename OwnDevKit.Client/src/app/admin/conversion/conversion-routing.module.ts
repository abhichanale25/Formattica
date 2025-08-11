import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConversionComponent } from './conversion/conversion.component';

const routes: Routes = [
  {path:'codeConversion', component:ConversionComponent}
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConversionRoutingModule { }

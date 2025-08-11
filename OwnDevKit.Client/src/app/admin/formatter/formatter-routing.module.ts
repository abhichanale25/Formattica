import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormatterToolComponent } from './formatter-tool/formatter-tool.component';

const routes: Routes = [
  {path:"formattertool", component:FormatterToolComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FormatterRoutingModule { }

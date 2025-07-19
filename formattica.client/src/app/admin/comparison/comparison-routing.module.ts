import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ComparisionComponent } from './comparision/comparision.component';

const routes: Routes = [
  {path:"codeComparision", component: ComparisionComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ComparisonRoutingModule { }

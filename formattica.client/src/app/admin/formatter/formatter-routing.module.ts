import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JsonFormatterComponent } from './json-formatter/json-formatter.component';
import { FormatterToolComponent } from './formatter-tool/formatter-tool.component';

const routes: Routes = [
  {path:"json", component:JsonFormatterComponent},
  {path:"formatter", component:FormatterToolComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FormatterRoutingModule { }

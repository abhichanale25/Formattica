import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path:"admin",
        loadChildren: () =>
            import("src/app/admin/admin.module").then((m) => m.AdminModule)
    },
    { path: '', redirectTo:"admin", pathMatch:"full"},
    // { path: "*", redirectTo:"admin", pathMatch:"full"}
    
]

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

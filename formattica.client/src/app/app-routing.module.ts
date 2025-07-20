import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  {path: "home", component: HomeComponent},
    {
        path:"admin",
        loadChildren: () =>
            import("src/app/admin/admin.module").then((m) => m.AdminModule)
    },
    { path: '', redirectTo: 'home', pathMatch: 'full' },   // ✅ Landing page is /home

  { path: '**', redirectTo: 'home' }                      // ✅ Wildcard route fallback
    
]

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenubarComponent } from './menubar/menubar.component';
import { AdminRoutingModule } from './admin-routing.module';




@NgModule({
  declarations: [ MenubarComponent],
  imports: [
    CommonModule,
    AdminRoutingModule,
  ],
  exports:[MenubarComponent]
})
export class AdminModule { }

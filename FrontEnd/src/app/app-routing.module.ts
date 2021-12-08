import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppointmentComponent } from './appointment/appointment.component';
import { AppointmentsListComponent } from './appointments-list/appointments-list.component';
import { SettingComponent } from './setting/setting.component';


const routes: Routes = [
  { path: 'Setting', component: SettingComponent },
  { path: 'CreateAppoint', component: AppointmentComponent },
  { path: 'AppointList', component: AppointmentsListComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

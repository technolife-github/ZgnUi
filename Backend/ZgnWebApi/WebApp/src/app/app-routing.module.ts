import { LoginGuard } from './guards/login.guard';
import { ManagementComponent } from './pages/management/management.component';
import { LoginComponent } from './pages/login/login.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {path:'login', component: LoginComponent},
  {path:'', component: ManagementComponent, canActivate:[LoginGuard], loadChildren: () => import('./pages/management/management.module').then(m => m.ManagementModule)},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component'
import { OwnerComponent } from './owner/owner.component'

import { AuthGuard } from './services/guard/auth.guard'


const routes: Routes = [
  {
  	path: '',
  	redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'owner/profile',
    component: OwnerComponent,
    canActivate: [AuthGuard]
  },
  {
    path: '**',
    component: LoginComponent,
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

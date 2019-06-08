import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component'
import { OwnerComponent } from './owner/owner.component'

import { OwnerGuard } from './services/auth/owner-guard/owner.guard'


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
    canActivate: [OwnerGuard]
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

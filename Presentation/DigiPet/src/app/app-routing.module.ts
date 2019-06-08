import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

import { AdminComponent } from './admin/admin.component';
import { OwnerComponent } from './owner/owner.component';
import { PetCareComponent } from './pet-care/pet-care.component';

import { AdminGuard } from './services/auth/admin-guard/admin.guard';
import { OwnerGuard } from './services/auth/owner-guard/owner.guard';
import { PetCareGuard } from './services/auth/pet-care-guard/pet-care.guard';


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
    path: 'signup',
    component: RegisterComponent,
  },
  {
    path: 'admin/profile',
    component: AdminComponent,
    canActivate: [AdminGuard]
  },
  {
    path: 'owner/profile',
    component: OwnerComponent,
    canActivate: [OwnerGuard]
  },
  {
    path: 'petcare/profile',
    component: PetCareComponent,
    canActivate: [PetCareGuard]
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

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';

import { MatTabsModule } from '@angular/material/tabs';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { AuthService } from './services/auth/auth.service';
import { AdminGuard } from './services/auth/admin-guard/admin.guard';
import { OwnerGuard } from './services/auth/owner-guard/owner.guard';
import { PetCareGuard } from './services/auth/pet-care-guard/pet-care.guard';

import { EncryptionService } from './services/encryption/encryption.service'

import { Urls } from './configuration/urls';

import { AdminComponent } from './admin/admin.component';
import { LoginComponent } from './login/login.component';
import { LoginNavbarComponent } from './login/login-navbar/login-navbar.component';
import { OwnerComponent } from './owner/owner.component';
import { PetCareComponent } from './pet-care/pet-care.component';
import { RegisterComponent } from './register/register.component';
import { RegisterNavbarComponent } from './register/register-navbar/register-navbar.component';
import { TabOwnerComponent } from './register/tab-owner/tab-owner.component';
import { TabPetCareComponent } from './register/tab-pet-care/tab-pet-care.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


@NgModule({
  declarations: [
    AdminComponent,
    AppComponent,
    LoginComponent,
    LoginNavbarComponent,
    OwnerComponent,
    PetCareComponent,
    RegisterComponent,
    RegisterNavbarComponent,
    TabOwnerComponent,
    TabPetCareComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    MatTabsModule,
    ReactiveFormsModule
  ],
  providers: [
    AdminGuard,
    AuthService,
    EncryptionService,
    OwnerGuard,
    PetCareGuard,
  	Urls
  ],
  bootstrap: [
  	AppComponent
  ]
})
export class AppModule { }

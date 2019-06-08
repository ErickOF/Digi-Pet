import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { Urls } from './configuration/urls';
import { LoginComponent } from './login/login.component';
import { OwnerComponent } from './owner/owner.component';
import { NavbarComponent } from './login/navbar/navbar.component';
import { AdminComponent } from './admin/admin.component';
import { PetCareComponent } from './pet-care/pet-care.component';
import { RegisterComponent } from './register/register.component';
import { TabOwnerComponent } from './register/tab-owner/tab-owner.component';
import { TabPetCareComponent } from './register/tab-pet-care/tab-pet-care.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    OwnerComponent,
    NavbarComponent,
    AdminComponent,
    PetCareComponent,
    RegisterComponent,
    TabOwnerComponent,
    TabPetCareComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
  	Urls
  ],
  bootstrap: [
  	AppComponent
  ]
})
export class AppModule { }

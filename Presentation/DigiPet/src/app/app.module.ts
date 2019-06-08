import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { Urls } from './configuration/urls';
import { LoginComponent } from './login/login.component';
import { OwnerComponent } from './owner/owner.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { AdminComponent } from './admin/admin.component';
import { PetCareComponent } from './pet-care/pet-care.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    OwnerComponent,
    NavbarComponent,
    AdminComponent,
    PetCareComponent
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

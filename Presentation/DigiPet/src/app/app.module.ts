import { BrowserModule } from '@angular/platform-browser';
import { FileUploadModule } from 'ng2-file-upload';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';

import { AngularFireModule } from 'angularfire2';
import { AngularFireStorageModule } from 'angularfire2/storage';

import { MatTableModule } from '@angular/material';
import { MatTabsModule } from '@angular/material/tabs';

import { environment } from './../environments/environment';

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
import { OwnerNavbarComponent } from './owner/owner-navbar/owner-navbar.component';
import { OwnerInformationComponent } from './owner/owner-information/owner-information.component';
import { OwnerPetsComponent } from './owner/owner-pets/owner-pets.component';
import { OwnerServicesComponent } from './owner/owner-services/owner-services.component';
import { PetCareInformationComponent } from './pet-care/pet-care-information/pet-care-information.component';
import { PetCareNavbarComponent } from './pet-care/pet-care-navbar/pet-care-navbar.component';
import { PetCareHistoryComponent } from './pet-care/pet-care-history/pet-care-history.component';
import { PetCareReportsComponent } from './pet-care/pet-care-reports/pet-care-reports.component';
import { PetCareServicesComponent } from './pet-care/pet-care-services/pet-care-services.component';
import { PetCareScheduleComponent } from './pet-care/pet-care-schedule/pet-care-schedule.component';
import { AdminNavbarComponent } from './admin/admin-navbar/admin-navbar.component';
import { AdminInformationComponent } from './admin/admin-information/admin-information.component';
import { AdminPetCaresComponent } from './admin/admin-pet-cares/admin-pet-cares.component';
import { AdminBusinessReportsComponent } from './admin/admin-business-reports/admin-business-reports.component';
import { AdminUserReportsComponent } from './admin/admin-user-reports/admin-user-reports.component';


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
		TabPetCareComponent,
		OwnerNavbarComponent,
		OwnerInformationComponent,
		OwnerPetsComponent,
		OwnerServicesComponent,
		PetCareInformationComponent,
		PetCareNavbarComponent,
		PetCareHistoryComponent,
		PetCareReportsComponent,
		PetCareServicesComponent,
		PetCareScheduleComponent,
		AdminNavbarComponent,
		AdminInformationComponent,
		AdminPetCaresComponent,
		AdminBusinessReportsComponent,
		AdminUserReportsComponent
	],
	imports: [
		AngularFireModule.initializeApp(environment.firebase),
		AngularFireStorageModule,
		AppRoutingModule,
		BrowserModule,
		BrowserAnimationsModule,
		FileUploadModule,
		FormsModule,
		HttpClientModule,
		MatTableModule,
		MatTabsModule,
		NgxLoadingModule.forRoot({
			animationType: ngxLoadingAnimationTypes.circleSwish,
			backdropBackgroundColour: 'rgba(0,0,0,0.1)', 
			backdropBorderRadius: '4px',
			primaryColour: '#ffffff', 
			secondaryColour: '#ffffff', 
			tertiaryColour: '#ffffff'
		}),
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

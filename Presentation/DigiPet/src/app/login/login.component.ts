import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from  '@angular/forms';
import { NgxLoadingComponent } from 'ngx-loading';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

import { ApiService } from './../services/api/api.service';
import { AuthService } from './../services/auth/auth.service';
import { DataTransferService } from './../services/data-transfer/data-transfer.service';
import { EncryptionService } from './../services/encryption/encryption.service';


@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
	public isSubmitted = false;
	public loading = false;
	public loginForm: FormGroup;

	constructor(private authService: AuthService, private router: Router,
				private formBuilder: FormBuilder, private api: ApiService,
				private encryptionService: EncryptionService,
				private dataTransferService: DataTransferService) {}

	ngOnInit() {
		this.authService.logout();
		this.loginForm = this.formBuilder.group({
			username: ['', Validators.required],
			password: ['', Validators.required]
		});
	}

	get formControls() {
		return this.loginForm.controls;
	}

	public login() {
		this.isSubmitted = true;
		if (this.loginForm.invalid) {
			return;
		}

		this.loading = true;
		let response = this.api.authenticateUser(this.loginForm.value);
		response.subscribe(data => {
			this.authService.login(data);
			if (data.role == 'Admin') {
				this.dataTransferService.setRole('Admin');
				this.router.navigateByUrl('admin/profile');
			} else if (data.role == 'PetOwner') {
				this.dataTransferService.setRole('PetOwner');
				this.router.navigateByUrl('owner/profile');
			} else if (data.role == 'Walker') {
			 	this.dataTransferService.setRole('Walker');
			 	this.router.navigateByUrl('petcare/profile');
			}
			this.loading = false;
		}, error => {
			this.loading = false;
			Swal.fire({
				title: '¡Error de autenticación!',
				text: '¡Usuario o contraseña inválidos!',
				type: 'error',
				confirmButtonText: 'Cool'
			});
		});
	}

}

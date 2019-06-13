import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from  '@angular/forms';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

import { ApiService } from './../services/api/api.service'
import { AuthService } from './../services/auth/auth.service';
import { EncryptionService } from './../services/encryption/encryption.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public loginForm: FormGroup;
  public isSubmitted = false;

  constructor(private authService: AuthService, private router: Router,
  			      private formBuilder: FormBuilder, private api: ApiService,
              private encryptionService: EncryptionService) {}

  ngOnInit() {
    this.authService.logout();
  	this.loginForm = this.formBuilder.group(
  	  {
  	  	username: ['', Validators.required],
  	  	password: ['', Validators.required]
  	  }
  	);
  }

  get formControls() {
  	return this.loginForm.controls;
  }

  public login() {
  	this.isSubmitted = true;
  	if (this.loginForm.invalid) {
  		return;
  	}

    let response = this.api.authenticateUser(this.loginForm.value);
    response.subscribe(data => {
      this.authService.login(data);
      if (data.role == 'Admin') {
        this.authService.setRole('Admin');
        this.router.navigateByUrl('admin/profile');
      } else if (data.role == 'PetOwner') {
        this.authService.setRole('PetOwner');
        this.router.navigateByUrl('owner/profile');
      } else if (data.role == 'Walker') {
        this.authService.setRole('Walker');
        this.router.navigateByUrl('petcare/profile');
      }
    }, error => {
      Swal.fire({
        title: '¡Error de autenticación!',
        text: '¡Usuario o contraseña inválidos!',
        type: 'error',
        confirmButtonText: 'Cool'
      });
    });
  }

}

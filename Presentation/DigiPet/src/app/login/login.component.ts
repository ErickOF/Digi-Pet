import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from  '@angular/forms';
import { Router } from '@angular/router';

import { UserLogin } from '../models/user-login'
import { AuthService } from '../services/auth/auth.service'


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public loginForm: FormGroup;
  public isSubmitted = false;

  constructor(private authService: AuthService, private router: Router,
  			      private formBuilder: FormBuilder) {}

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
    let loginUser = this.loginForm.value;
    let username = loginUser.username;
    let password = loginUser.password;
    // Amarrado
    // Llamar API
    if (username == 'erickof@xtec.com' && password == 'abcd1234') {
  	  this.authService.login(this.loginForm.value);
      this.authService.setRole('Admin');
  	  this.router.navigateByUrl('admin/profile');
    } else if (username == 'erickobregonf@gmail.com' && password == 'abcd1234') {
      this.authService.login(this.loginForm.value);
      this.authService.setRole('Owner');
      this.router.navigateByUrl('owner/profile');
    } else if (username == '2016123157' && password == 'abcd1234') {
      this.authService.login(this.loginForm.value);
      this.authService.setRole('PetCare');
      this.router.navigateByUrl('petcare/profile');
    } else {
      alert('Usuario desconocido');
    }
  }

}

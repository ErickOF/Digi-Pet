import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from  '@angular/forms';
import { Router } from '@angular/router';

import { UserLogin } from '../interfaces/user-login'
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
  	this.loginForm = this.formBuilder.group(
  	  {
  	  	email: ['', Validators.required],
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
  	this.authService.login(this.loginForm.value);
  	this.router.navigateByUrl('owner/profile');
  }

}

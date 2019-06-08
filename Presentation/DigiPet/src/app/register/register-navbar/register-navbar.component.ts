import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';


@Component({
	selector: 'app-register-navbar',
	templateUrl: './register-navbar.component.html',
	styleUrls: ['./register-navbar.component.css']
})
export class RegisterNavbarComponent implements OnInit {

	constructor(private router: Router) {}

	ngOnInit() {
	}

	public login() {
		console.log('Login');
		this.router.navigateByUrl('login');
	}

}

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../../services/auth/auth.service';


@Component({
	selector: 'app-owner-navbar',
	templateUrl: './owner-navbar.component.html',
	styleUrls: ['./owner-navbar.component.css']
})
export class OwnerNavbarComponent implements OnInit {

	constructor(private authService: AuthService, private router: Router) { }

	ngOnInit() {
	}

	logout(){
		this.authService.logout();
		this.router.navigateByUrl('/login');
	}

}

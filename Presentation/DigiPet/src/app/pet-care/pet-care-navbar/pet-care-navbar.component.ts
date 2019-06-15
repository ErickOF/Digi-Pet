import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../../services/auth/auth.service';


@Component({
	selector: 'app-pet-care-navbar',
	templateUrl: './pet-care-navbar.component.html',
	styleUrls: ['./pet-care-navbar.component.css']
})
export class PetCareNavbarComponent implements OnInit {

	constructor(private authService: AuthService, private router: Router) { }

	ngOnInit() {
	}

	logout(){
		this.authService.logout();
		this.router.navigateByUrl('/login');
	}

}

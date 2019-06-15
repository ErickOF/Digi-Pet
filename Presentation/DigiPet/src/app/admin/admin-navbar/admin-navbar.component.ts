import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../../services/auth/auth.service';


@Component({
	selector: 'app-admin-navbar',
	templateUrl: './admin-navbar.component.html',
	styleUrls: ['./admin-navbar.component.css']
})
export class AdminNavbarComponent implements OnInit {

	constructor(private authService: AuthService, private router: Router) { }

	ngOnInit() {
	}

	logout(){
		this.authService.logout();
		this.router.navigateByUrl('/login');
	}

}

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { DataTransferService } from './../services/data-transfer/data-transfer.service';
import { UsersService } from './../services/api/users/users.service';


@Component({
	selector: 'app-admin',
	templateUrl: './admin.component.html',
	styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

	public loading = false;
	public ready = false;

	constructor(private router: Router, private usersService: UsersService,
  				private dataTransferService: DataTransferService) {

		this.loading = true;
		
		let data = this.dataTransferService.getAccessToken();
		delete data.token;
		this.dataTransferService.setUserInformation(data);
		this.loading = false;
		this.ready = true;
	}

	ngOnInit() {
	}

}
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { DataTransferService } from './../services/data-transfer/data-transfer.service';
import { UsersService } from './../services/api/users/users.service';


@Component({
	selector: 'app-pet-care',
	templateUrl: './pet-care.component.html',
	styleUrls: ['./pet-care.component.css']
})
export class PetCareComponent implements OnInit {

	public loading = false;
	public ready = false;

	constructor(private router: Router, private usersService: UsersService,
  				private dataTransferService: DataTransferService) {

		this.loading = true;
		
		let token = this.dataTransferService.getAccessToken().token;
		let response = this.usersService.getPetCare(token);
		
		response.subscribe(data => {
			this.dataTransferService.setUserInformation(data);
			this.loading = false;
			this.ready = true;
		}, error => {
			this.loading = false;
			this.ready= true;
			console.log(error);
		});
	}

	ngOnInit() {
	}

}

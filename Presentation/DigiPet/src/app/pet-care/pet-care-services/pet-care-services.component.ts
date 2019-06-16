import { Component, OnInit } from '@angular/core';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { UsersService } from './../../services/api/users/users.service';


@Component({
	selector: 'app-pet-care-services',
	templateUrl: './pet-care-services.component.html',
	styleUrls: ['./pet-care-services.component.css']
})
export class PetCareServicesComponent implements OnInit {

	public services;

	constructor(private dataTransferService: DataTransferService,
				private usersService: UsersService) {
		let token = this.dataTransferService.getAccessToken().token;
		let response = this.usersService.getUpComingWalks(token);

		response.subscribe(data => {
			this.services = data;
		}, error => {
			console.log(error);
		});
		
	}

	ngOnInit() {
	}

}

import { Component, OnInit } from '@angular/core';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { UsersService } from './../../services/api/users/users.service';


@Component({
	selector: 'app-owner-services',
	templateUrl: './owner-services.component.html',
	styleUrls: ['./owner-services.component.css']
})
export class OwnerServicesComponent implements OnInit {

	public services: any = [];

	constructor(private dataTransferService: DataTransferService,
				private usersService: UsersService) {
		let token = this.dataTransferService.getAccessToken().token;
		let response = this.usersService.getUpComingWalksByOwner(token);

		response.subscribe(data => {
			this.services = data;
		}, error => {
			console.log(error);
		});
		
	}

	ngOnInit() {
	}

}

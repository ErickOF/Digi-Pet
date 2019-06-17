import { Component, OnInit } from '@angular/core';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { UsersService } from './../../services/api/users/users.service';


@Component({
	selector: 'app-pet-care-history',
	templateUrl: './pet-care-history.component.html',
	styleUrls: ['./pet-care-history.component.css']
})
export class PetCareHistoryComponent implements OnInit {

	public history: any = [];

	constructor(private dataTransferService: DataTransferService,
				private usersService: UsersService) {
		let token = this.dataTransferService.getAccessToken().token;
		let response = this.usersService.getUpComingWalksByPetCare(token);

		response.subscribe(data => {
			this.history = data;
		}, error => {
			console.log(error);
		});
		
	}

	ngOnInit() {
	}

}

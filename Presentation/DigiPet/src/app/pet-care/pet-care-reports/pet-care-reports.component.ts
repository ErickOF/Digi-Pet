import { Component, OnInit } from '@angular/core';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { UsersService } from './../../services/api/users/users.service';


@Component({
	selector: 'app-pet-care-reports',
	templateUrl: './pet-care-reports.component.html',
	styleUrls: ['./pet-care-reports.component.css']
})
export class PetCareReportsComponent implements OnInit {

	public reports: any = [];

	constructor(private dataTransferService: DataTransferService,
				private usersService: UsersService) {
		let token = this.dataTransferService.getAccessToken().token;
		let response = this.usersService.getPendingReportCards(token);

		response.subscribe(data => {
			this.reports = data;
			console.log(data);
		}, error => {
			console.log(error);
		});
		
	}

	ngOnInit() {
	}

}
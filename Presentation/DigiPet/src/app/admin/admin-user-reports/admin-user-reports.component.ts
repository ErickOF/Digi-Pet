import { Component, OnInit } from '@angular/core';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { UsersService } from './../../services/api/users/users.service';


@Component({
	selector: 'app-admin-user-reports',
	templateUrl: './admin-user-reports.component.html',
	styleUrls: ['./admin-user-reports.component.css']
})
export class AdminUserReportsComponent implements OnInit {

	constructor(private dataTransferService: DataTransferService,
				private usersService: UsersService) {
		let token = this.dataTransferService.getAccessToken().token;
		let response = this.usersService.getUserReports(token);

		response.subscribe(data => {
			console.log(data)
		}, error => {
			console.log(error);
		});
	}

	ngOnInit() {
	}

}

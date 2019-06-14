import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { DataTransferService } from './../services/data-transfer/data-transfer.service';
import { UsersService } from './../services/api/users/users.service';


@Component({
	selector: 'app-owner',
	templateUrl: './owner.component.html',
	styleUrls: ['./owner.component.css']
})
export class OwnerComponent implements OnInit {

	constructor(private router: Router, private usersService: UsersService,
  				private dataTransferService: DataTransferService) { }

	ngOnInit() {
		let token = this.dataTransferService.getAccessToken().token;
		let response = this.usersService.getOwner(token);
		response.subscribe( data => {
			console.log(data);
		}, error => {
			console.log(error);
		});
	}

}

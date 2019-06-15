import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { UsersService } from './../../services/api/users/users.service';


@Component({
	selector: 'app-admin-pet-cares',
	templateUrl: './admin-pet-cares.component.html',
	styleUrls: ['./admin-pet-cares.component.css']
})
export class AdminPetCaresComponent implements OnInit {

	public image = './assets/default-avatar.png';
	public petCares: any = [];
	public token: string;

	constructor(private router: Router, private usersService: UsersService,
  				private dataTransferService: DataTransferService) {

		this.token = this.dataTransferService.getAccessToken().token;
		this.loadAllPetCares();
	}

	ngOnInit() {
	}

	public activePetCare(petCare) {
		let response = this.usersService.activePetCare(this.token, petCare.id);

		response.subscribe(data => {
			console.log(data);
			this.loadAllPetCares();
		}, error => {
			console.log(error);
		});
	}

	public blockPetCare(petCare) {
		let response = this.usersService.blockPetCare(this.token, petCare.id);

		response.subscribe(data => {
			console.log(data);
			this.loadAllPetCares();
		}, error => {
			console.log(error);
		});
	}

	private loadAllPetCares() {
		let response = this.usersService.getAllPetCares(this.token);
		
		response.subscribe(data => {
			this.petCares = data;
		}, error => {
			console.log(error);
		});
	}

}

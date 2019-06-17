import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { UsersService } from './../../services/api/users/users.service';


@Component({
	selector: 'app-admin-pet-cares',
	templateUrl: './admin-pet-cares.component.html',
	styleUrls: ['./admin-pet-cares.component.css']
})
export class AdminPetCaresComponent implements OnInit {

	public image = './assets/default-avatar.png';
	public loading = false;
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
		this.loading = true;
		let response = this.usersService.activePetCare(this.token, petCare.id);

		response.subscribe(data => {
			if (data.ok) {
				this.loadAllPetCares();
			} else {
				this.loading = false;
				this.showErrorMsg('¡Error de conexión!', '¡Por favor inténtelo más tarde!');
			}
		}, error => {
			this.loading = false;
			this.showErrorMsg('¡Error de conexión!', '¡Por favor inténtelo más tarde!');
		});
	}

	public blockPetCare(petCare) {
		this.loading = true;
		let response = this.usersService.blockPetCare(this.token, petCare.id);

		response.subscribe(data => {
			if (data.ok) {
				this.loadAllPetCares();
			} else {
				this.loading = false;
				this.showErrorMsg('¡Error de conexión!', '¡Por favor inténtelo más tarde!');
			}
		}, error => {
			this.loading = false;
			this.showErrorMsg('¡Error de conexión!', '¡Por favor inténtelo más tarde!');
		});
	}

	private loadAllPetCares() {
		this.loading = true;
		let response = this.usersService.getAllPetCares(this.token);
		
		response.subscribe(data => {
			this.petCares = data;
			this.petCares.sort((a, b) => (a.id > b.id) ? 1 : ((b.id > a.id) ? -1 : 0));
			this.loading = false;
		}, error => {
			this.loading = false;
			this.showErrorMsg('¡Error de conexión!', '¡Por favor inténtelo más tarde!');
		});
	}

	private showErrorMsg(title: string, msg: string) {
		Swal.fire({
			title: title,
			text: msg,
			type: 'error',
			confirmButtonText: 'Ok'
		});
	}

}

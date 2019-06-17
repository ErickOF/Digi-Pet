import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { UsersService } from './../../services/api/users/users.service';


@Component({
	selector: 'app-owner-no-rating',
	templateUrl: './owner-no-rating.component.html',
	styleUrls: ['./owner-no-rating.component.css']
})
export class OwnerNoRatingComponent implements OnInit {

	public services: any = [];

	constructor(private dataTransferService: DataTransferService,
				private router: Router,
				private usersService: UsersService) {
		let token = this.dataTransferService.getAccessToken().token;
		let response = this.usersService.getWalksNoRating(token);
		
		response.subscribe(data => {
			this.services = data;
			console.log(data);
		}, error => {
			console.log(error);
		});
	}

	ngOnInit() {
	}

	public makeReport(idWalker: number) {
		let rate = {
			"walkId": idWalker,
			"stars": 0,
			"Denuncia": {
				"Description": "golpeo al perro"
			}
		};

		Swal.fire({
			title: 'Indique su queja.',
			input: 'textarea',
			inputAttributes: {
				autocapitalize: 'off'
			},
			showCancelButton: true,
			confirmButtonClass: 'btn btn-success',
			cancelButtonClass: 'btn btn-danger',
			buttonsStyling: false,
			allowOutsideClick: () => !Swal.isLoading()
		}).then((result) => {
			if (result.value) {
				rate.Denuncia.Description = result.value;
				this.sendRating(rate);
			} else if (result.dismiss) {
				Swal.fire({
					title: '¡Cancelado!',
					text: 'La operación fue cancelada.',
					type: 'error',
					confirmButtonClass: "btn btn-info",
					buttonsStyling: false
				})
			}
		})
	}

	private sendRating(rate) {
		let token = this.dataTransferService.getAccessToken().token;
		let response = this.usersService.setRating(token, rate);
		
		response.subscribe(data => {
			Swal.fire({
				title: '¡Éxito!',
				type: 'success',
				confirmButtonClass: "btn btn-info",
				buttonsStyling: false
			})
			this.router.navigateByUrl('/RefrshComponent', {skipLocationChange: true})
						.then(()=>this.router.navigate(['owner/profile']));
		}, error => {
			console.log(error);
			Swal.fire({
				title: 'Error!',
				text: 'No pudo realizarse la petición.',
				type: 'error',
				confirmButtonClass: "btn btn-info",
				buttonsStyling: false
			})
		});
	}

	public setRating(idWalker: number) {
		let rate = {
			"walkId": idWalker,
			"stars": 5,
			"Denuncia": null
		};
		console.log(rate);

		Swal.fire({
			title: 'Por favor califique el servicio: ',
			input: 'select',
			inputOptions: {
				0: '0',
				1: '1',
				2: '2',
				3: '3',
				4: '4',
				5: '5'
			},
			inputPlaceholder: 'Calificar servicio',
			showCancelButton: true,
			inputValidator: (value) => {
				return new Promise((resolve) => {
					if (value in [0,1,2,3,4,5]) {
						resolve()
					} else {
						resolve('¡Por favor seleccionar la calificación!')
					}
				})
			}
		}).then((result) => {
			if (result.value) {
				rate.stars = result.value;
				this.sendRating(rate);
			}
		})
	}

}

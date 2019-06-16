import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { UsersService } from './../../services/api/users/users.service';


@Component({
	selector: 'app-pet-care-schedule',
	templateUrl: './pet-care-schedule.component.html',
	styleUrls: ['./pet-care-schedule.component.css']
})
export class PetCareScheduleComponent implements OnInit {
	public date: Date;
	public dataSource = [];

	public hours = ['hour', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday', 'sunday'];

	public loading = false;

	public petCareSchedule = [];

	private schedule = {
		'Week': []
	}

	constructor(private usersService: UsersService,
  				private dataTransferService: DataTransferService) {
		this.date = new Date();
		this.hours = this.startAndEndOfWeek(this.date.toString());
		this.dataSource = this.generateSchedule();
		console.log(this.dataTransferService.getAccessToken().token);
	}

	ngOnInit() {
	}

	public checkboxClick(event, day, hour) {
		if (event.target.checked) {
			let valid = false;
			for (var i = 0; i < this.schedule.Week.length; i++) {
				if (this.schedule.Week[i]["Date"] == day) {
					this.schedule.Week[i]["HoursAvailable"].push(hour.split(':')[0]);
					valid = true;
				}
			}

			if (!valid) {
				this.schedule.Week.push({
					"Date": day,
					"HoursAvailable": [hour.split(':')[0]]
				});
			}
		} else {
			for (var i = 0; i < this.schedule.Week.length; i++) {
				if (this.schedule.Week[i]["Date"] == day) {
					this.schedule.Week[i]["HoursAvailable"].splice(this.schedule.Week[i]["HoursAvailable"].indexOf(hour.split(':')[0]), 1);
					if (this.schedule.Week[i] == []) {
						this.schedule.Week[i]["HoursAvailable"].splice(i, 1);
						break;
					}
				}
			}
		}
	}

	private generateSchedule() {
		let schedule = [];
		for (var i = 0; i < 24; i++) {
			schedule.push(i.toString() + ':00');
		}
		return schedule;
	}

	public save() {
		console.log(this.schedule);
		this.loading = true;
		let token = this.dataTransferService.getAccessToken().token;
		let response = this.usersService.setSchedule(token, this.schedule);
		
		response.subscribe(data => {
			this.loading = false;
			if (data.status == 200) {
				this.showSuccessMsg('¡Horario actualizado!', 'El horario fue actualizado con éxito');
			} else {
				this.showErrorMsg('¡Error de configuración!', 'Se debe actualizar al menos 24h antes');
			}
		}, error => {
			this.loading = false;
			if (error.status == 200) {
				this.showSuccessMsg('¡Horario actualizado!', 'El horario fue actualizado con éxito');
			} else {
				this.showErrorMsg('¡Error de conexión!', 'No se pudo actualizar el horario. Por favor inténtelo más tarde');
			}
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

	private showSuccessMsg(title: string, msg: string) {
		Swal.fire({
			title: title,
			text: msg,
			type: 'success',
			confirmButtonText: 'Ok'
		});
	}

	private startAndEndOfWeek(date) {
		var now = date ? new Date(date) : new Date();
		now.setHours(0, 0, 0, 0);
		return Array(8).fill('').map((_, i) => {
			if (i == 0) {
				return 'Horario';
			}

			var monday = new Date(now);
			monday.setDate(monday.getDate() + i);
			const day = monday.getDate();
			const month = monday.getMonth() + 1;
			const year = monday.getFullYear();

			return year + '-' + (month < 10? '0' + month : month) + '-' + day;
		});
	}

}

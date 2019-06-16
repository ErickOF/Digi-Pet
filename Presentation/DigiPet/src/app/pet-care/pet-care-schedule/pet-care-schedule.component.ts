import { Component, OnInit } from '@angular/core';


@Component({
	selector: 'app-pet-care-schedule',
	templateUrl: './pet-care-schedule.component.html',
	styleUrls: ['./pet-care-schedule.component.css']
})
export class PetCareScheduleComponent implements OnInit {
	public date: Date;
	public dataSource = [];

	public hours = ['hour', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday', 'sunday'];

	public petCareSchedule = [];

	private schedule = {
		'Week': []
	}

	constructor() {
		this.date = new Date();
		this.hours = this.startAndEndOfWeek(this.date.toString());
		this.dataSource = this.generateSchedule();
	}

	ngOnInit() {
	}

	public checkboxClick(event, day, hour) {
		console.log(event.target.checked, day, hour);
		if (event.target.checked) {
			let valid = false;
			for (var i = 0; i < this.schedule.Week.length; i++) {
				if (this.schedule.Week[i]['Date'] == day) {
					this.schedule.Week[i]['HoursAvailable'].push(parseInt(hour.split(':')));
					valid = true;
				}
			}

			if (!valid) {
				this.schedule.Week.push({
					'Date': day,
					'HoursAvailable': [parseInt(hour.split(':'))]
				});
			}
		} else {
			for (var i = 0; i < this.schedule.Week.length; i++) {
				if (this.schedule.Week[i]['Date'] == day) {
					delete this.schedule.Week[i]['HoursAvailable'][this.schedule.Week[i]['HoursAvailable'].indexOf(parseInt(hour.split(':')))];
				}
			}
		}
		console.log(this.schedule)
	}

	public generateSchedule() {
		let schedule = [];
		for (var i = 0; i < 24; i++) {
			schedule.push(i.toString() + ':00');
		}
		return schedule;
	}

	public startAndEndOfWeek(date) {
		console.log(date);
		var now = date ? new Date(date) : new Date();
		now.setHours(0, 0, 0, 0);
		return Array(8).fill('').map((_, i) => {
			if (i == 0) {
				return 'Horario';
			}

			var monday = new Date(now);
			monday.setDate(monday.getDate() + i - 1);
			const day = monday.getDate();
			const month = monday.getMonth() + 1;
			const year = monday.getFullYear();

			return year + '-' + month + '-' + day;
		});
	}

}

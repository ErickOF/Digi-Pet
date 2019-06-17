import { Component, OnInit } from '@angular/core';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { UsersService } from './../../services/api/users/users.service';


window.onclick = function(event) {
	let reportCardModal = document.getElementById("ReportCardViewModal");
	
	if (event.target == reportCardModal) {
		reportCardModal.style.display = "none";
	}
}

@Component({
	selector: 'app-pet-care-history',
	templateUrl: './pet-care-history.component.html',
	styleUrls: ['./pet-care-history.component.css']
})
export class PetCareHistoryComponent implements OnInit {

	public history: any = [];
	public idReport = 0;
	public report = {};

	constructor(private dataTransferService: DataTransferService,
				private usersService: UsersService) {
		let token = this.dataTransferService.getAccessToken().token;
		let response = this.usersService.getWalksHistoryByPetCare(token);

		response.subscribe(data => {
			this.history = data;
		}, error => {
			console.log(error);
		});
	}

	ngOnInit() {
	}

	public hideReportCardViewModal() {
		document.getElementById('ReportCardViewModal').style.display='none';
	}

	public showReportCard(report, id) {
		this.report = report;
		this.idReport = id;
		this.showReportCardViewModal();
	}

	public showReportCardViewModal() {
		document.getElementById('ReportCardViewModal').style.display='block';
	}

}

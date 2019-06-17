import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxLoadingComponent } from 'ngx-loading';
import { Observable } from "rxjs";
import { Router } from '@angular/router';
import Swal from 'sweetalert2';


import { ApiService } from './../../services/api/api.service';
import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { ImgUploadService } from './../../services/uploads/img-upload/img-upload.service';
import { UsersService } from './../../services/api/users/users.service';


window.onclick = function(event) {
	let reportCardModal = document.getElementById("ReportCardModal");
	
	if (event.target == reportCardModal) {
		reportCardModal.style.display = "none";
	}
}

@Component({
	selector: 'app-pet-care-reports',
	templateUrl: './pet-care-reports.component.html',
	styleUrls: ['./pet-care-reports.component.css']
})
export class PetCareReportsComponent implements OnInit {
	public idPendingReportCard = 0;
	public registerPendingReportCard: FormGroup;
	public reports: any = [];

	constructor(private api: ApiService,
				private dataTransferService: DataTransferService,
				private formBuilder: FormBuilder,
				private imgUploadService: ImgUploadService,
				private router: Router,
				private usersService: UsersService) {

		let token = this.dataTransferService.getAccessToken().token;
		let response = this.usersService.getPendingReportCards(token);

		response.subscribe(data => {
			this.reports = data;
		}, error => {
			console.log(error);
		});

		this.registerPendingReportCard = this.formBuilder.group({
			distance: ['', Validators.compose([
				Validators.required,
				Validators.pattern('^[0-9]*$')
			])],
			comments: ['', Validators.maxLength(300)]
		});
		
	}

	ngOnInit() {
	}

	get pendingReportCardFormControls() {
		return this.registerPendingReportCard.controls;
	}

	public fillPendingReportCard(reportCard) {
		this.idPendingReportCard = reportCard.id;
		this.showReportCardModal();
	}

	public hideReportCardModal() {
		document.getElementById('ReportCardModal').style.display='none';
	}

	public showReportCardModal() {
		document.getElementById('ReportCardModal').style.display='block';
	}
}
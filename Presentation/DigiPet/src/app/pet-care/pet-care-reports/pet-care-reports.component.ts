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
	public downloadURL: Observable<string>;
	public idPendingReportCard = 0;
	public isSubmitted = false;
	public loading = false;
	public loadingImgsPet = [false, false, false, false, false];
	public needs: any = [];
	public registerPendingReportCard: FormGroup;
	public registerNeeds: FormGroup;
	public reports: any = [];
	public uploadPercent: Observable<number>;
	public urlsPet = ['', '', '', '', ''];

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
			comments: ['', Validators.required]
		});

		this.needs.push(this.formBuilder.group({
			need: ['', Validators.required]
		}));

		this.registerNeeds = this.formBuilder.group({
			details: this.formBuilder.array(this.needs)
		});
		
	}

	ngOnInit() {
	}

	get pendingReportCardFormControls() {
		return this.registerPendingReportCard.controls;
	}

	get registerNeedsControls() {
		return (this.registerNeeds.get('details') as FormArray).controls;
	}

	public addNeed() {
		const details = this.registerNeeds.get('details') as FormArray;
		details.push(this.formBuilder.group({
			need: ['', Validators.required]
		}));
	}

	public deleteImage(i) {
		this.urlsPet[i] = '';
	}

	public deleteNeed(i: number) {
		const details = this.registerNeeds.get('details') as FormArray;
		if (details.controls.length > 1) {
			details.removeAt(i);
		}
	}

	public fillPendingReportCard(reportCard) {
		this.idPendingReportCard = reportCard.id;
		this.showReportCardModal();
	}

	public getNeedFormControls(index: number) {
		return ((this.registerNeeds.get('details') as FormArray).controls[index] as FormGroup).controls;
	}

	private getNeedsInfo(): string[] {
		let needs = [];
		let needsInfo = this.registerNeeds.value.details;

		for (var i = 0; i < needsInfo.length; i++) {
			needs.push(needsInfo[i].need);
		}

		return needs;
	}

	public hideReportCardModal() {
		document.getElementById('ReportCardModal').style.display='none';
	}

	public openFileDialog(i: number) {
		document.getElementById('selectFile' + i.toString()).click();
	}

	public sendReportCard() {
		this.isSubmitted = true;

		let reportCardInfo = this.registerPendingReportCard.value;
		let needsInfo = this.registerNeeds.value;
		
		if (!this.registerPendingReportCard.valid || !this.registerNeeds.valid) {
			return;
		}

		if (!this.validateUrlsPet()) {
			this.showErrorMsg('¡Mascota sin foto!', 'Debe seleccionar al menos una foto por mascota');
			return;
		}

		let reportCard = {
			"WalkId": this.idPendingReportCard,
			"Comments": reportCardInfo.comments,
			"Photos": this.urlsPet,
			"Distance": reportCardInfo.distance,
			"Necesidades": this.getNeedsInfo()
		}

		let token = this.dataTransferService.getAccessToken().token;

		let response = this.usersService.sendReportCard(token, reportCard);
		response.subscribe(data => {
			this.loading = false;
			this.hideReportCardModal();
			this.router.navigateByUrl('/RefrshComponent', {skipLocationChange: true})
						.then(()=>this.router.navigate(['petcare/profile']));
		}, error => {
			console.log(error);
			this.loading = false;
			this.showErrorMsg('¡Error!', '¡La tarjeta de reporte no pudo enviarse. Por favor intente más tarde!');
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

	public showReportCardModal() {
		document.getElementById('ReportCardModal').style.display='block';
	}

	private showSuccessMsg(title: string, msg: string) {
		Swal.fire({
			title: title,
			text: msg,
			type: 'success',
			confirmButtonText: 'Ok'
		});
	}

	public uploadPetImg(event, i) {
		this.loadingImgsPet[i] = true;
		
		let file = event.target.files[0];

		if (!file) {
			this.loadingImgsPet[i] = true;
			return;
		}
		
		let filePath = file.name.split('.')[0];

		this.uploadPercent = this.imgUploadService.uploadFile(filePath, file);
		this.uploadPercent.subscribe(data => {
			if (data == 100) {
				this.downloadURL = this.imgUploadService.getImage(filePath);
				this.downloadURL.subscribe(url => {
					this.urlsPet[i] = url;
					this.loadingImgsPet[i] = false;
				}, error => {
					if (data == 100) {
						this.downloadURL = this.imgUploadService.getImage(filePath);
						this.downloadURL.subscribe(url => {
							this.urlsPet[i] = url;
							this.loadingImgsPet[i] = false;
						}, error => {
							this.loadingImgsPet[i] = false;
						});
					}
				});
			}
		}, error => {
			this.loadingImgsPet[i] = false;
		});
	}

	private validateUrlsPet() {
		let valid = false;
		for (let i in this.urlsPet) {
			if (this.urlsPet[i] != '') {
				valid = true;
			}
		}
		return valid;
	}

}
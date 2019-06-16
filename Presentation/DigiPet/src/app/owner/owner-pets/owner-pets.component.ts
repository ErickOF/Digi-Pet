import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxLoadingComponent } from 'ngx-loading';
import { Observable } from "rxjs";
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

import { ApiService } from './../../services/api/api.service';
import { AuthService } from './../../services/auth/auth.service';
import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { ImgUploadService } from './../../services/uploads/img-upload/img-upload.service';


window.onclick = function(event) {
	let addPetModal = document.getElementById("AddPetModal");
	let walkServiceModal = document.getElementById("WalkServiceModal");
	
	if (event.target == addPetModal) {
		addPetModal.style.display = "none";
	}

	if (event.target == walkServiceModal) {
		walkServiceModal.style.display = "none";
	}
}

@Component({
	selector: 'app-owner-pets',
	templateUrl: './owner-pets.component.html',
	styleUrls: ['./owner-pets.component.css']
})
export class OwnerPetsComponent implements OnInit {
	public downloadURL: Observable<string>;
	public isSubmitted = false;
	public loading = false;
	public loadingImgsPet = [false, false, false, false, false];
	public pets;
	public registerPet: FormGroup;
	public registerWalkService: FormGroup;
	public uploadPercent: Observable<number>;
	public urlsPet = ['', '', '', '', ''];

	constructor(private api: ApiService,
				private authService: AuthService,
				private dataTransferService: DataTransferService,
				private formBuilder: FormBuilder,
				private imgUploadService: ImgUploadService,
				private router: Router) {

		this.pets = this.dataTransferService.getUserInformation().pets;

		for (let i = 0; i < this.pets.length; i++) {
			this.pets[i].dateCreated = this.pets[i].dateCreated.split('T')[0];
		}

		this.registerPet = this.formBuilder.group({
			name: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(30)
			])],
			breed: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(30)
			])],
			age: ['', Validators.compose([
				Validators.required,
				Validators.pattern('^[0-9]*$')
			])],
			size: 'S',
			photos: '',
			description: ['', Validators.maxLength(300)]
		});
	}

	ngOnInit() {
	}

	get petFormControls() {
		return this.registerPet.controls;
	}

	public addPet() {
		this.isSubmitted = true;
		
		if (!this.registerPet.valid) {
			return;
		}

		if (!this.validateUrlsPet()) {
			this.showErrorMsg('¡Mascota sin foto!', 'Debe seleccionar al menos una foto por mascota');
			return;
		}

		this.loading = true;
		
		let petInfo = this.registerPet.value;
		let pet = {
			"Name": petInfo.name,
			"Race": petInfo.breed,
			"Age": petInfo.age,
			"Size": petInfo.size,
			"Description": petInfo.description,
			"Photos": this.urlsPet
		};

		let token = this.dataTransferService.getAccessToken().token;

		let response = this.api.registerPet(pet, token);
		response.subscribe(newPet => {
			this.loading = false;
			this.hideAddPetModal();
			this.router.navigateByUrl('/RefrshComponent', {skipLocationChange: true})
						.then(()=>this.router.navigate(['owner/profile']));
		}, error => {
			this.loading = false;
			this.showErrorMsg('¡Error!', '¡La mascota no pudo registrarse. Por favor intente más tarde!');
		});
	}

	public deleteImage(i) {
		this.urlsPet[i] = '';
	}

	public hideAddPetModal() {
		document.getElementById('AddPetModal').style.display='none';
	}

	public hideWalkServiceModal() {
		document.getElementById('WalkServiceModal').style.display='none';
	}

	public openFileDialog(i: number) {
		document.getElementById('selectFile' + i.toString()).click();
	}

	public requestWalkService(pet) {
		console.log(pet);
		
		this.registerWalkService = this.formBuilder.group({
			name: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(30)
			])],
			breed: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(30)
			])],
			age: ['', Validators.compose([
				Validators.required,
				Validators.pattern('^[0-9]*$')
			])],
			size: 'S',
			photos: '',
			description: ['', Validators.maxLength(300)]
		});

		this.showWalkServiceModal();
	}

	public showAddPetModal() {
		document.getElementById('AddPetModal').style.display='block';
	}

	public showWalkServiceModal() {
		document.getElementById('WalkServiceModal').style.display='block';
	}

	private showErrorMsg(title: string, msg: string) {
		Swal.fire({
			title: title,
			text: msg,
			type: 'error',
			confirmButtonText: 'Cool'
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

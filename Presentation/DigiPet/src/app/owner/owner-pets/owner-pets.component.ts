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
import { UsersService } from './../../services/api/users/users.service';


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
	public isSubmittedWS = false;
	public loading = false;
	public loadingImgsPet = [false, false, false, false, false];
	public loadingService = false;
	public pets;
	public registerPet: FormGroup;
	public registerWalkService: FormGroup;
	public uploadPercent: Observable<number>;
	public urlsPet = ['', '', '', '', ''];

	public duration = [
		{ id: 0.5, name: "0.5h" },
		{ id: 1, name: "1h" },
		{ id: 1.5, name: "1.5h" },
		{ id: 2, name: "2h" },
		{ id: 2.5, name: "2.5h" },
		{ id: 3, name: "3h" },
		{ id: 3.5, name: "3.5h" },
		{ id: 4, name: "4h" },
		{ id: 4.5, name: "4.5h" },
		{ id: 5, name: "5h" },
		{ id: 5.5, name: "5.5h" },
		{ id: 6, name: "6h" },
		{ id: 6.5, name: "6.5h" },
		{ id: 7, name: "7h" },
		{ id: 7.5, name: "7.5h" },
		{ id: 8, name: "8h" }

	];

	public provinces = [
		{ id: 'San Jose', name: 'San Jose' },
		{ id: 'Alajuela', name: 'Alajuela' },
		{ id: 'Cartago', name: 'Cartago' },
		{ id: 'Heredia', name: 'Heredia' },
		{ id: 'Guanacaste', name: 'Guanacaste' },
		{ id: 'Puntarenas', name: 'Puntarenas' },
		{ id: 'Limón', name: 'Limón' }
	];

	constructor(private api: ApiService,
				private authService: AuthService,
				private dataTransferService: DataTransferService,
				private formBuilder: FormBuilder,
				private imgUploadService: ImgUploadService,
				private router: Router,
				private usersService: UsersService) {

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

		this.registerWalkService = this.formBuilder.group({
			date: ['', Validators.required],
			duration: ['', Validators.required],
			province: ['', Validators.required],
			canton: ['', Validators.required],
			description: ['', Validators.required],
			exactAddress: ['', Validators.required]
		});
	}

	ngOnInit() {
	}

	get petFormControls() {
		return this.registerPet.controls;
	}

	get walkServiceFormControls() {
		return this.registerWalkService.controls;
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

	public confirmWalkService() {
		this.isSubmittedWS = true;
		
		if (!this.registerWalkService.valid) {
			return;
		}

		this.loadingService = true;

		let walkServiceInfo = this.registerWalkService.value;

		let walkService = { 
			"PetId" : walkServiceInfo.idPet,
			"Begin" : walkServiceInfo.date.split('T').join(' '),
			"Duration" : walkServiceInfo.duration,
			"Province" : walkServiceInfo.province,
			"Canton" : walkServiceInfo.canton,
			"Description" : walkServiceInfo.description,
			"ExactAddress": walkServiceInfo.exactAddress
		};
		
		let token = this.dataTransferService.getAccessToken().token;

		let response = this.usersService.requestWalkService(token, walkService);
		response.subscribe(data => {
			this.loadingService = false;
			this.showSuccessMsg('¡Éxito!', 'Su caminata fue agendada')
			this.hideAddPetModal();
			this.router.navigateByUrl('/RefrshComponent', {skipLocationChange: true})
						.then(()=>this.router.navigate(['owner/profile']));
		}, error => {
			this.loadingService = false;
			if (error.error == 'cant find walker') {
				this.showErrorMsg('¡La solicitud no pudo realizarse!', '¡No hay cuidadores para su configuración!');
			}
			this.showErrorMsg('¡Error!', '¡La solicitud no pudo realizarse. Por favor intente más tarde!');
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

	public requestWalkService(idPet) {
		this.registerWalkService = this.formBuilder.group({
			idPet: idPet,
			date: ['', Validators.required],
			duration: ['', Validators.required],
			province: ['', Validators.required],
			canton: ['', Validators.required],
			description: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(300)
			])],
			exactAddress: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(300)
			])]
		});

		this.showWalkServiceModal();
	}

	public showAddPetModal() {
		document.getElementById('AddPetModal').style.display='block';
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

		this.urlsPet = ['', '', '', '', ''];
	}

	public showWalkServiceModal() {
		document.getElementById('WalkServiceModal').style.display='block';
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

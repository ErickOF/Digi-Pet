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


@Component({
	selector: 'app-tab-owner',
	templateUrl: './tab-owner.component.html',
	styleUrls: ['./tab-owner.component.css']
})
export class TabOwnerComponent implements OnInit {
	public downloadURL: Observable<string>;
	public isSubmitted = false;
	public loading = false;
	public loadingImg = false;
	public loadingImgsPets = [];
	public pets = [];
	public registerOwner: FormGroup;
	public registerPets: FormGroup;
	public uploadPercent: Observable<number>;
	public url = '';
	public urlsPets = [];

	public provinces = [
		{ id: 'San Jose', name: 'San Jose' },
		{ id: 'Alajuela', name: 'Alajuela' },
		{ id: 'Cartago', name: 'Cartago' },
		{ id: 'Heredia', name: 'Heredia' },
		{ id: 'Guanacaste', name: 'Guanacaste' },
		{ id: 'Puntarenas', name: 'Puntarenas' },
		{ id: 'Limón', name: 'Limón' }
	];

	public universities = [
		{ id: 'TEC', name: 'TEC' },
		{ id: 'UCR', name: 'UCR' },
		{ id: 'UNA', name: 'UNA' }
	];

	constructor(private router: Router, private formBuilder: FormBuilder,
				private api: ApiService, private authService: AuthService,
				private dataTransferService: DataTransferService,
				private imgUploadService: ImgUploadService) {
	}

	ngOnInit() {
		this.registerOwner = this.formBuilder.group({
			name: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(30)
			])],
			lastname: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(30)
			])],
			province: ['', Validators.required],
			canton: ['', Validators.required],
			email1: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(30),
				Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
			])],
			email2: ['', Validators.compose([
				Validators.maxLength(30),
				Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
			])],
			telephoneNumber: '',
			password: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(8),
				Validators.minLength(8),
				Validators.pattern('^(?=.*[a-zA-Z])(?=.*[0-9]).+$')
			])],
			photo: '',
			description: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(300)
			])]
		});

		this.pets.push(this.createPet());
		this.loadingImgsPets.push([false, false, false, false, false]);
		this.urlsPets.push(['', '', '', '', '']);

		this.registerPets = this.formBuilder.group({
			details: this.formBuilder.array(this.pets)
		});
	}

	get ownerFormControls() {
		return this.registerOwner.controls;
	}

	get registerPetsControls() {
		return (this.registerPets.get('details') as FormArray).controls;
	}

	public addPet() {
		const details = this.registerPets.get('details') as FormArray;
		details.push(this.createPet());
		this.loadingImgsPets.push([false, false, false, false, false]);
		this.urlsPets.push(['', '', '', '', '']);
	}

	private createPet(): FormGroup {
		return this.formBuilder.group({
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

	public deleteImage(pet, i) {
		this.urlsPets[pet][i] = '';
	}

	public getPetFormControls(index: number) {
		return ((this.registerPets.get('details') as FormArray).controls[index] as FormGroup).controls;
	}

	private getPets(): any {
		let pets = [];
		let petsInfo = this.registerPets.value.details;
		for (let i = 0; i < petsInfo.length; i++) {
			let petInfo = petsInfo[i];
			pets.push({
				"Name": petInfo.name,
				"Race": petInfo.breed,
				"Age": petInfo.age,
				"Size": petInfo.size,
				"Description": petInfo.description,
				"Photos": this.urlsPets[i]
			});
		}
		return pets;
	}

	public openFileDialog(i: number) {
		document.getElementById('selectFile' + i.toString()).click();
	}

	public register() {
		this.isSubmitted = true;
		
		if (!this.registerOwner.valid || !this.registerPets.valid) {
			return;
		}

		if (this.url == '') {
			this.showErrorMsg('¡Sin foto de perfil!', 'Debe seleccionar su foto de perfil');
			return;
		}

		if (!this.validateUrlsPets()) {
			this.showErrorMsg('¡Mascota sin foto!', 'Debe seleccionar al menos una foto por mascota');
			return;
		}

		this.loading = true;
		
		let ownerInfo = this.registerOwner.value;
		let owner = {
			"Password": ownerInfo.password,
			"Name": ownerInfo.name,
			"LastName": ownerInfo.lastname,
			"Email": ownerInfo.email1,
			"Email2": ownerInfo.email2,
			"Mobile": ownerInfo.telephoneNumber,
			"Province": ownerInfo.province,
			"Canton": ownerInfo.canton,
			"Description": ownerInfo.description,
			"Pets": this.getPets(),
			"Photo": this.url
		};

		let response = this.api.registerOwner(owner);
		response.subscribe(newOwner => {
			let responseAuth = this.api.authenticateUser({
				username: ownerInfo.email1,
				password: ownerInfo.password
			});
			responseAuth.subscribe(data => {
				this.loading = false;
				this.authService.login(data);
				this.dataTransferService.setRole('PetOwner');
				this.router.navigateByUrl('owner/profile');
			}, error => {
				this.loading = false;
				this.showErrorMsg('¡Error de conexión!','¡Por favor intente más tarde!');
			});
		}, error => {
			this.loading = false;
			this.showErrorMsg('¡Error!', '¡El usuario no pudo registrarse. Por favor intente más tarde!');
		});
	}

	private showErrorMsg(title: string, msg: string) {
		Swal.fire({
			title: title,
			text: msg,
			type: 'error',
			confirmButtonText: 'Cool'
		});
	}

	public uploadFile(event) {
		this.loadingImg = true;
		
		let file = event.target.files[0];
		delete event.target.files;

		if (!file) {
			this.loadingImg = true;
			return;
		}
		
		let filePath = file.name.split('.')[0];
		this.uploadPercent = this.imgUploadService.uploadFile(filePath, file);
		this.uploadPercent.subscribe(data => {
			if (data == 100) {
				this.downloadURL = this.imgUploadService.getImage(filePath);
				this.downloadURL.subscribe(url => {
					this.url = url;
					this.loadingImg = false;
				}, error => {
					if (data == 100) {
						this.downloadURL = this.imgUploadService.getImage(filePath);
						this.downloadURL.subscribe(url => {
							this.url = url;
							this.loadingImg = false;
						}, error => {
							this.loadingImg = false;
						});
					}
				});
			}
		}, error => {
			this.loadingImg = false;
		});
	}

	public uploadPetImg(event, pet, i) {
		this.loadingImgsPets[pet][i] = true;
		
		let file = event.target.files[0];
		delete event.target.files;

		if (!file) {
			this.loadingImgsPets[pet][i] = true;
			return;
		}
		
		let filePath = file.name.split('.')[0];
		this.uploadPercent = this.imgUploadService.uploadFile(filePath, file);
		this.uploadPercent.subscribe(data => {
			if (data == 100) {
				this.downloadURL = this.imgUploadService.getImage(filePath);
				this.downloadURL.subscribe(url => {
					this.urlsPets[pet][i] = url;
					this.loadingImgsPets[pet][i] = false;
				}, error => {
					if (data == 100) {
						this.downloadURL = this.imgUploadService.getImage(filePath);
						this.downloadURL.subscribe(url => {
							this.urlsPets[pet][i] = url;
							this.loadingImgsPets[pet][i] = false;
						}, error => {
							this.loadingImgsPets[pet][i] = false;
						});
					}
				});
			}
		}, error => {
			this.loadingImgsPets[pet][i] = false;
		});
	}

	private validateUrlsPets() {
		for (let i in this.urlsPets) {
			let valid  = false;
			for (let j in this.urlsPets[i]) {
				if (this.urlsPets[i][j] != '') {
					valid = true;
					break;
				}
			}

			if (!valid) {
				return false;
			}
		}
		return true;
	}

}

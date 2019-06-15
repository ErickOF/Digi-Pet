import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from  '@angular/forms';
import { NgxLoadingComponent } from 'ngx-loading';
import { Observable } from "rxjs";
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

import { ApiService } from './../../services/api/api.service';
import { AuthService } from './../../services/auth/auth.service';
import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { ImgUploadService } from './../../services/uploads/img-upload/img-upload.service';


@Component({
	selector: 'app-tab-pet-care',
	templateUrl: './tab-pet-care.component.html',
	styleUrls: ['./tab-pet-care.component.css']
})
export class TabPetCareComponent implements OnInit {
	public downloadURL: Observable<string>;
	public isSubmitted = false;
	public loading = false;
	public loadingImg = false;
	public registerPetCare: FormGroup;
	public uploadPercent: Observable<number>;
	public url = '';

	public provinces = [
		{ id: 0, name: 'San Jose' },
		{ id: 1, name: 'Alajuela' },
		{ id: 2, name: 'Cartago' },
		{ id: 3, name: 'Heredia' },
		{ id: 4, name: 'Guanacaste' },
		{ id: 5, name: 'Puntarenas' },
		{ id: 6, name: 'Limón' }
	];

	public universities = [
		{ id: 0, name: 'TEC'},
		{ id: 1, name: 'UCR'},
		{ id: 2, name: 'UNA'}
	];

	constructor(private router: Router, private formBuilder: FormBuilder,
				private api: ApiService, private authService: AuthService,
				private dataTransferService: DataTransferService,
				private imgUploadService: ImgUploadService) {
	}

	ngOnInit() {
		this.registerPetCare = this.formBuilder.group({
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
			university: ['', Validators.required],
			carnet: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(10)
			])],
			email1: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(30),
				Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
			])],
			email2: ['', Validators.compose([
				Validators.maxLength(30),
				Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
			])],
			telephoneNumber: ['', Validators.compose([
				Validators.required,
				Validators.maxLength(10),
				Validators.minLength(8),
				Validators.pattern('^[0-9]*$')
			])],
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
			])],
			doesOtherProvinces: 'false',
			provinces: new FormArray([])
		});

		this.addCheckboxes();
	}

	get petCareFormControls() {
		return this.registerPetCare.controls;
	}

	private addCheckboxes() {
		this.provinces.map((id, name) => {
			const control = new FormControl(false);
			(this.registerPetCare.controls.provinces as FormArray).push(control);
		});
	}

	private getOtherProvinces(boolProvinces): any {
		let otherProvinces: any = [];
		for (let i = 0; i < boolProvinces.length; i++) {
			if (boolProvinces[i]) {
				otherProvinces.push(this.provinces[i].name);
			}
		}
		return otherProvinces;
	}

	public register() {
		this.isSubmitted = true;

		if (!this.registerPetCare.valid) {
			return;
		}

		if (this.url == '') {
			this.showErrorMsg('¡Sin foto de perfil!', 'Debe seleccionar su foto de perfil');
			return;
		}

		this.loading = true;

		let petCareInfo = this.registerPetCare.value;
		let petCare = {
			"SchoolId": petCareInfo.carnet,
			"Password": petCareInfo.password,
			"Name": petCareInfo.name,
			"LastName": petCareInfo.lastname,
			"Email": petCareInfo.email1,
			"Email2": petCareInfo.email2,
			"Mobile": petCareInfo.telephoneNumber,
			"University": petCareInfo.university,
			"Province": petCareInfo.province,
			"Canton": petCareInfo.canton,
			"DoesOtherProvinces": petCareInfo.doesOtherProvinces,
			"OtherProvinces": this.getOtherProvinces(petCareInfo.provinces),
			"Description": petCareInfo.description
		};

		let response = this.api.registerPetCare(petCare);
		response.subscribe(newPetCare => {
			let responseAuth = this.api.authenticateUser({
				username: petCare.SchoolId,
				password: petCare.Password
			});

			responseAuth.subscribe(data => {
				this.loading = false;
				this.authService.login(data);
				this.dataTransferService.setRole('Walker');
				this.router.navigateByUrl('petcare/profile');
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

}

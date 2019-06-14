import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from  '@angular/forms';
import { NgxLoadingComponent, ngxLoadingAnimationTypes } from 'ngx-loading';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

import { ApiService } from './../../services/api/api.service';
import { AuthService } from './../../services/auth/auth.service';
import { DataTransferService } from './../../services/data-transfer/data-transfer.service';


class ImageSnippet {
	constructor(public src: string, public file: File) {}
}

@Component({
	selector: 'app-tab-pet-care',
	templateUrl: './tab-pet-care.component.html',
	styleUrls: ['./tab-pet-care.component.css']
})
export class TabPetCareComponent implements OnInit {
	public loading = false;
	public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;

	public uploader: FileUploader;
	private hasDragOver = false;

	public imageSrc: string;
	public isSubmitted = false;
	public provinces = [
		{ id: 0, name: 'San Jose' },
		{ id: 1, name: 'Alajuela' },
		{ id: 2, name: 'Cartago' },
		{ id: 3, name: 'Heredia' },
		{ id: 4, name: 'Guanacaste' },
		{ id: 5, name: 'Puntarenas' },
		{ id: 6, name: 'Limón' }
	]
	public registerPetCare: FormGroup;
	public universities = [
		{ id: 0, name: 'TEC'},
		{ id: 1, name: 'UCR'},
		{ id: 2, name: 'UNA'}
	];

	@Input()
	private editmode = true;

	@Input()
	private url = '';

	@Output()
	private urlChange = new EventEmitter();
	
	public fileOver(e: any): void {
		this.hasDragOver = e;
	}

	constructor(private router: Router, private formBuilder: FormBuilder,
				private api: ApiService, private authService: AuthService,
				private dataTransferService: DataTransferService) {
		this.uploader = new FileUploader({
			//url: 'http://localhost:9090/upload',
			disableMultipart: false,
			autoUpload: true
		});

		this.uploader.response.subscribe(res => {
			// Upload returns a JSON with the image IDx
			this.url = 'http://localhost:9090/get/' + JSON.parse(res).id;
			this.urlChange.emit(this.url);
		});
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
				Swal.fire({
					title: '¡Error de conexión!',
					text: '¡Por favor intente más tarde!',
					type: 'error',
					confirmButtonText: 'Cool'
				});
			});
		}, error => {
			this.loading = false;
			Swal.fire({
				title: '¡Error!',
				text: '¡El usuario no pudo registrarse. Por favor intente más tarde!',
				type: 'error',
				confirmButtonText: 'Cool'
			});
		});
	}

}

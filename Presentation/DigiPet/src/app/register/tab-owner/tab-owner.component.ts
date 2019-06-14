import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from  '@angular/forms';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

import { ApiService } from './../../services/api/api.service';
import { AuthService } from './../../services/auth/auth.service';


class ImageSnippet {
  constructor(public src: string, public file: File) {}
}

@Component({
  selector: 'app-tab-owner',
  templateUrl: './tab-owner.component.html',
  styleUrls: ['./tab-owner.component.css']
})
export class TabOwnerComponent implements OnInit {

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

	public registerOwner: FormGroup;
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
				private api: ApiService, private authService: AuthService) {
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
	}

	get ownerFormControls() {
		return this.registerOwner.controls;
	}

	public register() {
		this.isSubmitted = true;

		let petCareInfo = this.registerOwner.value;
		let petCare = {
			"Password": petCareInfo.password,
			"Name": petCareInfo.name,
			"LastName": petCareInfo.lastname,
			"Email": petCareInfo.email1,
			"Email2": petCareInfo.email2,
			"Mobile": petCareInfo.telephoneNumber,
			"Province": petCareInfo.province,
			"Canton": petCareInfo.canton,
			"Description": petCareInfo.description
		};
		/*
		let response = this.api.registerOwner(petCare);
    response.subscribe(newPetCare => {
    	let responseAuth = this.api.authenticateUser({
    		username: petCare.SchoolId,
    		password: petCare.Password
    	});
    	responseAuth.subscribe(data => {
    		this.authService.login(data);
    		this.authService.setRole('Walker');
      	this.router.navigateByUrl('petcare/profile');
    	}, error => {
    		Swal.fire({
	        title: '¡Error de conexión!',
	        text: '¡Por favor intente más tarde!',
	        type: 'error',
	        confirmButtonText: 'Cool'
	      });
    	});
    }, error => {
    	Swal.fire({
        title: '¡Error!',
        text: '¡El usuario no pudo registrarse. Por favor intente más tarde!',
        type: 'error',
        confirmButtonText: 'Cool'
      });
    });
    */
	}

}

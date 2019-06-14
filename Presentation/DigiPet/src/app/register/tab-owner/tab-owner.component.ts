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
	public pets = [];

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
	public registerPets: FormGroup;
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

		this.pets.push(this.createPet());

		this.registerPets = this.formBuilder.group({
			details: this.formBuilder.array(this.pets)
		});
	}

	get ownerFormControls() {
		return this.registerOwner.controls;
	}

	public addPet() {
		const details = this.registerPets.get('details') as FormArray;
		details.push(this.createPet());
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
				"Description": petInfo.description
			});
		}
		return pets;
	}

	public register() {
		this.isSubmitted = true;
		if (!this.registerOwner.valid || !this.registerPets.valid) {
			return;
		}

		let ownerInfo = this.registerOwner.value;
		let owner = {
			"Password": ownerInfo.password,
			"Name": ownerInfo.name,
			"LastName": ownerInfo.lastname,
			"Email": ownerInfo.email1,
			//"Email2": ownerInfo.email2,
			"Mobile": ownerInfo.telephoneNumber,
			"Province": ownerInfo.province,
			"Canton": ownerInfo.canton,
			"Description": ownerInfo.description,
			"Pets": this.getPets()
		};

		console.log(owner);

		let response = this.api.registerOwner(owner);
    response.subscribe(newOwner => {
    	let responseAuth = this.api.authenticateUser({
    		username: ownerInfo.email1,
    		password: ownerInfo.password
    	});
    	responseAuth.subscribe(data => {
    		console.log(data);
    		this.authService.login(data);
    		this.authService.setRole('PetOwner');
      	this.router.navigateByUrl('owner/profile');
    	}, error => {
    		Swal.fire({
	        title: '¡Error de conexión!',
	        text: '¡Por favor intente más tarde!',
	        type: 'error',
	        confirmButtonText: 'Cool'
	      });
    	});
    }, error => {
    	console.log(error);
    	Swal.fire({
        title: '¡Error!',
        text: '¡El usuario no pudo registrarse. Por favor intente más tarde!',
        type: 'error',
        confirmButtonText: 'Cool'
      });
    });
	}

}

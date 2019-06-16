import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxLoadingComponent } from 'ngx-loading';
import Swal from 'sweetalert2';

import { ApiService } from './../../services/api/api.service';
import { AuthService } from './../../services/auth/auth.service';
import { DataTransferService } from './../../services/data-transfer/data-transfer.service';
import { ImgUploadService } from './../../services/uploads/img-upload/img-upload.service';


window.onclick = function(event) {
	let modal = document.getElementById("myModal");
	if (event.target == modal) {
		modal.style.display = "none";
	}
}

@Component({
	selector: 'app-owner-pets',
	templateUrl: './owner-pets.component.html',
	styleUrls: ['./owner-pets.component.css']
})
export class OwnerPetsComponent implements OnInit {
	public pets;
	public registerPet: FormGroup;

	constructor(private formBuilder: FormBuilder,
				private api: ApiService,
				private authService: AuthService,
				private dataTransferService: DataTransferService,
				private imgUploadService: ImgUploadService) {
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

	public hideModal() {
		document.getElementById('myModal').style.display='none';
	}

	public showModal() {
		document.getElementById('myModal').style.display='block';
	}
}

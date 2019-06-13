import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from  '@angular/forms';
import { Router } from '@angular/router';


@Component({
	selector: 'app-tab-pet-care',
	templateUrl: './tab-pet-care.component.html',
	styleUrls: ['./tab-pet-care.component.css']
})
export class TabPetCareComponent implements OnInit {
	public registerPetCare: FormGroup;
	public isSubmitted = false;

	constructor(private router: Router, private formBuilder: FormBuilder) { }

	ngOnInit() {
		this.registerPetCare = this.formBuilder.group(
			{
				name: ['', Validators.compose([
					Validators.required,
					Validators.maxLength(30)
				])],
				lastname: ['', Validators.compose([
					Validators.required,
					Validators.maxLength(30)
				])],
				canton: '',
				province: ['', Validators.required],
				university: ['', Validators.required],
				carnet: ['', Validators.required],
				email1: ['', Validators.required],
				email2: '',
				telephoneNumber: ['', Validators.required],
				password: ['', Validators.required],
				photo: ['', Validators.required],
				description: ['', Validators.required]
			}
		);
	}

	get petCareFormControls() {
		return this.registerPetCare.controls;
	}

	public register() {
		this.isSubmitted = true;

		console.log(this.registerPetCare.value);
		console.log(this.registerPetCare)
	}
}

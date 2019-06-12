import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from  '@angular/forms';
import { Router } from '@angular/router';


@Component({
	selector: 'app-tab-pet-care',
	templateUrl: './tab-pet-care.component.html',
	styleUrls: ['./tab-pet-care.component.css']
})
export class TabPetCareComponent implements OnInit {
	public registerPetCare1: FormGroup;
	public registerPetCare2: FormGroup;
	public isSubmitted = false;

	constructor(private router: Router, private formBuilder: FormBuilder) { }

	ngOnInit() {
		this.registerPetCare1 = this.formBuilder.group(
			{
				name: ['', Validators.required],
				lastname: ['', Validators.required],
				canton: ['', Validators.required],
				province: ['', Validators.required],
				university: ['', Validators.required],
				carnet: ['', Validators.required],
				email1: ['', Validators.required],
				email2: ['', Validators.unrequired],
				telephoneNumber: ['', Validators.required],
				password: ['', Validators.required]
			}
		);
		this.registerPetCare2 = this.formBuilder.group(
			{
				photo: ['', Validators.required],
				description: ['', Validators.required]
			}
		);
	}

	get petCareFormControls1() {
		return this.registerPetCare1.controls;
	}

	get petCareFormControls2() {
		return this.registerPetCare2.controls;
	}

	public register() {
		this.isSubmitted = true;

		console.log(this.registerPetCare1.value, this.registerPetCare2.value);
	}

}

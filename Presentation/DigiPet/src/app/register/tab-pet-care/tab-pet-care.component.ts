import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from  '@angular/forms';
import { Router } from '@angular/router';


@Component({
	selector: 'app-tab-pet-care',
	templateUrl: './tab-pet-care.component.html',
	styleUrls: ['./tab-pet-care.component.css']
})
export class TabPetCareComponent implements OnInit {
	public registerOwner: FormGroup;
	public registerPet: FormGroup;
	public isSubmitted = false;

	constructor(private router: Router, private formBuilder: FormBuilder) { }

	ngOnInit() {
		this.registerOwner = this.formBuilder.group(
  		{
  			name: ['', Validators.required],
				lastname: ['', Validators.required],
				province: ['', Validators.required],
				canton: ['', Validators.required],
				email1: ['', Validators.required],
				email2: ['', Validators.required],
				telephoneNumber: ['', Validators.required],
				photo: ['', Validators.required],
				password: ['', Validators.required],
				description: ['', Validators.required]
			}
  	);
  	this.registerPet = this.formBuilder.group(
  		{
  			name: ['', Validators.required],
				breed: ['', Validators.required],
				age: ['', Validators.required],
				size: ['', Validators.required],
				photo: ['', Validators.required],
				description: ['', Validators.required]
			}
  	);
  }

  get ownerFormControls() {
  	return this.registerOwner.controls;
  }

  get petFormControls() {
  	return this.registerPet.controls;
  }

}

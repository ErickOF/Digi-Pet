import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from  '@angular/forms';
import { Router } from '@angular/router';


class ImageSnippet {
  constructor(public src: string, public file: File) {}
}

@Component({
	selector: 'app-tab-pet-care',
	templateUrl: './tab-pet-care.component.html',
	styleUrls: ['./tab-pet-care.component.css']
})
export class TabPetCareComponent implements OnInit {
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

	constructor(private router: Router, private formBuilder: FormBuilder) {
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
				province: ['', Validators.required],
				canton: '',
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
				acceptWalks: 'false',
				provinces: new FormArray([])
			}
		);

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

	public register() {
		this.isSubmitted = true;

		console.log(this.registerPetCare.value);
		console.log(this.registerPetCare)
	}
	
}

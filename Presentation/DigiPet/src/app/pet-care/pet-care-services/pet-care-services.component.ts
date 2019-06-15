import { Component, OnInit } from '@angular/core';


@Component({
	selector: 'app-pet-care-services',
	templateUrl: './pet-care-services.component.html',
	styleUrls: ['./pet-care-services.component.css']
})
export class PetCareServicesComponent implements OnInit {

	public services = [];

	constructor() { }

	ngOnInit() {
	}

}

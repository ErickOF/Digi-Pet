import { Component, OnInit } from '@angular/core';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';


@Component({
	selector: 'app-pet-care-information',
	templateUrl: './pet-care-information.component.html',
	styleUrls: ['./pet-care-information.component.css']
})
export class PetCareInformationComponent implements OnInit  {
	public image = './assets/default-avatar.png';

	public petCare;

	constructor(private dataTransferService: DataTransferService) {
		this.petCare = this.dataTransferService.getUserInformation();
		this.petCare.dateCreated = this.petCare.dateCreated.split('T')[0];
		this.petCare.doesOtherProvinces = this.petCare.doesOtherProvinces? "Sí" : "No";
		this.petCare.otherProvinces = this.petCare.otherProvinces.join(', ');
	}

	ngOnInit() {
	}

}

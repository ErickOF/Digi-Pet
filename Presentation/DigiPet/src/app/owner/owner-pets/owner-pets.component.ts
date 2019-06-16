import { Component, OnInit } from '@angular/core';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';


@Component({
	selector: 'app-owner-pets',
	templateUrl: './owner-pets.component.html',
	styleUrls: ['./owner-pets.component.css']
})
export class OwnerPetsComponent implements OnInit {

	public pets;

	constructor(private dataTransferService: DataTransferService) {
		this.pets = this.dataTransferService.getUserInformation().pets;
		for (let i = 0; i < this.pets.length; i++) {
			this.pets[i].dateCreated = this.pets[i].dateCreated.split('T')[0];
		}
		console.log(this.pets[0]);
	}

	ngOnInit() {
	}
}

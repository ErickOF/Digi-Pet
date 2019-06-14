import { Component, OnInit } from '@angular/core';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';


@Component({
	selector: 'app-owner-pets',
	templateUrl: './owner-pets.component.html',
	styleUrls: ['./owner-pets.component.css']
})
export class OwnerPetsComponent implements OnInit {

	public pets;

	constructor(private dataTransferService: DataTransferService) { }

	ngOnInit() {
		this.pets = this.dataTransferService.getUserInformation().pets;
		console.log(this.pets);
	}
}

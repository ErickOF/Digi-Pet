import { Component, OnInit } from '@angular/core';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';


@Component({
	selector: 'app-owner-information',
	templateUrl: './owner-information.component.html',
	styleUrls: ['./owner-information.component.css']
})
export class OwnerInformationComponent implements OnInit {
	public image = './assets/default-avatar.png';

	public owner;

	constructor(private dataTransferService: DataTransferService) {
		this.owner = this.dataTransferService.getUserInformation();
		this.owner.dateCreated = this.owner.dateCreated.split('T')[0];
		console.log(this.owner);
	}

	ngOnInit() {
	}

}

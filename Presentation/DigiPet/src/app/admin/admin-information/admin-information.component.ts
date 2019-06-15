import { Component, OnInit } from '@angular/core';

import { DataTransferService } from './../../services/data-transfer/data-transfer.service';


@Component({
	selector: 'app-admin-information',
	templateUrl: './admin-information.component.html',
	styleUrls: ['./admin-information.component.css']
})
export class AdminInformationComponent implements OnInit {
	
	public image = './assets/default-avatar.png';

	public admin;

	constructor(private dataTransferService: DataTransferService) {
		this.admin = this.dataTransferService.getUserInformation();
	}

	ngOnInit() {
	}

}
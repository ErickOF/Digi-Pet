import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { AuthService } from './../../auth/auth.service';
import { DataTransferService } from './../../data-transfer/data-transfer.service';


@Injectable({
	providedIn: 'root'
})
export class PetCareGuard implements CanActivate {

	constructor(private authService: AuthService,
				private dataTransferService: DataTransferService) {}

	canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) :
				Observable<boolean> | Promise<boolean> | boolean {
		return this.authService.isLoggedIn() && this.dataTransferService.getRole() == 'Walker';
	}

}

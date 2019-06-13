import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { AuthService } from './../../auth/auth.service';


@Injectable({
  providedIn: 'root'
})
export class OwnerGuard implements CanActivate {
  
  constructor(private authService: AuthService) {}

	canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) :
  				Observable<boolean> | Promise<boolean> | boolean {
  		return this.authService.isLoggedIn() && this.authService.getRole() == 'PetOwner';
	}

}

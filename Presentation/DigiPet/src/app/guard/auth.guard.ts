import { Injectable } from '@angular/core';
import { CanActive, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './../services/auth/auth.service'

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  
  constructor(private authService: AuthService) {}

  canActive(next: ActivatedRoutedSnapshot, state: RouterStateSnapshot) :
  			Observale<boolean> | Promise<boolean> | boolean {
  	return this.authService.isLoggedIn();
  }

}

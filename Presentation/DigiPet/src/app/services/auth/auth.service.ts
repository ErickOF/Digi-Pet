import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public login(userInfo) {
    localStorage.setItem('ACCESS_TOKEN', "access_token");
  }

  public logout() {
    localStorage.removeItem('ACCESS_TOKEN');
  }

  public isLoggedIn() {
    return localStorage.getItem('ACCESS_TOKEN') !== null;
  }

  public getRole() {
    return sessionStorage.getItem('ROLE');
  }

  public setRole(role) {
    sessionStorage.setItem('ROLE', role);
  }

}

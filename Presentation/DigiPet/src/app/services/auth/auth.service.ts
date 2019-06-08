import { Injectable } from '@angular/core';

import { UserLogin } from '../../models/user-login';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public login(userInfo: UserLogin) {
    localStorage.setItem('ACCESS_TOKEN', "access_token");
  }

  public logout() {
    localStorage.removeItem('ACCESS_TOKEN');
  }

  public isLoggedIn() {
    return localStorage.getItem('ACCESS_TOKEN') !== null;
  }

  public getRole() {
    return sessionStorage.getItem('role');
  }

  public setRole(role) {
    sessionStorage.setItem('role', role);
  }

  public setUser(data) {
    sessionStorage.setItem('UserInfo', data);
  }

}

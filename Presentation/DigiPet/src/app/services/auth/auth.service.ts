import { Injectable } from '@angular/core';

import { UserLogin } from '../../interfaces/user-login.ts';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public login(userInfo: UserLogin) {
    localStorage.setItem('ACCESS_TOKEN', "access_token");
  }

  public isLoggedIn() {
    return localStorage.getItem('ACCESS_TOKEN') !== null;

  }

  public logout() {
    localStorage.removeItem('ACCESS_TOKEN');
  }
}

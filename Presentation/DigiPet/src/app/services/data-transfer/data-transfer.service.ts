import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class DataTransferService {

  constructor() { }

  public deleteAccessToken() {
    localStorage.removeItem('ACCESS_TOKEN');
  }

  public getAccessToken() {
    return JSON.parse(localStorage.getItem('ACCESS_TOKEN'));
  }

  public getRole() {
    return localStorage.getItem('ROLE');
  }

  public getUserInformation() {
    return JSON.parse(localStorage.getItem('USER_INFO'));
  }

  public setAccessToken(token) {
    localStorage.setItem('ACCESS_TOKEN', JSON.stringify(token));
  }

  public setRole(role) {
    localStorage.setItem('ROLE', role);
  }

  public setUserInformation(userInformation) {
  	localStorage.setItem('USER_INFO', JSON.stringify(userInformation));
  }

}

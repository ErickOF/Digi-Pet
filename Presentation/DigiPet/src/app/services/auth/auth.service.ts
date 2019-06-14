import { Injectable } from '@angular/core';

import { DataTransferService } from './../data-transfer/data-transfer.service';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private dataTransferService: DataTransferService) {}

  public login(token) {
    this.dataTransferService.setAccessToken(token);
  }

  public logout() {
    this.dataTransferService.deleteAccessToken();
  }

  public isLoggedIn() {
    return this.dataTransferService.getAccessToken() !== null;
  }

}

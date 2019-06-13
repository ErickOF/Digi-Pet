import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";

import { Urls } from './../../configuration/urls'


@Injectable({
	providedIn: 'root'
})
export class ApiService {
	
	private httpOptions = {
		headers: new HttpHeaders({
			'Content-Type': 'application/x-www-form-urlencoded'
		})
	};

	constructor(private http: HttpClient) { }

	public registerPetCare(PetCare) {
	}

	public registerOwner(owner) {
	}

	public authenticateUser(userLogin): any {
		const body = new HttpParams()
										.set('UserName', userLogin.username)
										.set('Password', userLogin.password);
		return this.http.post(Urls.baseUrl + Urls.authenticateUser, body, this.httpOptions)
	}
}

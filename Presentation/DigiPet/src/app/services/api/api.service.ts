import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";

import { Urls } from './../../configuration/urls';


@Injectable({
	providedIn: 'root'
})
export class ApiService {
	
	private httpOptionsJSON = {
		headers: new HttpHeaders({
			'Content-Type': 'application/json'
		})
	};

	private httpOptionsUrlEncoded = {
		headers: new HttpHeaders({
			'Content-Type': 'application/x-www-form-urlencoded'
		})
	};

	constructor(private http: HttpClient) { }

	public authenticateUser(userLogin): any {
		const body = new HttpParams()
						.set('UserName', userLogin.username)
						.set('Password', userLogin.password);
		return this.http.post(Urls.baseUrl + Urls.authenticateUser, body,
						  					this.httpOptionsUrlEncoded);
	}

	public registerOwner(owner) {
		return this.http.post(Urls.baseUrl + Urls.createOwner, owner,
											this.httpOptionsJSON);
	}

	public registerPet(pet, token) {
		return this.http.post(Urls.baseUrl + Urls.createPet, pet,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
								.set('Content-Type', 'application/json'),
							observe: 'response'
						});
	}

	public registerPetCare(petCare) {
		return this.http.post(Urls.baseUrl + Urls.createPetCare, petCare,
											this.httpOptionsJSON);
	}

}

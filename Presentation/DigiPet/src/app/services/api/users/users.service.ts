import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";

import { Urls } from './../../../configuration/urls';


@Injectable({
	providedIn: 'root'
})
export class UsersService {

	constructor(private http: HttpClient) { }

	public getOwner(token: string) {
		return this.http.get(Urls.baseUrl + Urls.getOwnerProfile,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

	public getPetCare(token: string) {
		return this.http.get(Urls.baseUrl + Urls.getPetCareProfile,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

}

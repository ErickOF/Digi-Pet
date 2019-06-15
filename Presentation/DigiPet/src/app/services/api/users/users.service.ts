import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";

import { Urls } from './../../../configuration/urls';


@Injectable({
	providedIn: 'root'
})
export class UsersService {

	constructor(private http: HttpClient) { }

	public activePetCare(token: string, id: number) {
		return this.http.post(Urls.baseUrl + Urls.activePetCare + id, {},
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

	public blockPetCare(token: string, id: number) {
		return this.http.post(Urls.baseUrl + Urls.blockPetCare + id, {},
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

	public getAllPetCares(token: string) {
		return this.http.get(Urls.baseUrl + Urls.getAllPetCares,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

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

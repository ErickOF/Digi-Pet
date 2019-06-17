import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";

import { Urls } from './../../../configuration/urls';


@Injectable({
	providedIn: 'root'
})
export class UsersService {

	constructor(private http: HttpClient) { }

	public activePetCare(token: string, id: number) {
		return this.http
					.post(Urls.baseUrl + Urls.activePetCare + id, {},
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`),
							observe: 'response'
						});
	}

	public blockPetCare(token: string, id: number) {
		return this.http
					.post(Urls.baseUrl + Urls.blockPetCare + id, {},
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`),
							observe: 'response'
						});
	}

	public getAllPetCares(token: string) {
		return this.http
					.get(Urls.baseUrl + Urls.getAllPetCares,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

	public denyUserReport(token: string, id: number) {
		return this.http
					.delete(Urls.baseUrl + Urls.denyUserReport + id,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

	public getOwner(token: string) {
		return this.http
					.get(Urls.baseUrl + Urls.getOwnerProfile,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

	public getPetCare(token: string) {
		return this.http
					.get(Urls.baseUrl + Urls.getPetCareProfile,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

	public getPendingReportCards(token: string) {
		return this.http
					.get(Urls.baseUrl + Urls.getPendingReportCards,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

	public getUpComingWalksByOwner(token: string) {
		return this.http.get(Urls.baseUrl + Urls.getUpComingWalksByOwner,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

	public getUpComingWalksByPetCare(token: string) {
		return this.http
					.get(Urls.baseUrl + Urls.getUpComingWalksByPetCare,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

	public getUserReports(token: string) {
		return this.http
					.get(Urls.baseUrl + Urls.getUserReports,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

	public getWalksHistoryByPetCare(token: string) {
		return this.http
					.get(Urls.baseUrl + Urls.getWalksHistoryByPetCare,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
						});
	}

	public requestWalkService(token: string, walkService) {
		return this.http
					.post(Urls.baseUrl + Urls.requestWalkService, walkService,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
								.set('Content-Type', 'application/json'),
							observe: 'response'
						});
	}

	public sendReportCard(token: string, reportCard) {
		return this.http
					.post(Urls.baseUrl + Urls.setPendintReportCard, reportCard,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
								.set('Content-Type', 'application/json'),
							observe: 'response'
						});
	}

	public setSchedule(token: string, schedule) {
		return this.http
					.post(Urls.baseUrl + Urls.setSchedule, schedule,
						{
							headers: new HttpHeaders()
								.set('Authorization', `Bearer ${token}`)
								.set('Content-Type', 'application/json'),
							observe: 'response'
						});
	}

}

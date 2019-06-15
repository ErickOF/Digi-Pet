export class Urls {
	// Url for testing
	//public static baseUrl: string = 'https://localhost:69000/';
	// Url in hosting
	public static baseUrl: string = 'https://digipet-backend.herokuapp.com/';
	public static authenticateUser: string = 'users/authenticate'
	public static createOwner: string = 'api/Owners';
	public static createPet: string = 'api/Pets/'
	public static createPetCare: string = 'api/Walkers';
	public static getAllOwners: string = 'api/Owners';
	public static getPet: string = 'api/Pets/';
	public static getOwnerProfile: string = 'api/Owners/getprofile';
	public static getPetCareProfile: string = 'api/walkers/getprofile';
}

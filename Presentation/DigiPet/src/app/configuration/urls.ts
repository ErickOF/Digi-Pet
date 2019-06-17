export class Urls {
	// Url local
	//public static baseUrl: string = 'https://localhost:69000/';
	// Url hosting
	public static baseUrl: string = 'https://digipet-backend.herokuapp.com/';
	// Auth
	public static authenticateUser: string = 'users/authenticate';
	// Block & unblock
	public static activePetCare: string = 'api/admins/unblockWalker/';
	public static blockPetCare: string = 'api/admins/blockWalker/';
	// Create Users
	public static createOwner: string = 'api/Owners';
	public static createPet: string = 'api/Pets/'
	public static createPetCare: string = 'api/Walkers';
	// Get all users
	public static getAllOwners: string = 'api/Owners';
	public static getAllPetCares: string = 'api/walkers';
	// Get profiles
	public static getPet: string = 'api/Pets/';
	public static getOwnerProfile: string = 'api/Owners/getprofile';
	public static getPetCareProfile: string = 'api/walkers/getprofile';
	// Report cards
	public static getPendingReportCards: string = 'api/walkers/pendingReport';
	// Get services
	public static getUpComingWalksByOwner: string = 'api/owners/upcoming';
	public static getUpComingWalksByPetCare: string = 'api/walkers/upcoming';
	// Request Services
	public static requestWalkService: string = 'api/owners/requestWalk';
	// Set
	public static setPendintReportCard: string = 'api/walkers/postReport';
	public static setSchedule: string = 'api/walkers/schedule/';
}

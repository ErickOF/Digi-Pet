import { Injectable } from '@angular/core';
import { AngularFireStorage } from 'angularfire2/storage';


@Injectable({
	providedIn: 'root'
})
export class ImgUploadService {

	constructor(private storage: AngularFireStorage) { }

	public getImage(filePath) {
		let ref = this.storage.ref(filePath);
		return ref.getDownloadURL();
	}

	public uploadFile(filePath, file) {
		const task = this.storage.upload(filePath, file);
		return task.percentageChanges();
	}

}

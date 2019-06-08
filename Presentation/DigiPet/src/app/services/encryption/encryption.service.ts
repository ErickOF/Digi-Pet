import { Injectable } from '@angular/core';
import { sha224, sha256 } from 'js-sha256';

var crypto = require('crypto');


@Injectable({
  providedIn: 'root'
})
export class EncryptionService {
	private EMAC_SHA224_KEY = 'key224';
	private EMAC_SHA256_KEY = 'key256';
	private ITERATIONS = 10000;
	private SALT = '285D6904B215B81CD18207CF57E6F0B86125083B3A3AC8427BF99F9E420080C66C32CCAC5B4C9DC9BF11C87726D9DE925A1408BB52E72F7336EE28D8697337295BE9755A222C01EB439171C692CCBBECF57EBA89460FD9C151D560932C4B2F085DF895BDB8302FE33783A1B25DD15FB93C71D2BA14259AA459008A2FB3455E72';

	constructor() { }

    public encriptSha256(item) {
        return sha256(item);
    }

    public encriptEmacSha224(item) {
    	return sha224.hmac(this.EMAC_SHA224_KEY, item);
    }

    public encriptEmacSha256(item) {
    	return sha256.hmac(this.EMAC_SHA256_KEY, item);
    }

    public encriptPBKDF2_HMAC_Sha256(item) {
    	return crypto.pbkdf2Sync(item, this.SALT, this.ITERATIONS, 256, 'sha256');
    }

}

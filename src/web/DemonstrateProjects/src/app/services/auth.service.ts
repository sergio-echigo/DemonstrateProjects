import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { Account } from '../models/account';
import jwtDecode from 'jwt-decode';
import { DecodedToken } from '../models/decodedToken';
import { apiUrl, withBodyHttpOptions, withoutBodyHttpOptions } from '..';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpClient : HttpClient) {
  }

  signUp(user : any) : Observable<any> {
    return this.httpClient.post(this.apiUrl + "register", user, withBodyHttpOptions);
  }

  signIn(user : any) : Observable<any> {
    return this.httpClient.post(this.apiUrl + "login", user, withBodyHttpOptions);
  }

  logout() : Observable<any> {
    return this.httpClient.post(this.apiUrl + "logout", null, withoutBodyHttpOptions);
  }

  refresh() : Observable<any> {
    return this.httpClient.post(this.apiUrl + "refresh", null, withoutBodyHttpOptions);
  }

  deleteAccount(pswd : string) : Observable<any> {
    return this.httpClient.delete("https://localhost:7153/account?pswd=" + pswd, withoutBodyHttpOptions)
  }

  isAuthenticated() : boolean {
    return (this.getCookie("d_a") != null && this.getCookie("d_a") != "");
  }

  isExpired() : boolean {
    let token = this.getCookie("d_a");
    if (token) {
      let decoded = jwtDecode<DecodedToken>(token);
      let exp = (new Date(0)).setUTCSeconds(decoded.exp);

      return !(exp.valueOf() > new Date().valueOf())
    } 
    
    return false;
  }

  getAccount() : Observable<Account> {
    return this.httpClient.get<Account>("https://localhost:7153/account", withoutBodyHttpOptions);
  }

  getDaToken() : string | null {
    return this.getCookie("d_a");
  }

  private readonly apiUrl : string = apiUrl + "auth/"; 

  // https://stackoverflow.com/a/25346429/16050768
  private sescape(s : string) { return s.replace(/([.*+?\^$(){}|\[\]\/\\])/g, '\\$1'); }
  private getCookie(name : string) : string | null {
    var match = document.cookie.match(RegExp('(?:^|;\\s*)' + this.sescape(name) + '=([^;]*)'));
    return match ? match[1] : null;
  }
}

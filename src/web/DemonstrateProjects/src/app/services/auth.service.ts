import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { Account } from '../models/account';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpClient : HttpClient) {
  }

  isAuthenticated() : boolean {
    return (this.getCookie("d_a") != null && this.getCookie("d_a") != "");
  }

  signUp(user : any) : Observable<any> {
    return this.httpClient.post(this.apiUrl + "/register", user, this.httpOptions);
  }

  signIn(user : any) : Observable<any> {
    return this.httpClient.post(this.apiUrl + "/login", user, this.httpOptions);
  }

  refresh() : Observable<any> {
    return this.httpClient.post(this.apiUrl + "/refresh", null, this.httpOptions);
  }

  getAccount() : Observable<Account> {
    return this.httpClient.get<Account>("https://localhost:7153/account", { withCredentials: true });
  }

  deleteAccount(pswd : string) : Observable<any> {
    return this.httpClient.delete("https://localhost:7153/account?pswd=" + pswd, { withCredentials: true })
  }

  private readonly apiUrl : string = "https://localhost:7153/auth"; 
  private httpOptions = {
    headers: new HttpHeaders({'Content-Type': 'application/json'}),
    withCredentials: true
  }

  // https://stackoverflow.com/a/25346429/16050768
  private sescape(s : string) { return s.replace(/([.*+?\^$(){}|\[\]\/\\])/g, '\\$1'); }
  private getCookie(name : string) : string | null {
    var match = document.cookie.match(RegExp('(?:^|;\\s*)' + this.sescape(name) + '=([^;]*)'));
    return match ? match[1] : null;
  }
}

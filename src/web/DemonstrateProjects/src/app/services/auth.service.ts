import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpClient : HttpClient) {
  }

  readonly mainRoute : string = "https://localhost:7153";

  signup(user : any) : Observable<any> {
    return this.httpClient.post(this.apiUrl + "/register", user, this.httpOptions);
  }

  signin(user : any) : Observable<any> {
    return this.httpClient.post(this.apiUrl + "/login", user, this.httpOptions);
  }

  refresh() : Observable<any> {
    return this.httpClient.post(this.apiUrl + "/refresh", null, this.httpOptions);
  }

  private readonly apiUrl : string = "https://localhost:7153/auth"; 
  private httpOptions = {
    headers: new HttpHeaders({'Content-Type': 'application/json'}),
    withCredentials: true
  }
}

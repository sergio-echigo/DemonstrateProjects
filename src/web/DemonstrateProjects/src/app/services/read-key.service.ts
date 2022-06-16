import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ReadKey } from '../models/key';

@Injectable({
  providedIn: 'root'
})
export class ReadKeyService {

  constructor(private httpClient : HttpClient) { }

  create(object: any) : Observable<any> {
    return this.httpClient.post(this.apiUrl + "/new", object, this.httpOptions);
  }

  getAll() : Observable<ReadKey[]> {
    return this.httpClient.get<ReadKey[]>(this.apiUrl + "/personal", this.httpOptions);
  }

  get(key: string) : Observable<ReadKey> {
    return this.httpClient.get<ReadKey>(this.apiUrl + "/" + key, this.httpOptions);
  }

  delete(key: string) : Observable<any> {
    return this.httpClient.delete(this.apiUrl + "/delete?key=" + key, { withCredentials: true });
  }

  private readonly apiUrl : string = 'https://localhost:7153/keys';
  private httpOptions = {
    headers: new HttpHeaders({'Content-Type': 'application/json'}),
    withCredentials: true
  }
}

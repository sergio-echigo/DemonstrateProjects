import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { apiUrl, withBodyHttpOptions, withoutBodyHttpOptions } from '..';
import { ReadKey } from '../models/key';

@Injectable({
  providedIn: 'root'
})
export class ReadKeyService {

  constructor(private httpClient : HttpClient) { }

  create(object: any) : Observable<any> {
    return this.httpClient.post(this.apiUrl + "new", object, withBodyHttpOptions);
  }

  getAll() : Observable<ReadKey[]> {
    return this.httpClient.get<ReadKey[]>(this.apiUrl + "personal", withBodyHttpOptions);
  }

  get(key: string) : Observable<ReadKey> {
    return this.httpClient.get<ReadKey>(this.apiUrl + key, withBodyHttpOptions);
  }

  delete(key: string) : Observable<any> {
    return this.httpClient.delete(this.apiUrl + "delete?key=" + key, withoutBodyHttpOptions);
  }

  private readonly apiUrl : string = apiUrl + "keys/";
}

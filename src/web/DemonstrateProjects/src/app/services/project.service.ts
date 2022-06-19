import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { apiUrl, withBodyHttpOptions, withoutBodyHttpOptions } from '..';
import { Project } from '../models/project';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  constructor(private httpClient : HttpClient) { 

  }

  create(p : any) : Observable<any> {
    return this.httpClient.post(this.apiUrl + "create", p, withBodyHttpOptions);
  }

  getAll() : Observable<Project[]> {
    return this.httpClient.get<Project[]>(this.apiUrl, withoutBodyHttpOptions);
  }
  
  get(index : Number) : Observable<Project> {
    return this.httpClient.get<Project>(this.apiUrl + index, withoutBodyHttpOptions);
  }

  getByKey(key : string) : Observable<Project[]> {
    return this.httpClient.post<Project[]>(this.apiUrl + "key", { key: key }, withoutBodyHttpOptions);
  }

  put(index: Number, updated : Project) : Observable<any> {
    return this.httpClient.put(this.apiUrl + index + "edit", updated, withBodyHttpOptions)
  }

  delete(index: Number) : Observable<any> {
    return this.httpClient.delete(this.apiUrl + "delete?index=" + index, withoutBodyHttpOptions);
  }

  changeImg(file : any, index : string) : Observable<any> {
    let formData : FormData = new FormData();
    formData.append('file', file);

    return this.httpClient.post(this.apiUrl + "/" + index + "/img", formData, withoutBodyHttpOptions);
  }

  private readonly apiUrl : string = apiUrl + "projects/"
}

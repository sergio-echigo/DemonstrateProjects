import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Project } from '../models/project';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  constructor(private httpClient : HttpClient) { 

  }

  create(p : any) : Observable<any> {
    return this.httpClient.post(this.apiUrl + "/create", p, this.httpOptions);
  }

  getAll() : Observable<Project[]> {
    return this.httpClient.get<Project[]>(this.apiUrl, this.httpOptions);
  }
  
  get(index : Number) : Observable<Project> {
    return this.httpClient.get<Project>(this.apiUrl + "/" + index, this.httpOptions);
  }

  put(index: Number, updated : Project) : Observable<any> {
    return this.httpClient.put(this.apiUrl + "/" + index + "/edit", updated, this.httpOptions)
  }

  delete(index: Number) : Observable<any> {
    return this.httpClient.delete(this.apiUrl + "/delete?index=" + index, { withCredentials: true });
  }

  changeImg(file : any, index : string) : Observable<any> {
    let formData : FormData = new FormData();
    formData.append('file', file);

    return this.httpClient.post(this.apiUrl + "/" + index + "/img", formData, { withCredentials: true });
  }

  private readonly apiUrl : string = 'https://localhost:7153/projects';
  private httpOptions = {
    headers: new HttpHeaders({'Content-Type': 'application/json'}),
    withCredentials: true
  }
}

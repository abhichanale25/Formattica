import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environment.ts/environment.dev';

@Injectable({
  providedIn: 'root'
})
export class ComparisionService {

  constructor(private http: HttpClient) { }

  compareCode(obj:any){
    return this.http.post(environment.apiBaseUrl + "Comparison/compare", obj)
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environment.ts/environment.dev';

@Injectable({
  providedIn: 'root'
})
export class FormatterService {

  constructor(
    private http: HttpClient
  ) { }

  formatContent(obj:any){
    return this.http.post(environment.apiBaseUrl + "Formatter/format", obj)
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment.ts/environment.dev';

@Injectable({
  providedIn: 'root'
})
export class CompressorService {

  constructor(private http:HttpClient) { }

  formatDoc(formData: FormData): Observable<any> {
    return this.http.post(environment.apiBaseUrl + "Compression/compress", formData, {
      responseType: 'blob'
    });
  }
}

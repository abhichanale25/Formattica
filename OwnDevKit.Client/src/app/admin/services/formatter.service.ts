import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environment.ts/environment.dev';

@Injectable({
  providedIn: 'root'
})
export class FormatterService {

  constructor(
    private http: HttpClient
  ) { }

//   formatContent(content: string, formatType: string) {
//   const params = new HttpParams()
//     .set('content', content)
//     .set('formatType', formatType);

//   return this.http.post(environment.apiBaseUrl + "Formatter/format", null, { params });
// }

formatContent(payload: { content: string; formatType: string }) {
    return this.http.post(`${environment.apiBaseUrl}Formatter/format`, payload);
  }


}

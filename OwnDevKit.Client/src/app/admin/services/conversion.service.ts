import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environment.ts/environment.dev';

@Injectable({
  providedIn: 'root'
})
export class ConversionService {

  constructor(private http:HttpClient) { }

  // fileConversion(file: any, format: string){
  //   const params = new HttpParams().set('format', format);
  //   return this.http.post(
  //     `${environment.apiBaseUrl}Conversion/convert`, file, { params } 
  //   );
  // }

  fileConversion(formData: FormData, targetFormat: string) {
    const url = `${environment.apiBaseUrl}Conversion/convert?targetFormat=${encodeURIComponent(targetFormat)}`;
    return this.http.post(url, formData, { responseType: 'blob' });

  }
  

}

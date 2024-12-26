import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'  // Makes the service available application-wide
})
export class ApiService {
  baseUrl = 'http://simplebilling:8080';  // Global URL

  getUrl(endpoint: string): string {
    return `${this.baseUrl}${endpoint}`;
  }
}

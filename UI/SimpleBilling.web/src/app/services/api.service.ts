import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'  // Makes the service available application-wide
})
export class ApiService {
  baseUrl = 'http://34.58.234.179:8080';  // Global URL

  getUrl(endpoint: string): string {
    return `${this.baseUrl}${endpoint}`;
  }
}

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'  // Makes the service available application-wide
})
export class ApiService {
  baseUrl = 'http://34.159.41.202:8081';  // Global URL

  getUrl(endpoint: string): string {
    return `${this.baseUrl}${endpoint}`;
  }
}

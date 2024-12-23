import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'  // Makes the service available application-wide
})
export class ApiService {
  baseUrl = 'http://simplebilling-alb-1761924504.eu-north-1.elb.amazonaws.com/;';  // Global URL

  getUrl(endpoint: string): string {
    return `${this.baseUrl}${endpoint}`;
  }
}

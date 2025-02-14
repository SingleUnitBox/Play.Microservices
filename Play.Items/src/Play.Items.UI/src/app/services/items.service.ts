import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ItemsService {
  private apiUrl = 'https://localhost:5001/items'; // Change this if needed

  constructor(private http: HttpClient) {}

  getItems(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Entry } from './entry';

@Injectable({
  providedIn: 'root'
})
export class EntryService {
  private apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  getEntries(): Observable<Entry[]> {
    return this.httpClient.get<Entry[]>(`${this.apiUrl}entries`);
  }
}

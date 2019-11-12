import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
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

  createEntry(entry: Entry): Observable<Entry> {
    return this.httpClient.post<Entry>(`${this.apiUrl}entries`, entry);
  }

  updateEntry(entry: Entry) {
    return this.httpClient.put(`${this.apiUrl}entries/${entry.id}`, entry);
  }
}

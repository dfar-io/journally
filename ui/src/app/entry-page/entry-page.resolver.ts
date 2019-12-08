import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { EntryResolved } from '../entries/entry';
import { EntryService } from '../entries/entry.service';

// Using error handling solution found at:
// https://stackoverflow.com/questions/43898934/how-to-handle-error-in-a-resolver

@Injectable({
  providedIn: 'root'
})
export class EntryResolver implements Resolve<EntryResolved> {
  constructor(private entryService: EntryService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<EntryResolved> {
    return this.entryService.getEntry(route.params.id).pipe(
      map(entry => ({ entry })),
      catchError(error => {
        const message = `Retrieval error: ${error}`;
        return of({ entry: null, error: message });
      })
    );
  }
}

import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { Entry } from '../entries/entry';
import { EntryService } from '../entries/entry.service';

@Injectable({
  providedIn: 'root'
})
export class EntryResolver implements Resolve<Entry> {
  constructor(private entryService: EntryService) {}

  resolve(
    route: ActivatedRouteSnapshot
  ): Entry | Observable<Entry> | Promise<Entry> {
    return this.entryService.getEntry(route.params.id);
  }
}

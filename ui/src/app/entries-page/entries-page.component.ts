import { Component, OnInit } from '@angular/core';
import { Entry } from '../entries/entry';
import { EntryService } from '../entries/entry.service';

@Component({
  selector: 'app-entries-page',
  templateUrl: './entries-page.component.html',
  styleUrls: ['./entries-page.component.css']
})
export class EntriesPageComponent implements OnInit {
  entries: Entry[];
  errorMessage: string;
  areEntriesLoaded = false;

  constructor(private entryService: EntryService) {}

  ngOnInit() {
    this.entryService.getEntries().subscribe(
      response => {
        this.areEntriesLoaded = true;
        this.entries = response;
      },
      () => {
        this.areEntriesLoaded = true;
        this.errorMessage =
          'An unexpected error occurred while loading your entries.';
      }
    );
  }

  closeAlert() {
    this.errorMessage = '';
  }

  isNullOrWhitespace(str: string): boolean {
    if (str === undefined) {
      return true;
    }

    return str === null || str.match(/^ *$/) !== null;
  }
}

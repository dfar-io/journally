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

  constructor(private entryService: EntryService) {}

  ngOnInit() {
    this.entryService
      .getEntries()
      .subscribe(response => (this.entries = response));
  }
}

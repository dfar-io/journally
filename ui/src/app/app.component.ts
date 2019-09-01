import { Component, OnInit } from '@angular/core';
import { Entry } from './entries/entry';
import { EntryService } from './entries/entry.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  entry: Entry;

  constructor(private entryService: EntryService) {}

  ngOnInit() {
    this.entryService.getEntries().subscribe(entries => {
      this.entry = entries[0];
    });
  }
}

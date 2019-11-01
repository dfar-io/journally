import { Component, OnInit } from '@angular/core';
import { Entry } from './entries/entry';
import { EntryService } from './entries/entry.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginModalComponent } from './login-modal/login-modal.component';
import { AboutModalComponent } from './about-modal/about-modal.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  entry: Entry;

  constructor(
    private entryService: EntryService,
    private modalService: NgbModal
  ) {}

  ngOnInit() {
    // this.entryService.getEntries().subscribe(entries => {
    // this.entry = entries[0]; since it's not doing anything right now.
    // });
    const currentDate = new Date();
    this.entry = new Entry();
    this.entry.datetime = new Date();
    this.entry.content = null;
  }

  openLoginModal() {
    this.modalService.open(LoginModalComponent, {
      centered: true
    });
  }

  openAboutModal() {
    this.modalService.open(AboutModalComponent, {
      centered: true
    });
  }
}

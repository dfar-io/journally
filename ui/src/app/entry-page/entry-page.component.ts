import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AboutModalComponent } from '../about-modal/about-modal.component';
import { Alert } from '../alert';
import { Entry } from '../entries/entry';
import { EntryService } from '../entries/entry.service';
import { LoginModalComponent } from '../login-modal/login-modal.component';
import { User } from '../user/user';
import { UserService } from '../user/user.service';

@Component({
  selector: 'app-entry-page',
  templateUrl: './entry-page.component.html',
  styleUrls: ['./entry-page.component.css']
})
export class EntryPageComponent implements OnInit {
  entry: Entry;
  currentUser: User;
  isSaving: boolean;
  isNewEntry = true;
  hasTitle = false;

  constructor(
    private entryService: EntryService,
    private modalService: NgbModal,
    private userService: UserService,
    private route: ActivatedRoute
  ) {
    this.userService.currentUser.subscribe(x => (this.currentUser = x));
  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.entry = data.entry;
      this.isNewEntry = false;

      if (this.entry == null) {
        this.entry = new Entry();
        this.entry.datetime = new Date();
        this.entry.content = null;
        this.isNewEntry = true;
      }
    });
  }

  get entryWordCount() {
    if (this.entry.content == null) {
      return 0;
    }

    return this.entry.content.split(' ').length;
  }

  openAboutModal() {
    this.modalService.open(AboutModalComponent, {
      centered: true
    });
  }

  saveEntry() {
    if (!this.userService.isLoggedIn()) {
      const modalRef = this.modalService.open(LoginModalComponent, {
        centered: true
      });
      modalRef.componentInstance.modalAlert = new Alert(
        'warning',
        'You must be logged in to save entries.'
      );

      return;
    }

    this.isSaving = true;

    if (this.isNewEntry) {
      this.entryService.createEntry(this.entry).subscribe(response => {
        this.isSaving = false;
        this.entry.id = response.id;
        this.isNewEntry = false;
      });
    } else {
      this.entryService.updateEntry(this.entry).subscribe(() => {
        this.isSaving = false;
      });
    }
  }
}

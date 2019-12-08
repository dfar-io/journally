import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AboutModalComponent } from '../about-modal/about-modal.component';
import { Alert } from '../alert';
import { Entry, EntryResolved } from '../entries/entry';
import { EntryService } from '../entries/entry.service';
import { LoginModalComponent } from '../login-modal/login-modal.component';
import { ToastService } from '../shared/toast.service';
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
  isSaving = false;
  isNewEntry = true;
  errorMessage: string;

  constructor(
    private entryService: EntryService,
    private modalService: NgbModal,
    private userService: UserService,
    private route: ActivatedRoute,
    private toastService: ToastService
  ) {
    this.userService.currentUser.subscribe(x => (this.currentUser = x));
  }

  ngOnInit() {
    const resolvedEntry: EntryResolved = this.route.snapshot.data.resolvedEntry;
    if (resolvedEntry !== undefined && resolvedEntry.error) {
      this.toastService.show(
        'An unexpected error occurred while loading your entry.',
        {
          classname: 'bg-danger text-light',
          delay: 15000
        }
      );
    } else if (resolvedEntry !== undefined) {
      this.entry = resolvedEntry.entry;
    }

    if (this.entry == null) {
      this.entry = new Entry();
      this.entry.datetime = new Date();
      this.entry.content = null;
      this.isNewEntry = true;
    } else {
      this.isNewEntry = false;
    }
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
      this.entryService.createEntry(this.entry).subscribe(
        response => {
          this.isSaving = false;
          this.entry.id = response.id;
          this.isNewEntry = false;
          this.toastService.show('Entry saved.', {
            classname: 'bg-success text-light',
            delay: 5000
          });
        },
        () => {
          this.isSaving = false;
          this.toastService.show('Unexpected error while saving entry.', {
            classname: 'bg-danger text-light',
            delay: 15000
          });
        }
      );
    } else {
      this.entryService.updateEntry(this.entry).subscribe(
        () => {
          this.isSaving = false;
          this.toastService.show('Entry updated.', {
            classname: 'bg-success text-light',
            delay: 5000
          });
        },
        () => {
          this.isSaving = false;
          this.toastService.show('Unexpected error while updating entry.', {
            classname: 'bg-danger text-light',
            delay: 15000
          });
        }
      );
    }
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

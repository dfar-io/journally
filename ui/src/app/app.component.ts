import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AboutModalComponent } from './about-modal/about-modal.component';
import { Alert } from './alert';
import { Entry } from './entries/entry';
import { EntryService } from './entries/entry.service';
import { LoginModalComponent } from './login-modal/login-modal.component';
import { RegisterModalComponent } from './register-modal/register-modal.component';
import { User } from './user/user';
import { UserService } from './user/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  entry: Entry;
  currentUser: User;
  isSaving: boolean;
  isNewEntry = true;

  constructor(
    private entryService: EntryService,
    private modalService: NgbModal,
    private userService: UserService
  ) {
    this.userService.currentUser.subscribe(x => (this.currentUser = x));
  }

  ngOnInit() {
    this.entry = new Entry();
    this.entry.datetime = new Date();
    this.entry.content = null;
  }

  openLoginModal(alert: Alert = null) {
    const modalRef = this.modalService.open(LoginModalComponent, {
      centered: true
    });
    modalRef.componentInstance.modalAlert = alert;

    modalRef.result.then(result => {
      if (result === 'openRegisterModal') {
        this.openRegisterModal();
      }
    });
  }

  openRegisterModal() {
    const modalRef = this.modalService.open(RegisterModalComponent, {
      centered: true
    });

    modalRef.result.then(result => {
      if (result.event === 'openLoginModal') {
        this.openLoginModal(result.alert);
      }
    });
  }

  openAboutModal() {
    this.modalService.open(AboutModalComponent, {
      centered: true
    });
  }

  saveEntry() {
    if (!this.isUserLoggedIn()) {
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

  logout() {
    this.userService.logoutUser();
  }

  private isUserLoggedIn() {
    return this.currentUser != null;
  }
}

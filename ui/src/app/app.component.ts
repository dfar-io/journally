import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AboutModalComponent } from './about-modal/about-modal.component';
import { Alert } from './alert';
import { Entry } from './entries/entry';
import { EntryService } from './entries/entry.service';
import { LoginModalComponent } from './login-modal/login-modal.component';
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
    this.entryService
      .saveEntry(this.entry)
      .subscribe(() => (this.isSaving = false));
  }

  logout() {
    this.userService.logoutUser();
  }

  private isUserLoggedIn() {
    return this.currentUser != null;
  }
}

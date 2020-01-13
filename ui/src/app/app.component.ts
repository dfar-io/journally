import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AboutModalComponent } from './about-modal/about-modal.component';
import { Alert } from './alert';
import { LoginModalComponent } from './login-modal/login-modal.component';
import { RegisterModalComponent } from './register-modal/register-modal.component';
import { ToastService } from './shared/toast.service';
import { User } from './user/user';
import { UserService } from './user/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  currentUser: User;

  constructor(
    private modalService: NgbModal,
    private userService: UserService,
    private router: Router,
    private toastService: ToastService
  ) {
    this.userService.currentUser.subscribe(x => (this.currentUser = x));
  }

  ngOnInit() {
    if (this.currentUser != null) {
      this.userService.isTokenValid().subscribe(response => {
        if (!response) {
          this.logout('Session expired, please relogin.');
        }
      });
    }
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

  openEntries() {
    this.router.navigate(['/entries']);
  }

  openWriteEntry() {
    this.router.navigate(['']);
  }

  logout(text: string = 'Logged out.') {
    this.userService.logoutUser();
    this.toastService.show(text, { classname: 'text-dark' });
    this.router.navigate(['']);
  }
}

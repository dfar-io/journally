import { Component } from '@angular/core';
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
export class AppComponent {
  currentUser: User;

  constructor(
    private modalService: NgbModal,
    private userService: UserService,
    private router: Router,
    private toastService: ToastService
  ) {
    this.userService.currentUser.subscribe(x => (this.currentUser = x));
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

  logout() {
    this.userService.logoutUser();
    this.toastService.show('Logged out.', { classname: 'text-dark' });
    this.router.navigate(['']);
  }
}

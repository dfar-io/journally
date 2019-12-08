import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Alert } from '../alert';
import { ToastService } from '../shared/toast.service';
import { User } from '../user/user';
import { UserService } from '../user/user.service';

@Component({
  selector: 'app-login-modal',
  templateUrl: './login-modal.component.html',
  styleUrls: ['./login-modal.component.css']
})
export class LoginModalComponent implements OnInit {
  @Input() modalAlert: Alert;

  loginForm: FormGroup;
  isLoggingIn = false;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private userService: UserService,
    private toastService: ToastService
  ) {}

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      email: '',
      password: ''
    });
  }

  authenticateUser() {
    this.isLoggingIn = true;
    this.closeAlert();

    const user = new User();
    user.email = this.loginForm.value.email;
    user.password = this.loginForm.value.password;

    this.userService.authenticateUser(user).subscribe(
      () => {
        this.isLoggingIn = false;
        this.toastService.show('Logged in.', {
          classname: 'bg-success text-light'
        });
        this.activeModal.close();
      },
      () => {
        this.isLoggingIn = false;
        const errorMessage = 'An unexpected error has occurred.';
        this.modalAlert = new Alert('danger', errorMessage);
      }
    );
  }

  clickRegister() {
    this.activeModal.close('openRegisterModal');
  }

  closeAlert() {
    this.modalAlert = null;
  }

  isNullOrWhitespace(str: string): boolean {
    if (str === undefined) {
      return true;
    }

    return str === null || str.match(/^ *$/) !== null;
  }
}

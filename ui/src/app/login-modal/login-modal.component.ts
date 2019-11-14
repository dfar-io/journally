import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Alert } from '../alert';
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
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private userService: UserService
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
        this.activeModal.close();
      },
      errorResponse => {
        this.isLoggingIn = false;
        const errorMessage = errorResponse.error
          ? errorResponse.error
          : 'An unexpected error has occurred.';
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

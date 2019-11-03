import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';
import { RegisterModalComponent } from '../register-modal/register-modal.component';
import { User } from '../user/user';
import { UserService } from '../user/user.service';
import { Alert } from '../alert';

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

    const user = new User();
    user.email = this.loginForm.value.email;
    user.password = this.loginForm.value.password;

    this.userService.authenticateUser(user).subscribe(
      response => {
        this.isLoggingIn = false;
        this.activeModal.close();
      },
      errorResponse => {
        this.modalAlert = new Alert('danger', errorResponse.error);
        this.isLoggingIn = false;
      }
    );
  }

  // circular dependency introduced, need to remove this
  // check out
  // https://github.com/angular/components/issues/3593#issuecomment-286397115
  clickRegister() {
    this.activeModal.close();
    this.modalService.open(RegisterModalComponent, {
      centered: true
    });
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

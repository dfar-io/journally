import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LoginModalComponent } from '../login-modal/login-modal.component';
import { UserService } from '../user/user.service';
import { User } from '../user/user';
import { Alert } from '../alert';

@Component({
  selector: 'app-register-modal',
  templateUrl: './register-modal.component.html',
  styleUrls: ['./register-modal.component.css']
})
export class RegisterModalComponent implements OnInit {
  registerForm: FormGroup;
  isRegistering = false;
  modalMessage: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      email: '',
      password: '',
      verifyPassword: ''
    });
  }

  submitForm() {
    this.isRegistering = true;

    const user = new User();
    user.email = this.registerForm.value.email;
    user.password = this.registerForm.value.password;

    this.userService.registerUser(user).subscribe(
      response => {
        this.isRegistering = false;
        this.backToLogin(new Alert('success', `${response.email} registered.`));
      },
      errorResponse => {
        this.modalMessage = errorResponse.error;
        this.isRegistering = false;
      }
    );
  }

  // circular dependency introduced, need to remove this
  // check out
  // https://github.com/angular/components/issues/3593#issuecomment-286397115
  backToLogin(alert: Alert) {
    this.activeModal.close();
    const modalRef = this.modalService.open(LoginModalComponent, {
      centered: true
    });
    modalRef.componentInstance.modalAlert = alert;
  }

  closeAlert() {
    this.modalMessage = '';
  }

  isNullOrWhitespace(str: string): boolean {
    if (str === undefined) {
      return true;
    }

    return str === null || str.match(/^ *$/) !== null;
  }
}

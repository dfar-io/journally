import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Alert } from '../alert';
import { User } from '../user/user';
import { UserService } from '../user/user.service';

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

  backToLogin(alert: Alert = null) {
    this.activeModal.close({ event: 'openLoginModal', alert });
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

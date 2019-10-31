import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';
import { RegisterModalComponent } from '../register-modal/register-modal.component';

@Component({
  selector: 'app-login-modal',
  templateUrl: './login-modal.component.html',
  styleUrls: ['./login-modal.component.css']
})
export class LoginModalComponent implements OnInit {
  loginForm: FormGroup;
  isLoggingIn = false;

  constructor(
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      email: '',
      password: ''
    });
  }

  submitForm() {
    this.isLoggingIn = true;
    // simulates logging in
    setTimeout(() => {
      this.isLoggingIn = false;
    }, 2000);
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
}

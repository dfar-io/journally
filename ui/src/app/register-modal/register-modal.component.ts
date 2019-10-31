import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LoginModalComponent } from '../login-modal/login-modal.component';

@Component({
  selector: 'app-register-modal',
  templateUrl: './register-modal.component.html',
  styleUrls: ['./register-modal.component.css']
})
export class RegisterModalComponent implements OnInit {
  registerForm: FormGroup;
  isRegistering = false;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private modalService: NgbModal
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
    // simulates logging in
    setTimeout(() => {
      this.isRegistering = false;
    }, 2000);
  }

  // circular dependency introduced, need to remove this
  // check out
  // https://github.com/angular/components/issues/3593#issuecomment-286397115
  backToLogin() {
    this.activeModal.close();
    this.modalService.open(LoginModalComponent, {
      centered: true
    });
  }
}

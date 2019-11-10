import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { User } from 'src/app/user/user';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  @Input() currentUser: User;
  @Output() clickLogin = new EventEmitter();
  @Output() clickLogout = new EventEmitter();

  constructor() {}

  ngOnInit() {}

  onClickLogin() {
    this.clickLogin.emit();
  }

  onClickLogout() {
    this.clickLogout.emit();
  }
}

import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
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
  @Output() clickEntries = new EventEmitter();
  @Output() clickWriteEntry = new EventEmitter();

  constructor(private router: Router) {}

  ngOnInit() {}

  onClickLogin() {
    this.clickLogin.emit();
  }

  onClickLogout() {
    this.clickLogout.emit();
  }

  onClickEntries() {
    this.clickEntries.emit();
  }

  onClickWriteEntry() {
    this.clickWriteEntry.emit();
  }

  onWriteEntryPage() {
    return this.router.url === '/';
  }

  onEntriesPage() {
    return this.router.url === '/entries';
  }
}

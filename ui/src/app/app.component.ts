import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'blu-journal';
  date = new Date();

  displayDate() {
    return (
      this.date.getFullYear() +
      '.' +
      this.date.getMonth() +
      '.' +
      this.date.getDate()
    );
  }
}

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { NgbModalModule, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginModalComponent } from './login-modal/login-modal.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RegisterModalComponent } from './register-modal/register-modal.component';

@NgModule({
  declarations: [AppComponent, LoginModalComponent, RegisterModalComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModalModule,
    ReactiveFormsModule,
    FormsModule
  ],
  providers: [NgbActiveModal],
  bootstrap: [AppComponent],
  entryComponents: [LoginModalComponent, RegisterModalComponent]
})
export class AppModule {}

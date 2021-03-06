import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import {
  NgbActiveModal,
  NgbAlertModule,
  NgbModalModule
} from '@ng-bootstrap/ng-bootstrap';
import { AboutModalComponent } from './about-modal/about-modal.component';
import { AppComponent } from './app.component';
import { EntriesPageComponent } from './entries-page/entries-page.component';
import { EntryPageComponent } from './entry-page/entry-page.component';
import { LoginModalComponent } from './login-modal/login-modal.component';
import { RegisterModalComponent } from './register-modal/register-modal.component';
import { SharedModule } from './shared/shared.module';
import { JwtInterceptor } from './user/jwt.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    LoginModalComponent,
    RegisterModalComponent,
    AboutModalComponent,
    EntryPageComponent,
    EntriesPageComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    NgbModalModule,
    NgbAlertModule,
    ReactiveFormsModule,
    FormsModule,
    SharedModule
  ],
  providers: [
    NgbActiveModal,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent],
  entryComponents: [
    LoginModalComponent,
    RegisterModalComponent,
    AboutModalComponent
  ]
})
export class AppModule {}

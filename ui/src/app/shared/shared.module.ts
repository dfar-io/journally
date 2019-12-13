import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { NgbDropdownModule, NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from '../app-routing.module';
import { NavbarComponent } from './navbar/navbar.component';
import { TimeoutInterceptor } from './timeout-interceptor';
import { ToastsContainerComponent } from './toasts-container/toasts-container.component';

@NgModule({
  declarations: [NavbarComponent, ToastsContainerComponent],
  imports: [CommonModule, NgbDropdownModule, NgbToastModule, AppRoutingModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TimeoutInterceptor,
      multi: true
    }
  ],
  exports: [
    NavbarComponent,
    ToastsContainerComponent,
    NgbToastModule,
    AppRoutingModule
  ]
})
export class SharedModule {}

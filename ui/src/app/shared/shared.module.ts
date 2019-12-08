import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { NgbDropdownModule, NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { NavbarComponent } from './navbar/navbar.component';
import { TimeoutInterceptor } from './timeout-interceptor';
import { ToastsContainerComponent } from './toasts-container/toasts-container.component';

@NgModule({
  declarations: [NavbarComponent, ToastsContainerComponent],
  imports: [CommonModule, NgbDropdownModule, NgbToastModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TimeoutInterceptor,
      multi: true
    }
  ],
  exports: [NavbarComponent, ToastsContainerComponent, NgbToastModule]
})
export class SharedModule {}

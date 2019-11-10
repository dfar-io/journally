import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NavbarComponent } from './navbar/navbar.component';
import { TimeoutInterceptor } from './timeout-interceptor';

@NgModule({
  declarations: [NavbarComponent],
  imports: [CommonModule, NgbDropdownModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TimeoutInterceptor,
      multi: true
    }
  ],
  exports: [NavbarComponent]
})
export class SharedModule {}

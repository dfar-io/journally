import {
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError, timeout } from 'rxjs/operators';

@Injectable()
export class TimeoutInterceptor implements HttpInterceptor {
  public intercept(req: HttpRequest<any>, next: HttpHandler) {
    return next.handle(req).pipe(
      timeout(10000),
      catchError(e => {
        return throwError(e);
      })
    );
  }
}

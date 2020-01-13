import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { Observable } from 'rxjs/internal/Observable';
import { throwError } from 'rxjs/internal/observable/throwError';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from './user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;
  private apiUrl = `${environment.apiUrl}user/`;

  constructor(private httpClient: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(
      JSON.parse(localStorage.getItem('currentUser'))
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  registerUser(user: User): Observable<User> {
    return this.httpClient.post<User>(`${this.apiUrl}`, user);
  }

  authenticateUser(user: User): Observable<User> {
    return this.httpClient.post<User>(`${this.apiUrl}authenticate`, user).pipe(
      map(response => {
        if (response && response.token) {
          localStorage.setItem('currentUser', JSON.stringify(response));
          this.currentUserSubject.next(response);
        }

        return response;
      })
    );
  }

  isTokenValid(): Observable<boolean> {
    return this.httpClient
      .get(`${this.apiUrl}verifyToken`, {
        responseType: 'text',
        observe: 'response'
      })
      .pipe(
        map(response => {
          return response.status === 200;
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 401) {
            return of(false);
          }

          return throwError(error);
        })
      );
  }

  isLoggedIn() {
    return this.currentUserValue != null;
  }

  logoutUser() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}

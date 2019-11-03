import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { User } from './user';
import { environment } from 'src/environments/environment';
import { RegisterUserResponse } from './register-user-response';
import { AuthenticateUserResponse } from './authenticate-user-response';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private currentUserSubject: BehaviorSubject<AuthenticateUserResponse>;
  public currentUser: Observable<AuthenticateUserResponse>;
  private apiUrl = `${environment.apiUrl}user/`;

  constructor(private httpClient: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<AuthenticateUserResponse>(
      JSON.parse(localStorage.getItem('currentUser'))
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): AuthenticateUserResponse {
    return this.currentUserSubject.value;
  }

  registerUser(user: User): Observable<RegisterUserResponse> {
    return this.httpClient.post<RegisterUserResponse>(`${this.apiUrl}`, user);
  }

  authenticateUser(user: User): Observable<AuthenticateUserResponse> {
    return this.httpClient
      .post<AuthenticateUserResponse>(`${this.apiUrl}authenticate`, user)
      .pipe(
        map(response => {
          if (response && response.token) {
            localStorage.setItem('currentUser', JSON.stringify(response));
            this.currentUserSubject.next(response);
          }

          return response;
        })
      );
  }

  logoutUser() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}

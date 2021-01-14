import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from './../../environments/environment';
import { IAuthenticationResponse } from './../models/authentication-response';
import { ILoginParameters } from './../models/login-parameters';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  public baseURL = environment.apiURL;

  constructor(private httpClient: HttpClient) {}

  public login(loginParameters: ILoginParameters): Observable<void> {
    return this.httpClient
      .post<IAuthenticationResponse>(this.baseURL + 'users/login', loginParameters)
      .pipe(
        map((response) => {
          if (response) {
            localStorage.setItem('token', response.token);
          }
        })
      );
  }
}

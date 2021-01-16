import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt/lib/jwthelper.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from './../../environments/environment';
import { IAuthenticationResponse } from './../models/authentication-response';
import { ILoginParameters } from './../models/login-parameters';
import { IUser } from './../models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  public jwtHelper = new JwtHelperService();
  public decodedToken: any;

  constructor(private httpClient: HttpClient) {}

  public login(loginParameters: ILoginParameters): Observable<void> {
    return this.httpClient
      .post<IAuthenticationResponse>(environment.apiURL + 'users/login', loginParameters)
      .pipe(
        map((response) => {
          if (response) {
            localStorage.setItem('token', response.token);
            this.decodedToken = this.jwtHelper.decodeToken(response.token);
            console.log(this.decodedToken);
          }
        })
      );
  }

  public isLoggedIn(): boolean {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  public register(registerParameters: any): Observable<IUser> {
    return this.httpClient.post<IUser>(environment.apiURL + 'users/register', registerParameters);
  }
}

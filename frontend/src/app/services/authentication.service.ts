import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from './../../environments/environment';
import { IAuthenticationResponse } from './../models/authentication-response';
import { ILoginParameters } from './../models/login-parameters';
import { IRegisterParameter } from './../models/register-parameters';
import { IUser } from './../models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  public jwtHelper = new JwtHelperService();
  public decodedToken: any;
  public currentUser: IUser;
  public genericPhotoURL = new BehaviorSubject<string>('../../assets/user-default.png');
  public currentPhotoURL = this.genericPhotoURL.asObservable();

  constructor(private httpClient: HttpClient) {}

  public login(loginParameters: ILoginParameters): Observable<void> {
    return this.httpClient
      .post<IAuthenticationResponse>(environment.apiURL + 'authentication/login', loginParameters)
      .pipe(
        map((response) => {
          if (response) {
            localStorage.setItem('token', response.token);
            localStorage.setItem('user', JSON.stringify(response.user));
            this.decodedToken = this.jwtHelper.decodeToken(response.token);
            console.log(this.decodedToken);
            this.currentUser = response.user;
            this.changeUserPhoto(this.currentUser.mainPhotoURL);
          }
        })
      );
  }

  public changeUserPhoto(photoURL: string): void {
    this.genericPhotoURL.next(photoURL);
  }

  public isLoggedIn(): boolean {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  public register(registerParameters: IRegisterParameter): Observable<IUser> {
    return this.httpClient.post<IUser>(
      environment.apiURL + 'authentication/register',
      registerParameters
    );
  }
}

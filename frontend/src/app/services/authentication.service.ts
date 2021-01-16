import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from './../../environments/environment';
import { IAuthenticationResponse } from './../models/authentication-response';
import { ILoginParameters } from './../models/login-parameters';
import { IUser } from './../models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  constructor(private httpClient: HttpClient) {}

  public login(loginParameters: ILoginParameters): Observable<IAuthenticationResponse> {
    return this.httpClient.post<IAuthenticationResponse>(
      environment.apiURL + 'users/login',
      loginParameters
    );
  }

  public register(registerParameters: any): Observable<IUser> {
    return this.httpClient.post<IUser>(environment.apiURL + 'users/register', registerParameters);
  }
}

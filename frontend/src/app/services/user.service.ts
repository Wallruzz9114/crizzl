import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from './../../environments/environment';
import { IUser } from './../models/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private httpClient: HttpClient) {}

  public getAll(): Observable<IUser[]> {
    return this.httpClient.get<IUser[]>(environment.apiURL + 'users');
  }

  public get(id: number): Observable<IUser> {
    return this.httpClient.get<IUser>(environment.apiURL + `/users/${id}`);
  }
}

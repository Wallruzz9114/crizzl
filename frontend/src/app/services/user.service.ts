import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { IUpdateParameters } from '../models/update-parameters';
import { environment } from './../../environments/environment';
import { PaginatedResult } from './../models/paginated-result';
import { IUser } from './../models/user';
import { IUserFilters } from './../models/user-filters';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private httpClient: HttpClient) {}

  public getAll(
    pageNumber?: string,
    itemsPerPage?: string,
    userFilters?: IUserFilters
  ): Observable<PaginatedResult<IUser[]>> {
    const paginatedResult: PaginatedResult<IUser[]> = new PaginatedResult<IUser[]>();
    let params = new HttpParams();

    if (pageNumber != null && itemsPerPage != null) {
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', itemsPerPage);
    }

    if (userFilters != null) {
      params = params.append('minAge', userFilters.minAge);
      params = params.append('maxAge', userFilters.maxAge);
      params = params.append('gender', userFilters.gender);
      params = params.append('orderBy', userFilters.orderBy);
    }

    return this.httpClient
      .get<IUser[]>(environment.apiURL + 'users', { observe: 'response', params })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;

          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }

          return paginatedResult;
        })
      );
  }

  public get(id: number): Observable<IUser> {
    return this.httpClient.get<IUser>(environment.apiURL + `/users/${id}`);
  }

  public updateUser(updateParameters: IUpdateParameters): Observable<void> {
    return this.httpClient.put<void>(environment.apiURL + 'users/update', updateParameters);
  }

  public setMainPhoto(id: number): Observable<void> {
    return this.httpClient.post<void>(environment.apiURL + `photos/setasmain${id}`, {});
  }

  public deletePhoto(id: number): Observable<void> {
    return this.httpClient.delete<void>(environment.apiURL + `photos/delete${id}`);
  }
}

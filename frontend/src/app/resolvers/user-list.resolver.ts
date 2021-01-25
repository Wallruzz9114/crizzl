import { Injectable } from '@angular/core';
import { Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IUser } from './../models/user';
import { AlertifyService } from './../services/alertify.service';
import { UserService } from './../services/user.service';

@Injectable()
export class UserListResolver implements Resolve<IUser[]> {
  public pageNumber = 1;
  public itemsPerPage = 5;

  constructor(
    private userService: UserService,
    private router: Router,
    private alertifyService: AlertifyService
  ) {}

  public resolve(): Observable<IUser[]> {
    return this.userService.getAll(this.pageNumber.toString(), this.itemsPerPage.toString()).pipe(
      catchError((error) => {
        console.log(error);
        this.alertifyService.error('Problem retrieving data');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}

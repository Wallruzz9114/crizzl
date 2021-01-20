import { Injectable } from '@angular/core';
import { Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IUser } from './../models/user';
import { AlertifyService } from './../services/alertify.service';
import { AuthenticationService } from './../services/authentication.service';
import { UserService } from './../services/user.service';

@Injectable()
export class UserEditResolver implements Resolve<IUser> {
  constructor(
    private userService: UserService,
    private authenticationService: AuthenticationService,
    private router: Router,
    private alertifyService: AlertifyService
  ) {}

  public resolve(): Observable<IUser> {
    return this.userService.get(this.authenticationService.decodedToken.nameid).pipe(
      catchError((error) => {
        console.log(error);
        this.alertifyService.error('Problem retrieving your data');
        this.router.navigate(['/members']);
        return of(null);
      })
    );
  }
}

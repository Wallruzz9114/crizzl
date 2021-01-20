import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IUser } from './../models/user';
import { AlertifyService } from './../services/alertify.service';
import { UserService } from './../services/user.service';

@Injectable()
export class UserDetailsResolver implements Resolve<IUser> {
  constructor(
    private userService: UserService,
    private router: Router,
    private alertifyService: AlertifyService
  ) {}

  public resolve(route: ActivatedRouteSnapshot): Observable<IUser> {
    return this.userService.get(route.params['id']).pipe(
      catchError((error) => {
        console.log(error);
        this.alertifyService.error('Problem retrieving data');
        this.router.navigate(['/members']);
        return of(null);
      })
    );
  }
}

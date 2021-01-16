import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AlertifyService } from './../services/alertify.service';
import { AuthenticationService } from './../services/authentication.service';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationGuard implements CanActivate {
  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private alertifyService: AlertifyService
  ) {}

  canActivate(): boolean {
    if (!this.authenticationService.isLoggedIn()) {
      return true;
    }

    this.alertifyService.error('Unauthorized access');
    this.router.navigate(['/home']);

    return false;
  }
}

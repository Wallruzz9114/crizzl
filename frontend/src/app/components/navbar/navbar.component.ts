import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt/lib/jwthelper.service';
import { BsDropdownConfig } from 'ngx-bootstrap/dropdown';
import { AlertifyService } from './../../services/alertify.service';
import { AuthenticationService } from './../../services/authentication.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  providers: [{ provide: BsDropdownConfig, useValue: { isAnimated: true, autoClose: true } }],
})
export class NavbarComponent implements OnInit {
  public loginParameters: any = {};
  public jwtHelper = new JwtHelperService();
  public decodedToken: string;

  constructor(
    public authenticationService: AuthenticationService,
    private alertifyService: AlertifyService
  ) {}

  ngOnInit(): void {}

  public login(): void {
    this.authenticationService.login(this.loginParameters).subscribe(
      () => this.alertifyService.success('Logged in successfully'),
      () => this.alertifyService.error('Failed to login')
    );
  }

  public isLoggedIn(): boolean {
    return this.authenticationService.isLoggedIn();
  }

  public logout(): void {
    localStorage.removeItem('token');
    this.alertifyService.message('Logged out');
  }
}

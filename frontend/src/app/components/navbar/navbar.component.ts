import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
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
  public photoURL: string;

  constructor(
    public authenticationService: AuthenticationService,
    private alertifyService: AlertifyService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authenticationService.currentPhotoURL.subscribe((photoURL) => (this.photoURL = photoURL));
  }

  public login(): void {
    this.authenticationService.login(this.loginParameters).subscribe(
      (next) => {
        this.alertifyService.success('Logged in successfully');
        console.log(next);
      },
      (error) => {
        this.alertifyService.error('Failed to login');
        console.log(error);
      },
      () => this.router.navigate(['/members'])
    );
  }

  public isLoggedIn(): boolean {
    return this.authenticationService.isLoggedIn();
  }

  public logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authenticationService.decodedToken = null;
    this.authenticationService.currentUser = null;
    this.alertifyService.message('Logged out');
    this.router.navigate(['/home']);
  }
}

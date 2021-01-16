import { Component, OnInit } from '@angular/core';
import { AlertifyService } from './../../services/alertify.service';
import { AuthenticationService } from './../../services/authentication.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {
  public loginParameters: any = {};

  constructor(
    private authenticationService: AuthenticationService,
    private alertifyService: AlertifyService
  ) {}

  ngOnInit(): void {}

  public login(): void {
    this.authenticationService.login(this.loginParameters).subscribe(
      (response) => {
        if (response) {
          localStorage.setItem('token', response.token);
          this.alertifyService.success('Logged in successfully');
        }
      },
      () => this.alertifyService.error('Failed to login')
    );
  }

  public isLoggedIn(): boolean {
    const token = localStorage.getItem('token');
    return !!token;
  }

  public logout(): void {
    localStorage.removeItem('token');
    this.alertifyService.message('Logged out');
  }
}

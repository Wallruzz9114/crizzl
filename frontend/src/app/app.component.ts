import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt/lib/jwthelper.service';
import { AuthenticationService } from './services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  public title = 'Crizzl';
  public jwtHelper = new JwtHelperService();

  constructor(private authenticationService: AuthenticationService) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');

    if (token) {
      this.authenticationService.decodedToken = this.jwtHelper.decodeToken(token);
    }
  }
}

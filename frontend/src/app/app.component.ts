import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from './../environments/environment';
import { IUser } from './models/user';
import { AuthenticationService } from './services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  public title = environment.appTitle;
  public jwtHelper = new JwtHelperService();

  constructor(private authenticationService: AuthenticationService) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    const user: IUser = JSON.parse(localStorage.getItem('user'));

    if (token) {
      this.authenticationService.decodedToken = this.jwtHelper.decodeToken(token);
    }

    if (user) {
      this.authenticationService.currentUser = user;
      this.authenticationService.changeUserPhoto(user.mainPhotoURL);
    }
  }
}

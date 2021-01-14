import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './../../services/authentication.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {
  public loginParameters: any = {};

  constructor(private authenticationService: AuthenticationService) {}

  ngOnInit(): void {}

  public login() {
    this.authenticationService.login(this.loginParameters).subscribe(
      // eslint-disable-next-line @typescript-eslint/no-unused-vars
      (_) => console.log('Logged in successfully!'),
      (error) => console.log(`Failed to login ${error}`)
    );
  }
}

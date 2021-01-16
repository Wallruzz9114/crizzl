import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { IUser } from './../../models/user';
import { AlertifyService } from './../../services/alertify.service';
import { AuthenticationService } from './../../services/authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();

  public newUserParameters: any = {};
  public user: IUser;

  constructor(
    private authenticationService: AuthenticationService,
    private alertifyService: AlertifyService
  ) {}

  ngOnInit(): void {}

  public register(): void {
    this.authenticationService.register(this.newUserParameters).subscribe(
      (user) => {
        if (user) {
          this.user = user;
          this.alertifyService.success('Registration successful');
        }
      },
      (error) => this.alertifyService.error(`${error}`)
    );
  }

  public cancel(): void {
    this.cancelRegister.emit(false);
  }
}

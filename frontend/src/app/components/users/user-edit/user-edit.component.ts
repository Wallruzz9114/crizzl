import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { IUser } from './../../../models/user';
import { AlertifyService } from './../../../services/alertify.service';
import { AuthenticationService } from './../../../services/authentication.service';
import { UserService } from './../../../services/user.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.scss'],
})
export class UserEditComponent implements OnInit {
  @ViewChild('editForm', { static: true }) editForm: NgForm;
  public user: IUser;

  @HostListener('window:beforeunload', ['$event'])
  public unloadNotification($event: Event): void {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private activatedRoute: ActivatedRoute,
    private alertifyService: AlertifyService,
    private userService: UserService,
    private authenticationService: AuthenticationService
  ) {
    this.activatedRoute.data.subscribe((data) => {
      this.user = data['user'];
    });
  }

  ngOnInit(): void {}

  public updateUser(): void {
    const params = {
      username: this.authenticationService.decodedToken.nameid,
      bio: this.user.bio,
      datingTarget: this.user.datingTarget,
      interests: this.user.interests,
      city: this.user.city,
      country: this.user.country,
    };

    this.userService.updateUser(params).subscribe(
      (next) => {
        console.log(next);
        this.alertifyService.success('Profile updated sucessfully');
        this.editForm.reset(this.user);
      },
      (error) => {
        this.alertifyService.error(error);
      }
    );
  }
}

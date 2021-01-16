import { Component, OnInit } from '@angular/core';
import { IUser } from './../../../models/user';
import { AlertifyService } from './../../../services/alertify.service';
import { UserService } from './../../../services/user.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.scss'],
})
export class MemberListComponent implements OnInit {
  public users: IUser[];

  constructor(private userService: UserService, private alertifyService: AlertifyService) {}

  ngOnInit(): void {}

  public getUsers(): void {
    this.userService.getAll().subscribe(
      (users) => (this.users = users),
      (error) => this.alertifyService.error(error)
    );
  }
}

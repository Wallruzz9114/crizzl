import { Component, Input, OnInit } from '@angular/core';
import { IUser } from './../../../models/user';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.scss'],
})
export class MemberCardComponent implements OnInit {
  @Input() user: IUser;

  constructor() {}

  ngOnInit(): void {}
}

import { Component, OnInit } from '@angular/core';
import { environment } from './../../../environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  public registerMode = false;
  public apiURL = environment.apiURL;

  constructor() {}

  ngOnInit(): void {}

  public registerToggle(): void {
    this.registerMode = true;
  }

  public cancelRegisterMode(mode: boolean): void {
    this.registerMode = mode;
  }
}

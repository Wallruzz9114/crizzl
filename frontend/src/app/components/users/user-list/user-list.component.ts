import { Options } from '@angular-slider/ngx-slider';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { IUser } from '../../../models/user';
import { AlertifyService } from '../../../services/alertify.service';
import { UserService } from '../../../services/user.service';
import { PaginatedResult } from './../../../models/paginated-result';
import { Pagination } from './../../../models/pagination';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit {
  public users: IUser[];
  public user: IUser = JSON.parse(localStorage.getItem('user'));
  public genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' },
  ];
  public userFilters: any = {};
  public pagination: Pagination;
  public options: Options = {
    ceil: 100,
    floor: 18,
    showSelectionBar: true,
    selectionBarGradient: {
      from: 'white',
      to: '#FC0',
    },
  };

  constructor(
    private userService: UserService,
    private alertifyService: AlertifyService,
    private activatedRoute: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this.activatedRoute.data.subscribe((data) => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
      this.userFilters.orderBy = 'lastActive';
    });

    this.userFilters.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userFilters.minAge = 18;
    this.userFilters.maxAge = 100;
  }

  public resetFilters(): void {
    this.userFilters.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userFilters.minAge = 18;
    this.userFilters.maxAge = 100;
    this.getUsers();
  }

  public pageChanged(event: PageChangedEvent): void {
    this.pagination.currentPage = event.page;
  }

  public getUsers(): void {
    this.userService
      .getAll(
        this.pagination.currentPage.toString(),
        this.pagination.itemsPerPage.toString(),
        this.userFilters
      )
      .subscribe(
        (response: PaginatedResult<IUser[]>) => {
          this.users = response.result;
          this.pagination = response.pagination;
        },
        (error) => this.alertifyService.error(error)
      );
  }
}

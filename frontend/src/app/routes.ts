import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ListsComponent } from './components/lists/lists.component';
import { MessagesComponent } from './components/messages/messages.component';
import { UserDetailsComponent } from './components/users/user-details/user-details.component';
import { UserEditComponent } from './components/users/user-edit/user-edit.component';
import { UserListComponent } from './components/users/user-list/user-list.component';
import { AuthenticationGuard } from './guards/authentication.guard';
import { PreventUnsavedChanges } from './guards/prevent-unsaved-changes.guard';
import { UserDetailsResolver } from './resolvers/user-details.resolver';
import { UserEditResolver } from './resolvers/user-edit.resolver';
import { UserListResolver } from './resolvers/user-list.resolver';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthenticationGuard],
    children: [
      { path: 'users', component: UserListComponent, resolve: { users: UserListResolver } },
      {
        path: 'users/:id',
        component: UserDetailsComponent,
        resolve: { user: UserDetailsResolver },
      },
      {
        path: 'user/edit',
        component: UserEditComponent,
        resolve: { user: UserEditResolver },
        canDeactivate: [PreventUnsavedChanges],
      },
      { path: 'messages', component: MessagesComponent },
      { path: 'lists', component: ListsComponent },
    ],
  },
  { path: '**', redirectTo: '', pathMatch: 'full' },
];

import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { ListsComponent } from './components/lists/lists.component';
import { MessagesComponent } from './components/messages/messages.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { RegisterComponent } from './components/register/register.component';
import { PhotoEditorComponent } from './components/users/photo-editor/photo-editor.component';
import { UserCardComponent } from './components/users/user-card/user-card.component';
import { UserDetailsComponent } from './components/users/user-details/user-details.component';
import { UserEditComponent } from './components/users/user-edit/user-edit.component';
import { UserListComponent } from './components/users/user-list/user-list.component';
import { AuthenticationGuard } from './guards/authentication.guard';
import { PreventUnsavedChanges } from './guards/prevent-unsaved-changes.guard';
import { ErrorInterceptorProvider } from './interceptors/error.interceptor';
import { UserDetailsResolver } from './resolvers/user-details.resolver';
import { UserEditResolver } from './resolvers/user-edit.resolver';
import { UserListResolver } from './resolvers/user-list.resolver';
import { routes } from './routes';
import { AlertifyService } from './services/alertify.service';
import { AuthenticationService } from './services/authentication.service';
import { UserService } from './services/user.service';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    RegisterComponent,
    UserListComponent,
    UserCardComponent,
    MessagesComponent,
    ListsComponent,
    UserDetailsComponent,
    UserEditComponent,
    PhotoEditorComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    NgxGalleryModule,
    FileUploadModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    RouterModule.forRoot(routes),
    JwtModule.forRoot({
      config: {
        tokenGetter: () => localStorage.getItem('token'),
        allowedDomains: ['localhost:5000'],
        disallowedRoutes: ['http://localhost:5000/api/authentication'],
      },
    }),
  ],
  providers: [
    ErrorInterceptorProvider,
    AlertifyService,
    AuthenticationService,
    AuthenticationGuard,
    PreventUnsavedChanges,
    UserService,
    UserDetailsResolver,
    UserListResolver,
    UserEditResolver,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}

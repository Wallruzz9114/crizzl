import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { RegisterComponent } from './components/register/register.component';
import { ErrorInterceptorProvider } from './interceptors/error.interceptor';
import { AlertifyService } from './services/alertify.service';

@NgModule({
  declarations: [AppComponent, NavbarComponent, HomeComponent, RegisterComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    BsDropdownModule,
  ],
  providers: [ErrorInterceptorProvider, AlertifyService],
  bootstrap: [AppComponent],
})
export class AppModule {}

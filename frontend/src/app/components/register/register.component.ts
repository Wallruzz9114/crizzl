import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { IRegisterParameter } from './../../models/register-parameters';
import { AlertifyService } from './../../services/alertify.service';
import { AuthenticationService } from './../../services/authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();

  public registerParameters: IRegisterParameter;
  public registerForm: FormGroup;
  public bsConfig: Partial<BsDatepickerConfig>;

  constructor(
    private authenticationService: AuthenticationService,
    private alertifyService: AlertifyService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    (this.bsConfig = {
      containerClass: 'theme-orange',
      isAnimated: true,
    }),
      this.initializeForm();
  }

  public initializeForm(): void {
    this.registerForm = this.formBuilder.group(
      {
        username: ['', Validators.required],
        email: [
          '',
          Validators.required,
          Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$'),
        ],
        gender: ['male'],
        alias: ['', Validators.required],
        dateOfBirth: [null, Validators.required],
        city: ['', Validators.required],
        country: ['', Validators.required],
        password: ['', Validators.required, Validators.minLength(6)],
        confirmPassword: ['', Validators.required],
      },
      { validator: this.passwordsAreMatching }
    );
  }

  public passwordsAreMatching(
    formGroup: FormGroup
  ): {
    mismatch: boolean;
  } {
    return formGroup.get('password').value === formGroup.get('confirmPassword').value
      ? { mismatch: false }
      : { mismatch: true };
  }

  public register(): void {
    if (this.registerForm.valid) {
      this.registerParameters = Object.assign({}, this.registerForm.value);

      console.log(this.registerForm.value);
      console.log(this.registerParameters);

      if (this.registerParameters != null) {
        this.authenticationService.register(this.registerParameters).subscribe(
          () => this.alertifyService.success('Registration Successful'),
          (error) => console.log(error),
          () => {
            this.authenticationService
              .login({
                username: this.registerParameters.usermame,
                password: this.registerParameters.password,
              })
              .subscribe(() => this.router.navigate(['/users']));
          }
        );
      }
    }
  }

  public cancel(): void {
    this.cancelRegister.emit(false);
  }
}

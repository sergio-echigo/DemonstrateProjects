import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  constructor(private formBuilder : FormBuilder,
              private authService : AuthService,
              private router : Router) { }

  ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      username: ["", [Validators.required, Validators.pattern('^[A-Za-zñÑáéíóúÁÉÍÓÚ][A-Za-z0-9-_ñÑáéíóúÁÉÍÓÚ]+[A-Za-z0-9ñÑáéíóúÁÉÍÓÚ]$')]],
      email: ["", [Validators.required, Validators.email]],
      password: ["", [Validators.required, Validators.minLength(6)]]
      }
    );
  }

  private signup() {
    this.authService.signUp(this.formGroup?.value).subscribe({
      next: () => {
        this.router.navigate(['signin']);
      },
      error: () => {
        (document.getElementById('signUpRequestErrorMsg') as HTMLSpanElement).innerText = "Error! Try another set of information!";
      }
    })
  }

  submitSignUpForm() {
    this.formValid = this.formGroup?.valid;
    if (this.formValid)
      this.signup();
  }

  formGroup? : FormGroup;
  formValid? : boolean;
}
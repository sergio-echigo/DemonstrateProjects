import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signin',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {

  constructor(private formBuilder : FormBuilder,
              private authService : AuthService) { }

  ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      main: ["", [Validators.required]],
      password: ["", [Validators.required]]
    })
  }

  signIn() {
    this.authService.signin(this.formGroup?.value).subscribe({
      next: (x) => {
        console.log(x);
        /* Maybe Behavior.Set(IsAuthorized = true) */
      },
      error: () => {
        (document.getElementById('signInErrorMsg') as HTMLSpanElement).innerText = "Incorrect username, email or password."
      }
    })
  }

  formGroup? : FormGroup;
}
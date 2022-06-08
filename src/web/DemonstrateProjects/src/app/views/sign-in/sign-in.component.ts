import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-signin',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {

  constructor(private formBuilder : FormBuilder) { }

  ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      main: ["", [Validators.required]],
      password: ["", [Validators.required]]
    })
  }

  formGroup? : FormGroup;
}
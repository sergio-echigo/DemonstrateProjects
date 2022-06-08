import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-signup',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  constructor(private formBuilder : FormBuilder) { }

  ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      username: ["", [Validators.required, Validators.minLength(6), Validators.maxLength(50), Validators.pattern('^[A-Za-z0-9_-ñÑáéíóúÁÉÍÓÚ]$')]],
      email: ["", [Validators.required, Validators.email]],
      password: ["", [Validators.required, Validators.minLength(6)]]
      }
    );
  }

  formGroup? : FormGroup;

}
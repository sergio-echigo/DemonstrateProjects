import { Component, OnInit } from '@angular/core';
import { apiUrl } from 'src/app';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  private readonly apiUrl = apiUrl + "projects/key";

  sampleCodeRequest = `
  // Without index parameter.
  fetch('${this.apiUrl}', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'
    },
    body: {
        key: "YOUR_VALID_KEY_HERE"
    }
  });

  // With index parameter.
  fetch('${this.apiUrl}?index=0', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'
    },
    body: {
        key: "YOUR_VALID_KEY_HERE"
    }
  });
  `
}

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticatedBehaviorSubject } from 'src/app/behaviorSubject/AuthenticatedBehaviorSubject';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(private authBehavior : AuthenticatedBehaviorSubject,
              private authService : AuthService,
              private router : Router) { }

  ngOnInit(): void {
    this.authBehavior.isAuthenticated.subscribe(x => {
      this.isAuthenticated = x;
    });
  }

  logout() {
    this.authService.logout().subscribe({
      next: () => {
        this.authBehavior.isAuthenticated.next(false);
        this.router.navigate(['']);
      },
      error: () => {
        
      }
    })
  }

  isAuthenticated? : boolean;
}

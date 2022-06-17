import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticatedBehaviorSubject } from 'src/app/behaviorSubject/AuthenticatedBehaviorSubject';
import { Account } from 'src/app/models/account';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

  constructor(private authService : AuthService,
              private authBehavior: AuthenticatedBehaviorSubject,
              private router : Router) { }

  ngOnInit(): void {
    this.authService.getAccount().subscribe({
      next: (x) => {
        this.account = x;
      },
      error: () => {

      }
    });
  }

  deleteAccountRequest() : void {
    let pswd = prompt("Please, input your password.");
    if (pswd)
      this.deleteAccount(pswd);
  }

  private deleteAccount(pswd : string) : void {
    this.authService.deleteAccount(pswd).subscribe({
      next: () => {
        alert("Successfully deleted account! Thanks for using our app!");
        this.authBehavior.isAuthenticated.next(false);
        this.router.navigate(['/']);
      },
      error: () => {
        alert("Invalid credentials or server error.");
      }
    });
  }

  account? : Account;
}

import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { AuthService } from "../services/auth.service";

@Injectable({
    providedIn: 'root'
})

export class AuthenticatedBehaviorSubject {
    constructor(private authService : AuthService) {
        
    }

    public isAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.authService.isAuthenticated());
}
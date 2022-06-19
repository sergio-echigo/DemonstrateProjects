import { HttpHeaders, HTTP_INTERCEPTORS } from "@angular/common/http";
import { AuthInterceptor } from "./interceptors/auth-intercerptor";

// Change this if your api will run in another port or url!
export const apiUrl = 'https://localhost:7153/';

export const httpInterceptorsProviders = [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
]

export const withoutBodyHttpOptions = {
    headers: new HttpHeaders({'Content-Type': 'application/json'}),
    withCredentials: true
}

export const withBodyHttpOptions = {
    withCredentials: true
}
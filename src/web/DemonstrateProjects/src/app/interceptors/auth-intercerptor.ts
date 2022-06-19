import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthService } from "../services/auth.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private authService : AuthService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (this.authService.isExpired()) {
            if (!this.isAlreadyRefreshing) {
                this.isAlreadyRefreshing = true;
                this.authService.refresh().subscribe({
                    next: () => {
                        this.isAlreadyRefreshing = false;
                    },
                    error: () => {

                    }
                });
            }
        }

        let request : HttpRequest<any> = req;
        if (req.url == "https://localhost:7153/projects/key") {
            return next.handle(request).pipe();
        }

        let token = this.authService.getDaToken();
        if (token) {
            request = req.clone({
                headers: req.headers.set('Authorization', 'Bearer ' + token)
            });
        }

        return next.handle(request).pipe();
    }

    private isAlreadyRefreshing? : boolean;
}
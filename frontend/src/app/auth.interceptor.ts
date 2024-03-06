import { Injectable } from '@angular/core'
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http'
import { Observable, catchError, throwError } from 'rxjs'
import { AuthService } from './services/auth.service'

@Injectable()
export class authInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const authToken = this.authService.getToken()

    const authReq = authToken
      ? req.clone({
          headers: req.headers.set('Authorization', `Bearer ${authToken}`)
        })
      : req

    return next.handle(authReq).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err.status === 401) this.authService.logout()
        return throwError(() => new Error(err.message))
      })
    )
  }
}

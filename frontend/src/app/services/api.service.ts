import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable, catchError, throwError } from 'rxjs'
import { LoadingService } from './loading.service'

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  apiUrl = 'http://localhost:5149'

  constructor(
    private loadingService: LoadingService,
    private http: HttpClient
  ) {}

  getTokenData(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/token`).pipe(
      catchError(err => {
        return throwError(() => err)
      })
    )
  }

  updateTokenData(): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/token`, {}).pipe(
      catchError(err => {
        this.loadingService.hide()
        return throwError(() => err)
      })
    )
  }

  login(data: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { data }).pipe(
      catchError(err => {
        this.loadingService.hide()
        return throwError(() => err)
      })
    )
  }
}

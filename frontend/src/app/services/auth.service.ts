import { Injectable, Inject, PLATFORM_ID } from '@angular/core'
import { isPlatformBrowser } from '@angular/common'
import { Router } from '@angular/router'

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  protected AUTH_TOKEN = 'authToken'

  constructor(
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  login(token: string): void {
    if (!isPlatformBrowser(this.platformId)) return
    localStorage.setItem(this.AUTH_TOKEN, token)
    this.router.navigate(['/update'])
  }

  getToken(): string | null {
    if (!isPlatformBrowser(this.platformId)) return null
    return localStorage.getItem(this.AUTH_TOKEN)
  }

  logout(): void {
    localStorage.removeItem(this.AUTH_TOKEN)
    this.redirectToLogin()
  }

  redirectToLogin(): void {
    this.router.navigate(['/login'])
  }
}

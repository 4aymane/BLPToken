import { CanActivateFn } from '@angular/router'
import { AuthService } from './services/auth.service'
import { inject } from '@angular/core'

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService)

  const authToken = authService.getToken()
  if (!!authToken) return true

  authService.redirectToLogin()
  return false
}

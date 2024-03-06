import { Routes } from '@angular/router'
import { LoginComponent } from './pages/login/login.component'
import { InfoComponent } from './pages/info/info.component'
import { UpdateComponent } from './pages/update/update.component'
import { LandingComponent } from './pages/landing/landing.component'
import { authGuard } from './auth.guard'

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'login', component: LoginComponent },
  { path: 'info', component: InfoComponent },
  {
    path: 'update',
    component: UpdateComponent,
    canActivate: [authGuard]
  }
]

import { Component } from '@angular/core'
import { FormsModule } from '@angular/forms'
import { TokenService } from '../../services/api.service'
import { LoadingService } from '../../services/loading.service'
import { AuthService } from '../../services/auth.service'

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  login: { username: string; password: string } = { username: '', password: '' }

  constructor(
    private loadingService: LoadingService,
    private tokenService: TokenService,
    private authService: AuthService
  ) {}

  onSubmit() {
    if (!this.login.username || !this.login.password) return

    this.loadingService.show()
    this.tokenService.login(this.login).subscribe((data: any) => {
      if (!data.token) return
      this.authService.login(data.token)
      this.loadingService.hide()
    })
  }
}

import { Component } from '@angular/core'
import { TokenService } from '../../services/api.service'
import { LoadingService } from '../../services/loading.service'
import { Router } from '@angular/router'

@Component({
  selector: 'app-update',
  standalone: true,
  imports: [],
  templateUrl: './update.component.html',
  styleUrl: './update.component.scss'
})
export class UpdateComponent {
  constructor(
    private loadingService: LoadingService,
    private tokenService: TokenService,
    private router: Router
  ) {}

  update() {
    this.loadingService.show()

    this.tokenService.updateTokenData().subscribe(() => {
      this.loadingService.hide()
      this.router.navigate(['/info'])
    })
  }
}

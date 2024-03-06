import { Component } from '@angular/core'
import { TokenService } from '../../services/api.service'
import { CommonModule } from '@angular/common'

@Component({
  selector: 'app-info',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './info.component.html',
  styleUrl: './info.component.scss'
})
export class InfoComponent {
  token: any

  constructor(private tokenService: TokenService) {}

  ngOnInit(): void {
    this.tokenService.getTokenData().subscribe(data => {
      this.token = data
    })
  }
}

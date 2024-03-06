import { Component } from '@angular/core'
import { RouterOutlet } from '@angular/router'
import { NavbarComponent } from './components/navbar/navbar.component'
import { CommonModule } from '@angular/common'
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'
import { LoadingService } from './services/loading.service'

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    MatProgressSpinnerModule,
    RouterOutlet,
    CommonModule,
    NavbarComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'BLP'
  isLoading = false

  constructor(public loadingService: LoadingService) {}
}

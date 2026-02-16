import {Component, inject, signal} from '@angular/core';
import {Router, RouterLink, RouterOutlet} from '@angular/router';
import {ThemeService} from './services/theme';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('internship-logbook-ui');
  public router = inject(Router);
  public themeService = inject(ThemeService);

  toggleTheme(): void {
    this.themeService.toggle();
  }
}

import { Injectable, signal, inject, effect } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private document = inject(DOCUMENT);

  isDark = signal(this.getStoredTheme());

  constructor() {
    this.applyTheme(this.isDark());

    effect(() => {
      this.applyTheme(this.isDark());
    });
  }

  toggle(): void {
    this.isDark.update(v => !v);
  }

  private applyTheme(dark: boolean): void {
    const html = this.document.documentElement;

    if (dark) {
      html.classList.add('p-dark');
      html.style.colorScheme = 'dark';
    } else {
      html.classList.remove('p-dark');
      html.style.colorScheme = 'light';
    }

    localStorage.setItem('theme', dark ? 'dark' : 'light');
  }

  private getStoredTheme(): boolean {
    const stored = localStorage.getItem('theme');
    if (stored) {
      return stored === 'dark';
    }
    return window.matchMedia('(prefers-color-scheme: dark)').matches;
  }
}

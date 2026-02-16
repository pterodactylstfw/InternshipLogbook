import { Component } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {Auth} from '../services/auth/auth';
import {Router} from '@angular/router';
import {Button} from 'primeng/button';
import {Password, PasswordModule} from 'primeng/password';
import {InputTextModule} from 'primeng/inputtext';
import {CardModule} from 'primeng/card';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-login-page',
  imports: [
    CommonModule, ReactiveFormsModule, Button, InputTextModule, CardModule, PasswordModule
  ],
  templateUrl: './login-page.html',
  styleUrl: './login-page.scss',
})
export class LoginPage {
  loginForm: FormGroup;
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: Auth,
    private router: Router,
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    const { email, password } = this.loginForm.value;

    this.authService.login(email, password).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.role === 'Coordinator') {
          this.router.navigate(['/coordinator-dashboard']);
        } else if (res.role === 'Student') {
          this.router.navigate(['/student-profile']);
        }
      },
      error: (err) => {
        console.error("Eroare la logare:", err);
        this.errorMessage = 'Email sau parolă incorectă!';
        this.isLoading = false;
      }
    });
  }
}

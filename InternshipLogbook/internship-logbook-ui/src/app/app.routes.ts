import { Routes } from '@angular/router';
import {StudentProfile} from './student-profile/student-profile';
import {LoginPage} from './login-page/login-page';
import {authGuard} from './services/auth/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginPage },
  { path: 'student-profile', component: StudentProfile, canActivate: [authGuard] },

  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];

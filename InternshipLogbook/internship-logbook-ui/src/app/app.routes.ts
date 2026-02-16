import { Routes } from '@angular/router';
import {StudentProfile} from './student-profile/student-profile';
import {LoginPage} from './login-page/login-page';

export const routes: Routes = [
  {path: '', redirectTo: 'student-profile', pathMatch: 'full'},
  {path: 'student-profile', component: StudentProfile},
  {path: 'login', component: LoginPage}
];

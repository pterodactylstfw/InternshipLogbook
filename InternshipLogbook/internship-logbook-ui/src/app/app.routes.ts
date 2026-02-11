import { Routes } from '@angular/router';
import {StudentProfile} from './student-profile/student-profile';

export const routes: Routes = [
  {path: '', redirectTo: 'student-profile', pathMatch: 'full'},
  {path: 'student-profile', component: StudentProfile}
];

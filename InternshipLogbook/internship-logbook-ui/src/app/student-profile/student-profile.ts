import {Component, inject} from '@angular/core';
import {AsyncPipe, DatePipe} from '@angular/common';
import {StudentService} from '../services/student';

@Component({
  selector: 'app-student-profile',
  imports: [AsyncPipe, DatePipe],
  templateUrl: './student-profile.html',
  styleUrl: './student-profile.scss',
})
export class StudentProfile {
  private studentService = inject(StudentService);

  student$ = this.studentService.getStudent(1); // hardcodat

  activities$ = this.studentService.getDailyActivities(1); // hardcodat

}

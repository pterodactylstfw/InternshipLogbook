import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Student} from '../models/student';
import {DailyActivity} from '../models/daily-activity';
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  private http = inject(HttpClient); // http req to backend
  private baseURL = 'http://localhost:5203/api';


  getStudent(id: number): Observable<Student> {
    return this.http.get<Student>(`${this.baseURL}/Students/${id}`);
  }

  getDailyActivities(studentId: number): Observable<DailyActivity[]> {
    return this.http.get<DailyActivity[]>(`${this.baseURL}/DailyActivities/student/${studentId}`)
  }
}

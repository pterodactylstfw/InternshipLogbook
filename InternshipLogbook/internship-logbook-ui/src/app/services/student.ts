import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Student } from '../models/student';
import { DailyActivity } from '../models/daily-activity';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  private http = inject(HttpClient);
  private baseURL = 'http://localhost:5203/api';

  getStudent(id: number): Observable<Student> {
    return this.http.get<Student>(`${this.baseURL}/Students/${id}`);
  }

  getDailyActivities(studentId: number): Observable<DailyActivity[]> {
    return this.http.get<DailyActivity[]>(`${this.baseURL}/DailyActivities/student/${studentId}`);
  }

  addDailyActivity(studentId: number, activity: DailyActivity): Observable<DailyActivity> {
    return this.http.post<DailyActivity>(`${this.baseURL}/DailyActivities/student/${studentId}`,
      activity);
  }

  updateDailyActivity(id: number, activity: DailyActivity): Observable<DailyActivity> {
    return this.http.put<DailyActivity>(`${this.baseURL}/DailyActivities/${id}`,
      activity);
  }

  deleteDailyActivity(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseURL}/DailyActivities/${id}`);
  }

  downloadLogbook(studentId: number) {
    return this.http.get(`${this.baseURL}/Export/word/${studentId}`, {
      responseType: 'blob'
    });
  }

  getStudentsByCoordinator(coordId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseURL}/Students/coordinator/${coordId}`);
  }
}

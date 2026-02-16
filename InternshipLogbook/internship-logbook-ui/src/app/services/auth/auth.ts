import { Injectable } from '@angular/core';
import {BehaviorSubject, map, Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';
import {jwtDecode} from 'jwt-decode';


@Injectable({
  providedIn: 'root',
})
export class Auth {
  private baseURL = 'http://localhost:5203/api/Auth';
  private userSubject = new BehaviorSubject<any>(null);
  public user$ = this.userSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {
    this.loadUserFromStorage();
  }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.baseURL}/login`, { email, password }).pipe(
      map(response => {

        localStorage.setItem('token', response.token);

        this.decodeAndNotify(response.token);
        return response;
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.userSubject.next(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }


  private decodeAndNotify(token: string) {
    try {
      const decoded: any = jwtDecode(token);

      const user = {
        email: decoded.email,
        role: decoded.role,
        studentId: decoded.StudentId,
        id: decoded.nameid,
        fullName: decoded.FullName || decoded.email
      };

      this.userSubject.next(user);
    } catch (error) {
      console.error('Token invalid', error);
      this.logout();
    }
  }

  private loadUserFromStorage() {
    const token = this.getToken();
    if (token) {
      this.decodeAndNotify(token);
    }
  }
}

import {Component, inject, OnInit} from '@angular/core';
import {Auth} from '../services/auth/auth';
import {Card} from 'primeng/card';
import {CommonModule} from '@angular/common';
import {TableModule} from 'primeng/table';
import {Button} from 'primeng/button';
import {TagModule} from 'primeng/tag';
import {Dialog} from 'primeng/dialog';
import {StudentService} from '../services/student';

@Component({
  selector: 'app-coordinator-dashboard',
  imports: [
    CommonModule,
    Card,
    TableModule,
    Button,
    Card,
    TagModule,
    Dialog

  ],
  templateUrl: './coordinator-dashboard.html',
  styleUrl: './coordinator-dashboard.scss',
})
export class CoordinatorDashboard implements OnInit {
  public authService = inject(Auth);
  private studentService = inject(StudentService);

  students: any[] = [];
  selectedStudentActivities: any[] = [];

  displayJournal: boolean = false;
  selectedStudentName: string = '';
  loading: boolean = false;

  ngOnInit(): void {
    this.authService.user$.subscribe(user => {
      if (user && user.id) {
        this.loadMyStudents(user.id);
      }
    });
  }

  loadMyStudents(coordId: number) {
    this.loading = true;
    this.studentService.getStudentsByCoordinator(coordId).subscribe({
      next: (data) => {
        this.students = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Eroare la încărcarea studenților:', err);
        this.loading = false;
      }
    });
  }

  viewJournal(student: any) {
    this.selectedStudentName = student.name;
    this.displayJournal = true;
    this.selectedStudentActivities = [];

    this.studentService.getDailyActivities(student.id).subscribe({
      next: (data) => {
        this.selectedStudentActivities = data;
        console.log("Activități încărcate:", data);
      },
      error: (err) => {
        console.error('Eroare la încărcarea jurnalului:', err);
      }
    });
  }

  getSeverity(status: string) {
    switch (status) {
      case 'Completed': return 'success';
      case 'In Progress': return 'info';
      case 'Pending': return 'warn';
      default: return 'secondary';
    }
  }

  logout() {
    this.authService.logout();
  }
}

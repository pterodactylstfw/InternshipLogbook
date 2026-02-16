import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

import { CardModule } from 'primeng/card';
import { TableModule, Table } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { TextareaModule } from 'primeng/textarea';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { SkeletonModule } from 'primeng/skeleton';
import { TooltipModule } from 'primeng/tooltip';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { ConfirmationService, MessageService } from 'primeng/api';

import { StudentService } from '../services/student';
import { DailyActivity } from '../models/daily-activity';
import { Student } from '../models/student';
import {Auth} from '../services/auth/auth';

@Component({
  selector: 'app-student-profile',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CardModule,
    TableModule,
    ButtonModule,
    DatePickerModule,
    TextareaModule,
    InputTextModule,
    InputNumberModule,
    ToastModule,
    ConfirmDialogModule,
    SkeletonModule,
    TooltipModule,
    IconFieldModule,
    InputIconModule,
  ],
  templateUrl: './student-profile.html',
  styleUrl: './student-profile.scss',
})
export class StudentProfile implements OnInit {
  @ViewChild('dt') dt!: Table;

  private studentService = inject(StudentService);
  private fb = inject(FormBuilder);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);
  private authService = inject(Auth);
  private studentId: number | null = null;


  student: Student | null = null;
  studentLoading = true;
  studentError = false;


  activities: DailyActivity[] = [];
  activitiesLoading = true;
  activitiesError = false;


  showForm = false;
  editingActivityId: number | null = null;
  saving = false;

  activityForm: FormGroup = this.fb.group({
    dayNumber:            [null, [Validators.required, Validators.min(1)]],
    dateOfActivity:       [null, Validators.required],
    startTime:            [null, Validators.required],
    endTime:              [null, Validators.required],
    venue:                ['',  Validators.required],
    activities:           ['',  Validators.required],
    equipmentUsed:        [''],
    practicedSkills:      [''],
    personalObservations: [''],
  });

  get isEditing(): boolean {
    return this.editingActivityId !== null;
  }

  ngOnInit(): void {
    this.authService.user$.subscribe(user => {
      if (user && user.studentId) {

        this.studentId = Number(user.studentId);

        this.loadStudent();
        this.loadActivities();
      } else {
        this.studentId = null;
      }
    });
  }

  loadStudent(): void {
    if(!this.studentId) return;

    this.studentLoading = true;
    this.studentError = false;
    this.studentService.getStudent(this.studentId).subscribe({
      next: (data) => {
        this.student = data;
        this.studentLoading = false;
      },
      error: () => {
        this.studentError = true;
        this.studentLoading = false;
      },
    });
  }

  loadActivities(): void {
    if (!this.studentId) return;

    this.activitiesLoading = true;
    this.activitiesError = false;
    this.studentService.getDailyActivities(this.studentId).subscribe({
      next: (data) => {
        this.activities = data;
        this.activitiesLoading = false;
      },
      error: () => {
        this.activitiesError = true;
        this.activitiesLoading = false;
      },
    });
  }

  toggleForm(): void {
    this.showForm = !this.showForm;
    if (this.showForm) {
      this.activityForm.reset();
      this.editingActivityId = null;
    }
  }

  onEdit(activity: DailyActivity): void {
    this.editingActivityId = activity.id!;
    this.showForm = true;

    let startTime: Date | null = null;
    let endTime: Date | null = null;
    if (activity.timeFrame) {
      const parts = activity.timeFrame.split(' - ');
      if (parts.length === 2) {
        startTime = this.parseTime(parts[0].trim());
        endTime = this.parseTime(parts[1].trim());
      }
    }

    this.activityForm.patchValue({
      dayNumber: activity.dayNumber,
      dateOfActivity: new Date(activity.dateOfActivity),
      startTime: startTime,
      endTime: endTime,
      venue: activity.venue,
      activities: activity.activities,
      equipmentUsed: activity.equipmentUsed,
      practicedSkills: activity.skillsPracticed,
      personalObservations: activity.observations,
    });

    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.activityForm.get(fieldName);
    return !!(field && field.invalid && field.touched);
  }

  private formatTime(date: Date): string {
    const h = date.getHours().toString().padStart(2, '0');
    const m = date.getMinutes().toString().padStart(2, '0');
    return `${h}:${m}`;
  }

  private parseTime(timeStr: string): Date {
    const [h, m] = timeStr.split(':').map(Number);
    const d = new Date();
    d.setHours(h, m, 0, 0);
    return d;
  }

  getFieldError(fieldName: string): string {
    const field = this.activityForm.get(fieldName);
    if (!field || !field.errors || !field.touched) return '';
    if (field.errors['required']) return 'Câmpul este obligatoriu';
    if (field.errors['min']) return 'Valoarea minimă este 1';
    return '';
  }

  onFilter(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.dt.filterGlobal(value, 'contains');
  }

  onSave(): void {
    if (!this.studentId) {
      this.messageService.add({
        severity: 'error',
        summary: 'Eroare',
        detail: 'Nu ești autentificat sau datele nu s-au încărcat încă.'
      });
      return;
    }

    if (this.activityForm.invalid) {
      this.activityForm.markAllAsTouched();
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenție',
        detail: 'Completează toate câmpurile obligatorii!',
      });
      return;
    }

    this.saving = true;
    const raw = this.activityForm.value;
    let dateStr = raw.dateOfActivity;
    if (dateStr instanceof Date) {
      dateStr = dateStr.toISOString().split('T')[0];
    }

    const activity: DailyActivity = {
      studentId: this.studentId!,
      dayNumber: raw.dayNumber,
      dateOfActivity: dateStr,
      timeFrame: `${this.formatTime(raw.startTime)} - ${this.formatTime(raw.endTime)}`,
      venue: raw.venue,
      activities: raw.activities,
      equipmentUsed: raw.equipmentUsed ?? '',
      skillsPracticed: raw.practicedSkills ?? '',
      observations: raw.personalObservations ?? '',
    };

    if (this.isEditing) {
      this.studentService.updateDailyActivity(this.editingActivityId!, activity).subscribe({
        next: () => {
          this.loadActivities();
          this.activityForm.reset();
          this.showForm = false;
          this.editingActivityId = null;
          this.saving = false;
          this.messageService.add({
            severity: 'success',
            summary: 'Actualizat',
            detail: 'Activitatea a fost actualizată cu succes!',
          });
        },
        error: (err) => {
          console.error('Eroare la actualizare:', err);
          this.saving = false;
          this.messageService.add({
            severity: 'error',
            summary: 'Eroare',
            detail: 'Nu s-a putut actualiza activitatea.',
          });
        },
      });
    } else {
      this.studentService.addDailyActivity(this.studentId, activity).subscribe({
        next: () => {
          this.loadActivities();
          this.activityForm.reset();
          this.showForm = false;
          this.saving = false;
          this.messageService.add({
            severity: 'success',
            summary: 'Salvat',
            detail: 'Activitatea a fost adăugată cu succes!',
          });
        },
        error: (err) => {
          console.error('Error saving:', err);
          this.saving = false;
          this.messageService.add({
            severity: 'error',
            summary: 'Eroare',
            detail: 'Nu s-a putut salva activitatea.',
          });
        },
      });
    }
  }

  onDelete(activity: DailyActivity): void {
    this.confirmationService.confirm({
      message: `Sigur vrei să ștergi activitatea din Ziua ${activity.dayNumber}?`,
      header: 'Confirmare Ștergere',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Da, șterge',
      rejectLabel: 'Anulează',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.studentService.deleteDailyActivity(activity.id!).subscribe({
          next: () => {
            this.loadActivities();
            this.messageService.add({
              severity: 'success',
              summary: 'Șters',
              detail: `Activitatea din Ziua ${activity.dayNumber} a fost ștearsă.`,
            });
          },
          error: (err) => {
            console.error('Eroare la ștergere:', err);
            this.messageService.add({
              severity: 'error',
              summary: 'Eroare',
              detail: 'Nu s-a putut șterge activitatea.',
            });
          },
        });
      },
    });
  }
  exportToWord() {
    if (!this.studentId) {
      this.messageService.add({severity: 'error', summary: 'Eroare', detail: 'Nu ești autentificat!'});
      return;
    }

    this.studentService.downloadLogbook(this.studentId).subscribe({
      next: (response: Blob) => {
        const fileUrl = window.URL.createObjectURL(response);

        const anchor = document.createElement('a');
        anchor.href = fileUrl;
        const name = this.student?.fullName.replace(/\s+/g, '_') || `Student_${this.studentId}`;

        anchor.download = `Caiet_Practica_${name}.docx`;

        anchor.click();

        window.URL.revokeObjectURL(fileUrl);
      },
      error: (err) => {
        console.error('Eroare la export:', err);
        this.messageService.add({severity: 'error', summary: 'Eroare', detail: 'Nu s-a putut genera documentul.'});
      }
    });
  }

  onLogout(): void {
    this.authService.logout();
  }
}

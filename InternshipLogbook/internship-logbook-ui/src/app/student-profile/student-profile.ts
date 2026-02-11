import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe, NgIf, NgFor } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject, switchMap } from 'rxjs';

import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { TextareaModule } from 'primeng/textarea';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService, MessageService } from 'primeng/api';

import { StudentService } from '../services/student';
import { DailyActivity } from '../models/daily-activity';

@Component({
  selector: 'app-student-profile',
  imports: [
    AsyncPipe,
    DatePipe,
    NgIf,
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
  ],
  templateUrl: './student-profile.html',
  styleUrl: './student-profile.scss',
})
export class StudentProfile {
  private studentService = inject(StudentService);
  private fb = inject(FormBuilder);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);
  private studentId = 1;

  student$ = this.studentService.getStudent(this.studentId);

  private refresh$ = new BehaviorSubject<void>(undefined);
  activities$ = this.refresh$.pipe(
    switchMap(() => this.studentService.getDailyActivities(this.studentId))
  );

  showForm = false;
  editingActivityId: number | null = null;

  activityForm: FormGroup = this.fb.group({
    dayNumber:            [null, [Validators.required, Validators.min(1)]],
    dateOfActivity:       [null, Validators.required],
    timeFrame:            ['',  Validators.required],
    venue:                ['',  Validators.required],
    activities:           ['',  Validators.required],
    equipmentUsed:        [''],
    practicedSkills:      [''],
    personalObservations: [''],
  });

  get isEditing(): boolean {
    return this.editingActivityId !== null;
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

    this.activityForm.patchValue({
      dayNumber: activity.dayNumber,
      dateOfActivity: new Date(activity.dateOfActivity),
      timeFrame: activity.timeFrame,
      venue: activity.venue,
      activities: activity.activities,
      equipmentUsed: activity.equipmentUsed,
      practicedSkills: activity.practicedSkills,
      personalObservations: activity.personalObservations,
    });

    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onSave(): void {
    if (this.activityForm.invalid) {
      this.activityForm.markAllAsTouched();
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenție',
        detail: 'Completează toate câmpurile obligatorii!',
      });
      return;
    }

    const raw = this.activityForm.value;
    let dateStr = raw.dateOfActivity;
    if (dateStr instanceof Date) {
      dateStr = dateStr.toISOString().split('T')[0];
    }

    const activity: DailyActivity = {
      studentId: this.studentId,
      dayNumber: raw.dayNumber,
      dateOfActivity: dateStr,
      timeFrame: raw.timeFrame,
      venue: raw.venue,
      activities: raw.activities,
      equipmentUsed: raw.equipmentUsed ?? '',
      practicedSkills: raw.practicedSkills ?? '',
      personalObservations: raw.personalObservations ?? '',
    };

    if (this.isEditing) {
      this.studentService.updateDailyActivity(this.editingActivityId!, activity).subscribe({
        next: () => {
          this.refresh$.next();
          this.activityForm.reset();
          this.showForm = false;
          this.editingActivityId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'Actualizat',
            detail: 'Activitatea a fost actualizată cu succes!',
          });
        },
        error: (err) => {
          console.error('Eroare la actualizare:', err);
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
          this.refresh$.next();
          this.activityForm.reset();
          this.showForm = false;
          this.messageService.add({
            severity: 'success',
            summary: 'Salvat',
            detail: 'Activitatea a fost adăugată cu succes!',
          });
        },
        error: (err) => {
          console.error('Error saving:', err);
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
            this.refresh$.next();
            this.messageService.add({
              severity: 'success',
              summary: 'Șters',
              detail: `Activitatea din Ziua ${activity.dayNumber} a fost ștearsă.`,
            });
          },
          error: (err) => {
            console.error('Error deleting:', err);
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
}

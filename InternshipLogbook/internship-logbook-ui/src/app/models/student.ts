import {DailyActivity} from './daily-activity';

export interface Student {
  id?: number;
  fullName: string;
  faculty: string;
  studyProgramme: string;
  yearOfStudy: number;
  internshipPeriod: string;
  internshipDirector: string;
  hostInstitution: string;
  hostTutor: string;
  hostDescription: string;
  dailyActivities: DailyActivity[];
}

export interface DailyActivity {
  id?: number;
  studentId: number;
  dayNumber: number;
  dateOfActivity: string;
  timeFrame: string;
  venue: string;
  activities: string;
  equipmentUsed: string;
  skillsPracticed: string;
  observations: string;
}

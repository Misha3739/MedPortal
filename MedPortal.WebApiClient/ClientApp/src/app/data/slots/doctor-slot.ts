export interface ITimeSlot {
  id: number;
  startTime: Date;
  finishTime: Date;
}

export interface IDoctorClinicTimeSlot extends ITimeSlot {
  clinicId: number;
  doctorId: number;
}


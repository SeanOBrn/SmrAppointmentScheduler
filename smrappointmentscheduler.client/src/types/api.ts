export interface AvailableSlotDto {
  appointmentSlotId: number;
  branchId: number;
  branchName: string;
  mechanicId: number;
  mechanicName: string;
  serviceTypeId: number;
  serviceTypeName: string;
  start: string; // ISO string
  end: string; // ISO string
  isBooked: boolean;
}

export interface BookingConfirmationDto {
  appointmentId: number;
  appointmentSlotId: number;
  customerName: string;
  status: string;
  bookingReference?: string;
}

export interface CreateBookingRequestDto {
  appointmentSlotId: number;
  customerName: string;
  customerPhone?: string | null;
  serviceTypeId: number;
  branchId: number;
  vehicleRegistration?: string | null;
  notes?: string | null;
}

export interface MechanicDto {
  id: number;
  firstName: string;
  lastName: string;
  fullName?: string;
  branchId: number;
}

export interface MechanicAppointmentDto {
  appointmentId: number;
  appointmentSlotId: number;
  start: string;
  end: string;
  customerName: string;
  status: string;
}

export interface WorkNoteDto {
  id?: number;
  appointmentId: number;
  note: string;
  createdAt?: string;
}

export interface AppointmentDetailDto {
  appointmentId: number;
  appointmentSlotId: number;
  branchId: number;
  branchName: string;
  mechanicId: number;
  mechanicName: string;
  serviceTypeId: number;
  serviceTypeName: string;
  start: string;
  end: string;
  customerName: string;
  customerPhone?: string | null;
  vehicleRegistration?: string | null;
  status: string;
  workNotes: WorkNoteDto[];
}

export interface UpdateAppointmentStatusDto {
  appointmentId: number;
  status: string; // Scheduled, InProgress, Completed, NoShow
}

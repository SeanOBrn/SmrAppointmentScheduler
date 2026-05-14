import type { AvailableSlotDto, CreateBookingRequestDto, BookingConfirmationDto, MechanicDto, MechanicAppointmentDto, AppointmentDetailDto, WorkNoteDto, UpdateAppointmentStatusDto } from '../types/api';
const BASE = '/api';

async function handleResponse(response: Response) {
  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || response.statusText);
  }
  if (response.status === 204) return null;
  return response.json();
}

export async function getSlots(branchId?: number, serviceTypeId?: number): Promise<any> {
  const params = new URLSearchParams();
  if (branchId) params.append('branchId', String(branchId));
  if (serviceTypeId) params.append('serviceTypeId', String(serviceTypeId));
  const res = await fetch(`${BASE}/slots?${params.toString()}`);
  return await handleResponse(res);
}

export async function createBooking(payload: any): Promise<any> {
  const res = await fetch(`${BASE}/bookings`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });
  return await handleResponse(res);
}

export async function getMechanics(branchId?: number): Promise<any> {
  const params = new URLSearchParams();
  if (branchId) params.append('branchId', String(branchId));
  const res = await fetch(`${BASE}/mechanics?${params.toString()}`);
  return await handleResponse(res);
}

export async function getMechanicAppointments(mechanicId: number): Promise<any> {
  const res = await fetch(`${BASE}/mechanics/${mechanicId}/appointments`);
  return await handleResponse(res);
}

export async function getAppointment(appointmentId: number): Promise<any> {
  const res = await fetch(`${BASE}/appointments/${appointmentId}`);
  return await handleResponse(res);
}

export async function addWorkNote(appointmentId: number, note: string): Promise<void> {
  const res = await fetch(`${BASE}/appointments/${appointmentId}/worknotes`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ appointmentId, note })
  });
  return handleResponse(res) as Promise<void>;
}

export async function updateAppointmentStatus(appointmentId: number, status: string): Promise<void> {
  const res = await fetch(`${BASE}/appointments/${appointmentId}/status`, {
    method: 'PATCH',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ appointmentId, status })
  });
  return handleResponse(res) as Promise<void>;
}

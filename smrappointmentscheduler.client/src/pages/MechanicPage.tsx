import { useEffect, useState } from 'react';
import { getMechanics, getMechanicAppointments, getAppointment, addWorkNote, updateAppointmentStatus } from '../api/api';
import type { MechanicDto, MechanicAppointmentDto, AppointmentDetailDto } from '../types/api';

export default function MechanicPage() {
  const [mechanics, setMechanics] = useState<MechanicDto[]>([]);
  const [selectedMechanicId, setSelectedMechanicId] = useState<number | null>(null);
  const [appointments, setAppointments] = useState<MechanicAppointmentDto[]>([]);
  const [selectedAppointmentId, setSelectedAppointmentId] = useState<number | null>(null);
  const [appointmentDetail, setAppointmentDetail] = useState<AppointmentDetailDto | null>(null);
  const [newNote, setNewNote] = useState('');
  const [statusUpdating, setStatusUpdating] = useState(false);

  /* eslint-disable react-hooks/set-state-in-effect */
  useEffect(() => {
    // call the shared loader defined below
    void loadMechanics();
  }, []);
  /* eslint-enable react-hooks/set-state-in-effect */

  useEffect(() => {
    if (selectedMechanicId !== null) {
      loadAppointments(selectedMechanicId);
    }
  }, [selectedMechanicId]);

  useEffect(() => {
    if (selectedAppointmentId !== null) {
      loadAppointmentDetail(selectedAppointmentId);
    } else {
      setAppointmentDetail(null);
    }
  }, [selectedAppointmentId]);

  async function loadMechanics() {
    const data = await getMechanics() as any[];
    setMechanics(data);
    if (data.length > 0) setSelectedMechanicId(data[0].id);
  }

  async function loadAppointments(mechanicId: number) {
    const data = await getMechanicAppointments(mechanicId);
    setAppointments(data);
  }

  async function loadAppointmentDetail(appointmentId: number) {
    const data = await getAppointment(appointmentId);
    setAppointmentDetail(data);
  }

  async function handleAddNote() {
    if (!selectedAppointmentId || !newNote.trim()) return;
    await addWorkNote(selectedAppointmentId, newNote);
    setNewNote('');
    if (selectedAppointmentId) await loadAppointmentDetail(selectedAppointmentId);
  }

  async function handleUpdateStatus(status: string) {
    if (!selectedAppointmentId) return;
    setStatusUpdating(true);
    try {
      await updateAppointmentStatus(selectedAppointmentId, status);
      if (selectedAppointmentId) await loadAppointmentDetail(selectedAppointmentId);
      if (selectedMechanicId) await loadAppointments(selectedMechanicId);
    } finally {
      setStatusUpdating(false);
    }
  }

  return (
    <div style={{ padding: '1rem' }}>
      <h1>Mechanic Schedule</h1>

      <div style={{ marginBottom: '1rem' }}>
        <label>Mechanic</label>
        <select value={selectedMechanicId ?? ''} onChange={e => setSelectedMechanicId(e.target.value ? Number(e.target.value) : null)}>
          {mechanics.map(m => (
            <option key={m.id} value={m.id}>{m.firstName} {m.lastName}</option>
          ))}
        </select>
      </div>

      <div style={{ display: 'flex', gap: '1rem' }}>
        <div style={{ flex: 1 }}>
          <h2>Appointments (Today & Tomorrow)</h2>
          <ul>
            {appointments.map(a => (
              <li key={a.appointmentId} style={{ marginBottom: '0.5rem' }}>
                <button onClick={() => setSelectedAppointmentId(a.appointmentId)} style={{ marginRight: '0.5rem' }}>Open</button>
                <strong>{new Date(a.start).toLocaleString()}</strong> - {a.customerName} ({a.status})
              </li>
            ))}
          </ul>
        </div>

        <div style={{ flex: 1 }}>
          <h2>Appointment Details</h2>
          {appointmentDetail ? (
            <div>
              <div><strong>Customer:</strong> {appointmentDetail.customerName}</div>
              <div><strong>Phone:</strong> {appointmentDetail.customerPhone ?? 'N/A'}</div>
              <div><strong>Vehicle:</strong> {appointmentDetail.vehicleRegistration ?? 'N/A'}</div>
              <div><strong>Status:</strong> {appointmentDetail.status}</div>

              <h3>Notes</h3>
              <div>{appointmentDetail.workNotes.length === 0 ? <em>No work notes</em> : (
                <ul>
                  {appointmentDetail.workNotes.map(n => (
                    <li key={n.id}>{new Date(n.createdAt || '').toLocaleString()}: {n.note}</li>
                  ))}
                </ul>
              )}</div>

              <div style={{ marginTop: '1rem' }}>
                <textarea value={newNote} onChange={e => setNewNote(e.target.value)} rows={3} />
                <div>
                  <button onClick={handleAddNote} disabled={!newNote.trim()}>Add Note</button>
                </div>
              </div>

              <div style={{ marginTop: '1rem' }}>
                <button onClick={() => handleUpdateStatus('InProgress')} disabled={statusUpdating}>Mark In Progress</button>
                <button onClick={() => handleUpdateStatus('Completed')} style={{ marginLeft: '0.5rem' }} disabled={statusUpdating}>Mark Completed</button>
                <button onClick={() => handleUpdateStatus('NoShow')} style={{ marginLeft: '0.5rem' }} disabled={statusUpdating}>Mark No-Show</button>
              </div>
            </div>
          ) : (
            <div>Select an appointment to view details.</div>
          )}
        </div>
      </div>
    </div>
  );
}

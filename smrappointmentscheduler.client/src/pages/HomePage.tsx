/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useState } from 'react';
import { getMechanics, getMechanicAppointments, getAppointment } from '../api/api';
import type { MechanicDto, AppointmentDetailDto } from '../types/api';

type MechanicGroup = {
  mechanic: MechanicDto;
  appointments: AppointmentDetailDto[];
};

export default function HomePage() {
  const [groups, setGroups] = useState<MechanicGroup[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function loadTodayAppointments() {
    setLoading(true);
    setError(null);
    try {

      const mechanics = await getMechanics() as MechanicDto[];

      const results = await Promise.all(mechanics.map(async (m) => {
        const basic = await getMechanicAppointments(m.id) as { appointmentId: number }[];
        // fetch full details for each appointment
        const details = await Promise.all(basic.map(async (a) => {
          try {
            const d = await getAppointment(a.appointmentId);
            return d as AppointmentDetailDto;
          } catch {
            return null;
          }
        }));

        const filtered = details.filter((d): d is AppointmentDetailDto => d !== null);
        return { mechanic: m, appointments: filtered } as MechanicGroup;
      }));

      // sort appointments within group by start
      results.forEach(g => g.appointments.sort((a, b) => new Date(a.start).getTime() - new Date(b.start).getTime()));

      // update state once after all awaits complete
      setGroups(results.filter(g => g.appointments.length > 0));
    } catch (err) {
      const e = err as Error;
      setError(e?.message || 'Failed to load appointments');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    void (async () => { await loadTodayAppointments(); })();
  }, []);

  return (
    <div style={{ padding: '1rem' }}>
      <h1>Today's Appointments</h1>
      {loading && <div>Loading...</div>}
      {error && <div style={{ color: 'red' }}>{error}</div>}

      {!loading && groups.length === 0 && <div>No appointments for today.</div>}

      {groups.map(g => (
        <div key={g.mechanic.id} style={{ marginBottom: '1.5rem' }}>
          <h2 style={{ marginBottom: '0.5rem' }}>{g.mechanic.firstName} {g.mechanic.lastName}</h2>
          <table style={{ width: '100%', borderCollapse: 'collapse' }}>
            <thead>
              <tr style={{ textAlign: 'left', borderBottom: '1px solid #ddd' }}>
                <th style={{ padding: '0.5rem' }}>Time</th>
                <th style={{ padding: '0.5rem' }}>Customer</th>
                <th style={{ padding: '0.5rem' }}>Vehicle</th>
                <th style={{ padding: '0.5rem' }}>Service</th>
                <th style={{ padding: '0.5rem' }}>Status</th>
              </tr>
            </thead>
            <tbody>
              {g.appointments.map(a => (
                <tr key={a.appointmentId} style={{ borderBottom: '1px solid #f0f0f0' }}>
                  <td style={{ padding: '0.5rem', verticalAlign: 'top' }}>{new Date(a.start).toLocaleTimeString()}</td>
                  <td style={{ padding: '0.5rem', verticalAlign: 'top' }}>{a.customerName}</td>
                  <td style={{ padding: '0.5rem', verticalAlign: 'top' }}>{a.vehicleRegistration ?? 'N/A'}</td>
                  <td style={{ padding: '0.5rem', verticalAlign: 'top' }}>{a.serviceTypeName}</td>
                  <td style={{ padding: '0.5rem', verticalAlign: 'top' }}>{a.status}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      ))}
    </div>
  );
}

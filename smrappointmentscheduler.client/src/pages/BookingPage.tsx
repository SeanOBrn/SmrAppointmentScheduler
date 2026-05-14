import { useEffect, useState } from 'react';
import { getSlots, createBooking } from '../api/api';
import type { AvailableSlotDto, CreateBookingRequestDto, BookingConfirmationDto } from '../types/api';

export default function BookingPage() {
  const [slots, setSlots] = useState<AvailableSlotDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [branchId, setBranchId] = useState<number | undefined>(undefined);
  const [serviceTypeId, setServiceTypeId] = useState<number | undefined>(undefined);

  const [selectedSlot, setSelectedSlot] = useState<AvailableSlotDto | null>(null);
  const [customerName, setCustomerName] = useState('');
  const [customerPhone, setCustomerPhone] = useState('');
  const [vehicleRegistration, setVehicleRegistration] = useState('');
  const [notes, setNotes] = useState('');

  const [confirmation, setConfirmation] = useState<BookingConfirmationDto | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadSlots();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  async function loadSlots(bId?: number, sId?: number) {
    setLoading(true);
    setError(null);
    try {
      const data = await getSlots(bId, sId);
      setSlots(data);
    } catch (err: any) {
      setError(err?.message || 'Failed to load slots');
    } finally {
      setLoading(false);
    }
  }

  // derive branch and service options from slots
  const branches = Array.from(new Map(slots.map(s => [s.branchId, { id: s.branchId, name: s.branchName }])).values());
  const services = Array.from(new Map(slots.map(s => [s.serviceTypeId, { id: s.serviceTypeId, name: s.serviceTypeName }])).values());

  async function applyFilters() {
    await loadSlots(branchId, serviceTypeId);
  }

  function openBooking(slot: AvailableSlotDto) {
    setSelectedSlot(slot);
    setCustomerName('');
    setCustomerPhone('');
    setVehicleRegistration('');
    setNotes('');
    setConfirmation(null);
    setError(null);
  }

  async function submitBooking(e: React.FormEvent) {
    e.preventDefault();
    if (!selectedSlot) return;
    setError(null);
    try {
      const payload: CreateBookingRequestDto = {
        appointmentSlotId: selectedSlot.appointmentSlotId,
        customerName: customerName,
        customerPhone: customerPhone || undefined,
        serviceTypeId: selectedSlot.serviceTypeId,
        branchId: selectedSlot.branchId,
        vehicleRegistration: vehicleRegistration || undefined,
        notes: notes || undefined
      };

      const result = await createBooking(payload);
      setConfirmation(result);

      // refresh slots
      await loadSlots(branchId, serviceTypeId);
      setSelectedSlot(null);
    } catch (err: any) {
      setError(err?.message || 'Failed to create booking');
    }
  }

  return (
    <div style={{ padding: '1rem' }}>
      <h1>Book an Appointment</h1>

      <div style={{ marginBottom: '1rem' }}>
        <label style={{ marginRight: '0.5rem' }}>Branch</label>
        <select value={branchId ?? ''} onChange={e => setBranchId(e.target.value ? Number(e.target.value) : undefined)}>
          <option value="">All</option>
          {branches.map(b => (
            <option key={b.id} value={b.id}>{b.name}</option>
          ))}
        </select>

        <label style={{ marginLeft: '1rem', marginRight: '0.5rem' }}>Service</label>
        <select value={serviceTypeId ?? ''} onChange={e => setServiceTypeId(e.target.value ? Number(e.target.value) : undefined)}>
          <option value="">All</option>
          {services.map(s => (
            <option key={s.id} value={s.id}>{s.name}</option>
          ))}
        </select>

        <button style={{ marginLeft: '1rem' }} onClick={applyFilters}>Filter</button>
      </div>

      {loading && <div>Loading slots...</div>}
      {error && <div style={{ color: 'red' }}>{error}</div>}

      <div>
        <h2>Available Slots</h2>
        {slots.length === 0 && !loading && <div>No slots available.</div>}
        <ul>
          {slots.map(s => (
            <li key={s.appointmentSlotId} style={{ marginBottom: '0.5rem' }}>
              <strong>{new Date(s.start).toLocaleString()}</strong> — {s.branchName} / {s.mechanicName} / {s.serviceTypeName}
              <button style={{ marginLeft: '1rem' }} onClick={() => openBooking(s)}>Book</button>
            </li>
          ))}
        </ul>
      </div>

      {selectedSlot && (
        <div style={{ marginTop: '1rem', padding: '1rem', border: '1px solid #ccc' }}>
          <h3>Booking for {new Date(selectedSlot.start).toLocaleString()}</h3>
          <form onSubmit={submitBooking}>
            <div style={{ marginBottom: '0.5rem' }}>
              <label style={{ display: 'block' }}>Customer name</label>
              <input value={customerName} onChange={e => setCustomerName(e.target.value)} required />
            </div>

            <div style={{ marginBottom: '0.5rem' }}>
              <label style={{ display: 'block' }}>Phone</label>
              <input value={customerPhone} onChange={e => setCustomerPhone(e.target.value)} />
            </div>

            <div style={{ marginBottom: '0.5rem' }}>
              <label style={{ display: 'block' }}>Vehicle registration</label>
              <input value={vehicleRegistration} onChange={e => setVehicleRegistration(e.target.value)} />
            </div>

            <div style={{ marginBottom: '0.5rem' }}>
              <label style={{ display: 'block' }}>Notes</label>
              <textarea value={notes} onChange={e => setNotes(e.target.value)} rows={3} />
            </div>

            <div>
              <button type="submit">Confirm booking</button>
              <button type="button" style={{ marginLeft: '0.5rem' }} onClick={() => setSelectedSlot(null)}>Cancel</button>
            </div>
          </form>
        </div>
      )}

      {confirmation && (
        <div style={{ marginTop: '1rem', padding: '1rem', border: '1px solid #4caf50', background: '#e8f5e9' }}>
          <h3>Booking Confirmed</h3>
          <div>Reference: <strong>{confirmation.bookingReference}</strong></div>
          <div>Appointment ID: {confirmation.appointmentId}</div>
        </div>
      )}
    </div>
  );
}

import { Link } from 'react-router-dom';

export default function NavBar() {
  return (
    <nav style={{ padding: '1rem', borderBottom: '1px solid #ddd' }}>
      <Link to="/" style={{ marginRight: '1rem' }}>Home</Link>
      <Link to="/booking" style={{ marginRight: '1rem' }}>Booking</Link>
      <Link to="/mechanics" style={{ marginRight: '1rem' }}>Mechanics</Link>
    </nav>
  );
}

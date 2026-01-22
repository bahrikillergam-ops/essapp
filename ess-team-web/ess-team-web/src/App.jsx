import { useEffect, useState } from 'react';
import { managerService } from './services/api';

function App() {
  const [managers, setManagers] = useState([]);

  useEffect(() => {
    // When the page loads, ask the backend for managers
    managerService.getAll()
      .then(response => {
        setManagers(response.data);
      })
      .catch(error => {
        console.error("The API is not answering!", error);
      });
  }, []);

  return (
    <div style={{ padding: '40px', fontFamily: 'sans-serif' }}>
      <h1 style={{ color: '#0078d4' }}>ESS Team Management</h1>
      <h2>Managers List</h2>
      
      {managers.length > 0 ? (
        <table border="1" cellPadding="10" style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ backgroundColor: '#f2f2f2' }}>
              <th>First Name</th>
              <th>Last Name</th>
              <th>Role</th>
              <th>Email</th>
            </tr>
          </thead>
          <tbody>
            {managers.map(m => (
              <tr key={m.managerId}>
                <td>{m.firstName}</td>
                <td>{m.lastName}</td>
                <td>{m.role}</td>
                <td>{m.phone}</td>
                <td>{m.email}</td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <p>Connecting to backend... If this stays empty, make sure your .NET API is running!</p>
      )}
    </div>
  );
}

export default App;
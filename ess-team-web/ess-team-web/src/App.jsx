import { useEffect, useState } from 'react';
import { managerService } from './services/api';

function App() {
  const [managers, setManagers] = useState([]);

  // --- NEW: Function to load data from API ---
  const loadManagers = () => {
    managerService.getAll()
      .then(response => {
        setManagers(response.data);
      })
      .catch(error => {
        console.error("The API is not answering!", error);
      });
  };

  useEffect(() => {
    loadManagers(); // Load data when page opens
  }, []);

  // --- NEW: Function to delete a manager ---
  const handleDelete = (id) => {
    if (window.confirm("Are you sure you want to delete this manager?")) {
      managerService.delete(id)
        .then(() => {
          alert("Manager deleted successfully!");
          loadManagers(); // Refresh the table so the person disappears
        })
        .catch(error => {
          console.error("Delete failed:", error);
          alert("Could not delete. Check your backend!");
        });
    }
  };

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
              <th>Phone</th>
              <th>Email</th>
              <th>Actions</th>
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
                <td>
                  <button 
                    onClick={() => handleDelete(m.managerId)} 
                    style={{ backgroundColor: '#ff4d4d', color: 'white', border: 'none', padding: '5px 10px', borderRadius: '4px', cursor: 'pointer' }}
                  >
                    Delete
                  </button>
                </td>
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
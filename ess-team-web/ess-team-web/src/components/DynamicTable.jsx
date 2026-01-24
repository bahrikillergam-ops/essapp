import React from 'react';

const DynamicTable = ({ data, onDelete }) => {
  if (!data || data.length === 0) return <p>No data found.</p>;

  // Automatically get headers from the object keys
  const headers = Object.keys(data[0]);

  return (
    <div className="table-responsive">
      <table className="ess-table">
        <thead>
          <tr>
            {headers.map((key) => (
              <th key={key}>{key.replace(/([A-Z])/g, ' $1').toUpperCase()}</th>
            ))}
            <th>ACTIONS</th>
          </tr>
        </thead>
        <tbody>
          {data.map((item, index) => (
            <tr key={index}>
              {headers.map((key) => (
                <td key={key}>{String(item[key] ?? '')}</td>
              ))}
              <td>
                <button 
                  className="del-btn" 
                  onClick={() => onDelete(item.id || item.playerId || item.managerId)}
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default DynamicTable;
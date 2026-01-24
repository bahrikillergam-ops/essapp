import React, { useState, useEffect } from 'react';

const DynamicModal = ({ isOpen, onClose, onSave, sectionName, templateItem }) => {
  const [formData, setFormData] = useState({});

  // When the modal opens, fill the form with existing data (for Update) 
  // or a blank blueprint (for Add).
  useEffect(() => {
    if (isOpen && templateItem) {
      setFormData(templateItem);
    }
  }, [isOpen, templateItem]);

  if (!isOpen) return null;

  // Logic to determine which fields to show
  const fields = Object.keys(templateItem).filter(key => {
    // 1. Always show 'managerId' because it is a required Foreign Key
    if (key.toLowerCase() === 'managerid') return true;
    
    // 2. Hide other IDs (like playerId, matchId) because they are auto-generated
    const isInternalId = key.toLowerCase().endsWith('id');
    
    // 3. Hide complex objects/navigation properties (prevent [object Object])
    const isObject = typeof templateItem[key] === 'object';

    return !isInternalId && !isObject;
  });

  const handleChange = (e) => {
    const { name, value } = e.target;

    // DATA TYPE CONVERSION:
    // Ensure numeric fields are sent as Integers so the .NET API doesn't crash
    const isNumeric = name.toLowerCase().includes('id') || 
                      name === 'quantity' || 
                      name === 'jerseyNumber';
    
    const val = isNumeric ? (parseInt(value, 10) || 0) : value;

    setFormData({ ...formData, [name]: val });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSave(formData);
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <header className="modal-header">
          <h3 style={{ color: 'var(--ess-red)', margin: 0 }}>
            {formData.id || formData.playerId || formData.managerId ? 'UPDATE' : 'ADD NEW'} {sectionName.toUpperCase()}
          </h3>
        </header>

        <form onSubmit={handleSubmit} className="modal-form">
          <div className="form-grid">
            {fields.map(field => (
              <div key={field} className="modal-field">
                <label>{field.replace(/([A-Z])/g, ' $1').toUpperCase()}</label>
                <input 
                  name={field}
                  // Populate with existing value if editing
                  value={formData[field] || ''}
                  // Auto-switch input type based on field name
                  type={
                    field.toLowerCase().includes('date') ? 'date' : 
                    field.toLowerCase().includes('time') ? 'time' : 
                    field.toLowerCase().includes('email') ? 'email' : 'text'
                  }
                  placeholder={field === 'managerId' ? "Required: Enter Manager ID Number" : ""}
                  onChange={handleChange}
                  required
                />
              </div>
            ))}
          </div>

          <div className="modal-btns">
            <button type="button" onClick={onClose} className="btn-cancel">
              CANCEL
            </button>
            <button type="submit" className="btn-save">
              SAVE TO DATABASE
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default DynamicModal;
import React, { useState, useEffect } from 'react';

const DynamicModal = ({ isOpen, onClose, onSave, sectionName, templateItem }) => {
  const [formData, setFormData] = useState({});

  useEffect(() => {
    if (isOpen) setFormData(templateItem);
  }, [isOpen, templateItem]);

  if (!isOpen) return null;

  const fields = Object.keys(templateItem).filter(key => {
    const isInternalId = (key.toLowerCase().endsWith('id') && key.toLowerCase() !== 'managerid');
    const isObject = typeof templateItem[key] === 'object';
    return !isInternalId && !isObject;
  });

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <h2 className="modal-title">UPDATE {sectionName.toUpperCase()}</h2>
        <form onSubmit={(e) => { e.preventDefault(); onSave(formData); }}>
          
          {fields.map(field => (
            <div key={field} className="form-row">
              <label className="form-label">{field.replace(/([A-Z])/g, ' $1').toUpperCase()}</label>
              <input 
                className="form-input"
                name={field}
                value={formData[field] || ''}
                onChange={(e) => setFormData({...formData, [field]: e.target.value})}
                required
              />
            </div>
          ))}

          <div className="modal-actions">
            <button type="button" onClick={onClose} className="btn-modal-cancel">CANCEL</button>
            <button type="submit" className="btn-modal-submit">SAVE TO DATABASE</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default DynamicModal;
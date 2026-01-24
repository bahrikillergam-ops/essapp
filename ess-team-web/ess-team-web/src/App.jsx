import { useEffect, useState } from 'react';
import { genericService } from './services/api';
import DynamicModal from './components/DynamicModal';
import './App.css';

const SECTIONS = ['players', 'managers', 'matches', 'trainings', 'equipment'];

const BLUEPRINTS = {
  players: { firstName: "", lastName: "", position: "", jerseyNumber: 0, phone: "", email: "", managerId: 1 },
  managers: { firstName: "", lastName: "", role: "", phone: "", email: "" },
  matches: { matchDate: "", opponent: "", location: "", result: "", managerId: 1 },
  trainings: { trainingDate: "", time: "", location: "", focus: "", managerId: 1 },
  equipment: { equipmentName: "", quantity: 0, condition: "", managerId: 1 }
};

function App() {
  const [currentSection, setCurrentSection] = useState('players');
  const [list, setList] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingItem, setEditingItem] = useState(null);

  const loadData = async () => {
    try {
      const response = await genericService(currentSection).getAll();
      setList(response.data);
    } catch (error) { setList([]); }
  };

  useEffect(() => { loadData(); }, [currentSection]);

  const handleSave = async (formData) => {
    try {
      const id = editingItem?.id || editingItem?.playerId || editingItem?.managerId;
      if (editingItem) {
        await genericService(currentSection).update(id, formData);
      } else {
        await genericService(currentSection).create(formData);
      }
      setIsModalOpen(false);
      setEditingItem(null);
      loadData();
    } catch (err) { alert("Action failed. Check Manager ID."); }
  };

  const openEditModal = (item) => {
    setEditingItem(item);
    setIsModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm("Are you sure you want to delete this record?")) {
      try {
        await genericService(currentSection).delete(id);
        loadData();
      } catch (err) { alert("Delete failed."); }
    }
  };

  const columns = list.length > 0 
    ? Object.keys(list[0]).filter(key => typeof list[0][key] !== 'object' && list[0][key] !== null)
    : Object.keys(BLUEPRINTS[currentSection]);

  return (
    <div className="ess-container">
      <aside className="ess-sidebar">
        <div className="sidebar-header">
         <img src="src/assets/logo-ess.png" alt="ESS Logo" className="sidebar-logo" />
          <h2 className="club-title">Étoile Sportive du Sahel <br/> Volley Ball</h2>
        </div>
        
        <nav className="sidebar-nav">
          {SECTIONS.map(s => (
            <button 
              key={s} 
              className={`nav-link ${currentSection === s ? 'active' : ''}`} 
              onClick={() => setCurrentSection(s)}
            >
              {s.toUpperCase()}
            </button>
          ))}
        </nav>
      </aside>

      <main className="ess-main">
        <header className="main-header">
          <h1 className="section-title">{currentSection.toUpperCase()}</h1>
          <button className="btn-add-main" onClick={() => { setEditingItem(null); setIsModalOpen(true); }}>
            + NEW {currentSection.slice(0, -1).toUpperCase()}
          </button>
        </header>

        <div className="table-card">
          <table className="ess-table">
            <thead>
              <tr>
                {columns.map(col => <th key={col}>{col.toUpperCase()}</th>)}
                <th style={{textAlign: 'center'}}>ACTIONS</th>
              </tr>
            </thead>
            <tbody>
              {list.map((item, i) => (
                <tr key={i}>
                  {columns.map(col => <td key={col}>{String(item[col] ?? '')}</td>)}
                  <td className="action-cell">
                    <button className="btn-edit-action" onClick={() => openEditModal(item)}>Edit</button>
                    <button className="btn-del-action" onClick={() => handleDelete(item.id || item.playerId || item.managerId)}>Delete</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        <DynamicModal 
          isOpen={isModalOpen}
          onClose={() => { setIsModalOpen(false); setEditingItem(null); }}
          onSave={handleSave}
          sectionName={currentSection}
          templateItem={editingItem || BLUEPRINTS[currentSection]}
        />
      </main>
    </div>
  );
}

export default App;
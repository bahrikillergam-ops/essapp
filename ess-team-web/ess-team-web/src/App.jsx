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
  const [editingItem, setEditingItem] = useState(null); // Tracks if we are updating

  const loadData = async () => {
    try {
      const response = await genericService(currentSection).getAll();
      setList(response.data);
    } catch (error) { setList([]); }
  };

  useEffect(() => { loadData(); }, [currentSection]);

  // Handle both Create and Update
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
    } catch (err) {
      alert("Error saving data. Check if Manager ID exists!");
    }
  };

  const openEditModal = (item) => {
    setEditingItem(item);
    setIsModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm("Delete this record?")) {
      await genericService(currentSection).delete(id);
      loadData();
    }
  };

  const columns = list.length > 0 
    ? Object.keys(list[0]).filter(key => typeof list[0][key] !== 'object' && list[0][key] !== null)
    : Object.keys(BLUEPRINTS[currentSection]);

  return (
    <div className="ess-container">
      <aside className="ess-sidebar">
        <div className="sidebar-brand">
          <img src="/logo_letoile-removebg-preview (1).png" alt="ESS" className="sidebar-logo" />
          <h1>ESS VOLLEY</h1>
        </div>
        <nav>
          {SECTIONS.map(s => (
            <button key={s} className={currentSection === s ? 'nav-item active' : 'nav-item'} onClick={() => setCurrentSection(s)}>
              {s.toUpperCase()}
            </button>
          ))}
        </nav>
      </aside>

      <main className="ess-main">
        <header className="ess-header">
          <h2>{currentSection.toUpperCase()}</h2>
          <button className="btn-add" onClick={() => { setEditingItem(null); setIsModalOpen(true); }}>+ ADD NEW</button>
        </header>

        <div className="ess-card">
          <table className="ess-table">
            <thead>
              <tr>
                {columns.map(col => <th key={col}>{col.toUpperCase()}</th>)}
                <th>ACTIONS</th>
              </tr>
            </thead>
            <tbody>
              {list.map((item, i) => (
                <tr key={i}>
                  {columns.map(col => <td key={col}>{String(item[col] ?? '')}</td>)}
                  <td className="action-btns">
                    <button className="btn-edit" onClick={() => openEditModal(item)}>Edit</button>
                    <button className="btn-del" onClick={() => handleDelete(item.id || item.playerId || item.managerId)}>Delete</button>
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
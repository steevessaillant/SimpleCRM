import { CustomerForm } from './components/CustomerForm.tsx';

function App() {
  return (
    <div>
      <header>
        <h1>Main SPA Page</h1>
      </header>
      <div style={{color: 'brown'}}>
        <h2>Component 1</h2>
        <div><CustomerForm /></div>
      </div>
    </div>

  );
}

export default App;

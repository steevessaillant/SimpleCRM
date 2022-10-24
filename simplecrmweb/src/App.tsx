import * as React from 'react';
import { CustomerForm } from './components/CustomerForm';

function App() {
  return (
    <div>
      <header>
        <h1>Main SPA Page</h1>
      </header>
      <div style={{color: 'green'}}>
        <h2>Component 1</h2>
        <div><CustomerForm /></div>
      </div>
    </div>

  );
}

export default App;

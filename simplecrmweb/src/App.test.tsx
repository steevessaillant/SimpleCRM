import * as React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import App from './App';
import {CustomerForm} from './components/CustomerForm';

test('renders react root', () => {
  render(<App/>);
});

test('renders react CustomerForm component ', () => {
  render(<CustomerForm />);
});

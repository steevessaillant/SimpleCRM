/**
 * @jest-environment jsdom
 */

import { render} from '@testing-library/react';
import App from './App';
import {CustomerForm} from './components/CustomerForm';


test('renders react root', () => {
  render(App());
});

test('renders react CustomerForm component ', () => {
  render(CustomerForm());
});

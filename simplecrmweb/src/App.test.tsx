/**
 * @jest-environment jsdom
 */
import React from 'react'
import { act, fireEvent, getByText, render, screen, waitFor} from '@testing-library/react';
import App from './App';
import {CustomerForm} from './components/CustomerForm';

test('renders react root', () => {
  render(<App/>);
});

test('renders react CustomerForm component ', () => {
  render(<CustomerForm />);
});


describe("when the button is pressed", () => {
  
  

  it("should display required error", async () => {
    const form = render(
      CustomerForm()
    );
  
  act(() => {
    /* fire events that update state */
    fireEvent.click(form.getByRole("button"));
  });
  
  await waitFor(() => {
    const form = render(
      CustomerForm()
    );
    expect(form.baseElement.innerHTML).toContain("Required");
  });
  
  
  });

  it("should display must be 18 error", async () => {
  
    const form = render(
      CustomerForm()
    );

    act(() => {
      const dateInput = form.getByTestId('dateOfBirth');
      fireEvent.change(dateInput, { target: { value: '2222-01-01' }});
      /* fire events that update state */
      fireEvent.click(form.getByRole("button"));
    });
    
    await waitFor(() => {
      expect(form.baseElement.innerHTML).toContain("Required");
    });
    
    
    });
});
import * as React from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as moment from "moment";
import { FormLabel } from 'react-bootstrap';

function validateId(value: string) {
  let error;
  if (!value) {
    error = 'Required';
  } else if (!/[^\W_]$/i.test(value)) {
    error = 'Id must be alpha-numeric';
  }
  return error;
}

function validateName(value: string) {
  let error;
  if (!value) {
    error = 'Required';
  }
  else if (!/^[a-z]+$/i.test(value)) {
    error = 'First name must be alpha';
  }
  return error;
}

function validateLName(value: string) {
  let error;
  if (!value) {
    error = 'Required';
  }
  else if (!/^[a-z]+$/i.test(value)) {
    error = 'Last name must be alpha';
  }
  return error;
}

function validateDateOfBirth(value: moment.MomentInput) {
  let error: string;
  const yearsAgo = moment().diff(value, 'years', true); //with precision = true like 17.95 would not be 18
  const minimumAge = 18;

  if (!value) {
    error = 'Required';
    return error;
  }
  yearsAgo < minimumAge ? error = "Must be 18 years of age" : error = "";
  return error;
}

function post(state: { id: string; firstName: string; lastName: string; dateOfBirth: string; }){
  fetch("http://localhost:5000/api/CRMCustomer", {
    method: 'POST',
    headers: {
      'accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      id: state.id,
      firstName: state.firstName,
      lastName: state.lastName,
      dateOfBirth: state.dateOfBirth
    })
  })
    .catch(error => {
      console.log(error)
    });
}

export const CustomerForm = () =>  (
  
  <div>
    <h1>Customer Edit Form</h1>
    <Formik 
      initialValues={{
        id: '',
        firstName: '',
        lastName: '',
        dateOfBirth: '',
      }}
      onSubmit={(values) => {
        if(values.id !== '' && values.firstName !== '' 
        && values.lastName !== '' && values.dateOfBirth !== ''){
          post(values);
        }
      }}
    >
       {({ validateField, validateForm }) => (
        <Form data-cy="form">
          <FormLabel>Id</FormLabel>
          <Field name="id" data-cy="id" validate={validateId} />
          <ErrorMessage data-cy="errorForId" name='id' component='div' />
          <br/>
          <FormLabel>First Name</FormLabel>
          <Field name="firstName" data-cy="firstName" validate={validateName} />
          <ErrorMessage data-cy="errorForFirstName" name='firstName' component='div' />
          <br/>
          <FormLabel>Last Name</FormLabel>
          <Field name="lastName" data-cy="lastName" validate={validateLName} />
          <ErrorMessage data-cy="errorForLastName" name='lastName' component='div' />
          <br/>
          <FormLabel>Date Of Birth</FormLabel>
          <Field type="date" name="dateOfBirth" data-cy="dateOfBirth" validate={validateDateOfBirth} />
          <ErrorMessage data-cy="errorForDateOfBirth" name='dateOfBirth' component='div' />
          <br/>
          <button type="submit" data-cy="submit">Create / Update Customer</button>
        </Form>
        
      )}
    </Formik>
  </div>
 
);
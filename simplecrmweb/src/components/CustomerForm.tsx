import React, {useEffect} from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import moment from "moment";

function validateId(value) {
  let error;
  if (!value) {
    error = 'Id is Required';
  } else if (!/[^\W_]$/i.test(value)) {
    error = 'Id must be alpha-numeric';
  }
  return error;
}

function validateFName(value) {
  let error;
  if (!value) {
    error = 'Required';
  }
  else if (!/^[a-z]+$/i.test(value)) {
    error = 'First name must be alpha';
  }
  return error;
}

function validateLName(value) {
  let error;
  if (!value) {
    error = 'Required';
  }
  else if (!/^[a-z]+$/i.test(value)) {
    error = 'Last name must be alpha';
  }
  return error;
}

function validateDateOfBirth(value) {
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
  fetch("http://localhost:5222/api/CRMCustomer", {
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

export const CustomerForm = (props) =>  (
  
  <div>
    <h1>Signup</h1>
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
          console.log('here there');
          post(values);
        }
      }}
    >
       {()  => (
        <Form data-cy="form">
          <Field name="id" data-cy="id" validate={validateId} />
          <Field name="firstName" data-cy="firstName" validate={validateFName} />
          <Field name="lastName" data-cy="lastName" validate={validateLName} />
          <Field type="date" name="dateOfBirth" data-cy="dateOfBirth" validate={validateDateOfBirth} />
          <button type="submit" data-cy="submit">Create / Update Customer</button>
        </Form>
        
      )}
    </Formik>
  </div>
 
);
CustomerForm.displayName = "CustomerForm";
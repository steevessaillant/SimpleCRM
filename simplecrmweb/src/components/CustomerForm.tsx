import React from 'react';
import { Formik, Form, Field } from 'formik';
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
  let error;
  const yearsAgo = moment().diff(value, 'years', true); //with presion = true like 1.01
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
    .then(response => {
      //if (response.ok === true) {
      console.log(response)
      //setMessage("Customer, " + this.state.firstName + " " + this.state.lastName + " born on " + this.state.dateOfBirth + " was posted successfully!")
      //}
    })
    .catch(error => {
      // enter your logic for when there is an error (ex. error toast)
      console.log(error)
      //setMessage(error);
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
      onSubmit={values => {
        post(values);
      }}
    >
       {({ errors, touched, validateField, validateForm })  => (
        <Form data-cy="form">
          <Field name="id" data-cy="id" validate={validateId} />
          {errors.id && touched.firstName && <div>{errors.firstName}</div>}
          <Field name="firstName" data-cy="firstName" validate={validateFName} />
          {errors.firstName && touched.firstName && <div>{errors.firstName}</div>}
          <Field name="lastName" data-cy="lastName" validate={validateLName} />
          {errors.lastName && touched.lastName && <div>{errors.lastName}</div>}
          <Field type="date" name="dateOfBirth" data-cy="dateOfBirth" validate={validateDateOfBirth} />
          {errors.dateOfBirth && touched.dateOfBirth && <div>{errors.dateOfBirth}</div>}
          <button type="submit" data-cy="submit">Create / Update Customer</button>
          <div></div>
        </Form>
        
      )}
    </Formik>
  </div>
 
);
CustomerForm.displayName = "CustomerForm";
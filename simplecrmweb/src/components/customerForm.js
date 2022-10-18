import React, { useState, useRef } from "react";
import SimpleReactValidator from 'simple-react-validator';

function CustomerForm() {


  const [, forceUpdate] = useState()
  const validator = useRef(new SimpleReactValidator())


  const [id, setId] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [dateOfBirth, setDOB] = useState("");
  const [message, setMessage] = useState("");

  let handleSubmit = async (e) => {
    e.preventDefault();
    if (!validator.current.allValid()) {
      validator.current.showMessages();
      setMessage("Please fix the errors in the form")
      forceUpdate(1)      // rerender to show messages for the first time
    } else {
      fetch("http://localhost:5222/api/CRMCustomer", {
        method: 'POST',
        headers: {
          'accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          id: id,
          firstName: firstName,
          lastName: lastName,
          dateOfBirth: dateOfBirth
        })
      })
        .then(response => {
          if (response.ok === true) {
            setMessage("Customer, " + firstName + " " + lastName + " born on " + dateOfBirth + " was posted successfully!")
          }
        })
        .catch(error => {
          // enter your logic for when there is an error (ex. error toast)
          console.log(error)
          setMessage(error);
        });
      }
  }

  return (
    <div className="App">
      <form onSubmit={handleSubmit}>
        <input data-cy="id" required
          type="text"
          value={id}
          placeholder="Id"
          onChange={(e) => setId(e.target.value)}
        />
        {validator.current.message('id', id, 'required|alpha_num', { className: 'text-danger' })}
        <input data-cy="firstName" required
          type="text"
          value={firstName}
          placeholder="First Name"
          onChange={(e) => setFirstName(e.target.value)}
        />
        {validator.current.message('firstName', firstName, 'required|alpha', { className: 'text-danger' })}
        <input data-cy="lastName" required
          type="text"
          value={lastName}
          placeholder="Last Name"
          onChange={(e) => setLastName(e.target.value)}
        />
        {validator.current.message('lastName', lastName, 'required|alpha', { className: 'text-danger' })}
        <input data-cy="dob" required
          type="date"
          value={dateOfBirth}
          placeholder="DateOfBirth"
          onChange={(e) => setDOB(e.target.value)}
        />
        {validator.current.message('dateOfBirth', dateOfBirth, 'required|alpha_num_dash', { className: 'text-danger' })}
        <br />
        <button type="submit">Add new customer</button>

        <div className="message" data-cy="message">{message}</div>
      </form>
    </div>
  );
}

export default CustomerForm;
import React, { useState } from "react";
function CustomerForm() {

  const [id, setId] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [dateOfBirth, setDOB] = useState("");
  const [message, setMessage] = useState("");

  let handleSubmit = async (e) => {
    e.preventDefault();
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
        if(response.ok === true){
          setMessage("Customer, " + firstName + " " + lastName + " born on " + dateOfBirth +  " was posted successfully!")
        }
      })
      .catch(error => {
      // enter your logic for when there is an error (ex. error toast)
       console.log(error)
       setMessage(error);
      });  

}

return (
  <div className="App">
    <form onSubmit={handleSubmit}>
      <input data-cy="id" required="true"
        type="text"
        value={id}
        placeholder="Id"
        onChange={(e) => setId(e.target.value)}
      />
      <input data-cy="firstName"
        type="text"
        value={firstName}
        placeholder="First Name"
        onChange={(e) => setFirstName(e.target.value)}
      />
      <input data-cy="lastName"
        type="text"
        value={lastName}
        placeholder="Last Name"
        onChange={(e) => setLastName(e.target.value)}
      />
      <input data-cy="dob"
        type="date"
        value={dateOfBirth}
        placeholder="DateOfBirth"
        onChange={(e) => setDOB(e.target.value)}
      />
      <br/>
      <button type="submit">Add new customer</button>

      <div className="message">{message}</div> 
    </form>
  </div>
);
}

export default CustomerForm;
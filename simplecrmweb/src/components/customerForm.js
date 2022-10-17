import React, { useState } from "react";
function CustomerForm(props) {

  const [Id, setId] = useState("");
  const [FirstName, setFirstName] = useState("");
  const [LastName, setLastName] = useState("");
  const [DateOfBirth, setAge] = useState(props.minimumAge);
  const [message, setMessage] = useState("");

  let handleSubmit = async (e) => {
    e.preventDefault();
    try {
      let res = await fetch("http://localhost:5222/api/CRMCustomer", {
        method: "POST",
        mode: 'cors',
        headers: {
            'accept': 'application/json' ,
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin':  'http://localhost:8080',
            'Access-Control-Allow-Methods': 'POST',
            'Access-Control-Allow-Headers': 'Content-Type, Authorization'
          },
        body: JSON.stringify({
          id:Id,
          firstName: FirstName,
          lastName: LastName,
          age: DateOfBirth
        }),
      });

      if (res.status === 200) {
        setFirstName("");
        setLastName("");
        setAge("");
        setMessage("Customer with Id : " + Id + " created successfully");
      } else {
        setMessage("Some error occured" + resJson);
      }
    } catch (err) {
      console.log(err);
    }
  };

  return (
    <div className="App">
      <form onSubmit={handleSubmit}>
      <input data-cy="id"
          type="text"
          value={Id}
          placeholder="Id"
          onChange={(e) => setId(e.target.value)}
        />
        <input data-cy="firstName"
          type="text" 
          value={FirstName}
          placeholder="First Name"
          onChange={(e) => setFirstName(e.target.value)}
        />
        <input data-cy="lastName"
          type="text"
          value={LastName}
          placeholder="Last Name"
          onChange={(e) => setLastName(e.target.value)}
        />
        <input data-cy="age"
          type="number"
          value={DateOfBirth}
          placeholder="DateOfBirth"
          onChange={(e) => setAge(e.target.value)}
        />

        <button type="submit">Add new customer</button>

        <div className="message">{message ? <p>{message}</p> : null}</div>
      </form>
    </div>
  );
}

export default CustomerForm;
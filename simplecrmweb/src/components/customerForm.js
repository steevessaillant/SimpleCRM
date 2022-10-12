import Form from 'react-bootstrap/Form'
import Col from 'react-bootstrap/Col'
import Row from 'react-bootstrap/Row'
import React, { useState } from "react";
function CustomerForm(props) {

    const [customerInfo, setCustomerInfo] = useState({
        id: "",
        firstName: "",
        lastName: "",
        age: 18,
    })

    const handleChange = (event) => {
        debugger;
        setCustomerInfo({ ...customerInfo, [event.target.name]: event.target.value });
    }

    return (
        <div>
            <h1>Add a new customer</h1>
            <Form className="form-container" onChange={handleChange}>
                <Row>
                    <Col>
                        <Form.Control data-cy="id" placeholder="Id" value={customerInfo.id} onChange={handleChange} />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <Form.Control data-cy="firstName" placeholder="First name" value={customerInfo.firstName} onChange={handleChange} />
                    </Col>
                    <Col>
                        <Form.Control data-cy="lastName" placeholder="Last name" value={customerInfo.lastName} onChange={handleChange} />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <Form.Control data-cy="age" placeholder="Age" type='number' min={props.minimumAge} value={customerInfo.age} onChange={handleChange} />
                    </Col>
                    <Col>
                        <button type="submit">Add</button>
                    </Col>
                </Row>
            </Form>
        </div>
    );
}

export default CustomerForm;
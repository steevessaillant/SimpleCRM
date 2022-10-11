import Form from 'react-bootstrap/Form'
import Col from 'react-bootstrap/Col'
import Row from 'react-bootstrap/Row'
function CustomerForm() {

    return (
        <div>
            <h1>Add a new customer</h1>
            <Form>
                <Row>
                    <Col>
                        <Form.Control placeholder="Id" />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <Form.Control placeholder="First name" />
                    </Col>
                    <Col>
                        <Form.Control placeholder="Last name" />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <Form.Control data-cy="age" placeholder="Age" type='number' min={18} defaultValue={18} />
                    </Col>
                    <Col>
                        <Form.Control value="Submit" type='submit' />
                    </Col>
                </Row>
            </Form>
        </div>
    );
}

export default CustomerForm;
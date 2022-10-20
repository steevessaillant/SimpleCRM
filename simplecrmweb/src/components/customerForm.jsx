import React, { useState } from 'react';
import SimpleReactValidator from 'simple-react-validator';
import moment from 'moment';
import DatePicker from 'react-datepicker';
import "react-datepicker/dist/react-datepicker.css";

class CustomerForm extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      ajaxError: 'Please fix the errors in the form',
      dateOfBirth: moment().subtract(18, "years")._d
    }
    this.handleDateChange = this.handleDateChange.bind(this);
    this.validator = new SimpleReactValidator({
      // element: (message, className) => <div className="invalid-feedback d-block">{message}</div>,
      //locale: 'fr',
      autoForceUpdate: this,
      className: 'text-danger'
    });
  }



  submitForm() {
    if (this.validator.allValid()) {

      fetch("http://localhost:5222/api/CRMCustomer", {
        method: 'POST',
        headers: {
          'accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          id: this.state.id,
          firstName: this.state.firstName,
          lastName: this.state.lastName,
          dateOfBirth: this.state.dateOfBirth
        })
      })
        .then(response => {
          if (response.ok === true) {
            //setMessage("Customer, " + this.state.firstName + " " + this.state.lastName + " born on " + this.state.dateOfBirth + " was posted successfully!")
          }
        })
        .catch(error => {
          // enter your logic for when there is an error (ex. error toast)
          console.log(error)
          //setMessage(error);
        });
    } else {
      this.validator.showMessages();
    }
  }

  buildInputControl(name, value, type = 'text', rules = 'required|alpha') {
    value = value || this.state[name];
    rules = rules || name;
    return (
      <div className="col-sm-6 col-md-4">
        <div>{value}</div>
        <div className="form-group">
          <label>{name} : </label>
          <input className="form-control" type={type} data-cy={name} name={name}
           value={this.state[name]} onChange={this.handleInputChange.bind(this)} onBlur={() => this.validator.showMessageFor(name)}  />
          {this.validator.message(name, value, rules)}
        </div>
      </div>
    );
  }

  handleInputChange(event) {
    const target = event.target;
    const value = target.type === 'checkbox' ? target.checked : target.value;
    const name = target.name;
    this.setState({
      [name]: value
    });
  }

  handleDateChange = (date) => {
    this.setState({
      dateOfBirth: date
    })
  }
  render() {

    return (
      <div className="container card my-4">
        <div>{moment(this.state.dateOfBirth).format('MM/DD/YYYY')}</div>
        <div className="card-body">
          <h3>Add/Edit Customer</h3>
          <small className="text-muted">Click submit to view messages.</small>
          <hr />
          <div data-cy="form" className="row">
            {this.buildInputControl('id', this.state.id,'text','required|alpha_num')}
            {this.buildInputControl('firstName', this.state.firstName)}
            {this.buildInputControl('lastName', this.state.lastName)}
            <div className="col-sm-6 col-md-4">
              <div className="form-group">
                <label>Date of Birth : </label>
                <DatePicker data-cy='dateOfBirth' name='dateOfBirth'
                  selected={this.state.dateOfBirth}
                  onChange={this.handleDateChange}
                  dateFormat="MM/dd/yyyy"
                  maxDate={moment().subtract(18, "years")._d}
                  defaultValue={moment().subtract(18, "years")._d}
                  showYearDropdown
                  dateFormatCalendar="MMMM"
                  yearDropdownItemNumber={15}
                  scrollableYearDropdown
                />
              </div>
            </div>
          </div>


          {this.validator.messageWhenPresent(this.state.ajaxError, { element: message => <div className="alert alert-warning" role="alert" data-cy='message'>{message}</div> })}

          <button className="btn btn-primary" data-cy="submit" onClick={this.submitForm.bind(this)}>Add new customer</button>
        </div>
      </div>
    );
  }
}

export default CustomerForm;
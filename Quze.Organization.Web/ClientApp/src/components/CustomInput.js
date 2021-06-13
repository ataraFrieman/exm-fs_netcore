import React, { Component } from 'react';
import { isValidIsraeliID, isValidPhone } from '../helpers/User';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

export class CustomInput extends Component {

    constructor(props) {
        super(props);
        this.state = {
            validationError: this.props.validationError,
            value: (this.props.value)
        }
        this.displayPropsError = true
        this.inputBlur = this.inputBlur.bind(this);
        this.deleteInput = this.deleteInput.bind(this);
        this.inputChange = this.inputChange.bind(this);
        this.validation = this.validation.bind(this);
        this.inputRef = React.createRef();
        this.numberTypes = ["number", "identityNumber", "phone"];

    }

    inputChange(evt) {
        if (this.numberTypes.includes(this.props.type)) {
            var theEvent = evt;
            var value = theEvent.target.value;
            var regex = /[^0-9]/;
            if (regex.test(value)) {
                theEvent.target.value = theEvent.target.defaultValue;
            }
        }
        this.setState({ value: evt.target.value })
        if (this.props.onChange)
            this.props.onChange(evt);
    }

    inputBlur(e) {
        var event = e;
        event.target.value = event.target.value.trim();
        this.validation();
        if (e.target.defaultValue != e.target.value) {
            if (this.props.onChange)
                this.props.onChange(event);
        }

    }

    deleteInput(e) {
        this.inputRef.current.value = "";
        var event = {
            target: { value: "", name: this.props.name }
        };
        if (this.props.onChange)
            this.props.onChange(event);
        this.validation();

    }

    validation() {
        var value = this.inputRef.current.value;
        var validationError = "";
        if (value == "") {
            if (this.props.mandatory && this.props.mandatory == true)
                validationError = "שדה חובה";
        }
        else {
            switch (this.props.type) {
                case "number":
                    break;
                case "identityNumber":
                    validationError = isValidIsraeliID(value) ? "" : "מספר זהות שגוי";
                    break;
                case "phone":
                    validationError = isValidPhone(value) ? "" : "מספר טלפון שגוי";
                    break;

            }
        }
        this.setState({ validationError: validationError });
        if (validationError != "")
            return false;
        else
            return true;
    }

    render() {
        var value =  this.state.value;
        var propsArray = { ...this.props }
        delete propsArray["value"];
        delete propsArray["onChange"];
        return (
            <table className="col-12">
                <tbody>
                    <tr>
                        <td>
                            {
                                !this.inputRef.current || this.inputRef.current.value == ""
                                    || this.props.disabled == true ?
                                    "" :
                                    <span className="x" onClick={e => this.deleteInput(e)}>x</span>
                            }
                        </td>
                        <td>
                            <input className="form-control textInput" id={this.props.id} onChange={e => this.inputChange(e)} onBlur={e => this.inputBlur(e)} ref={this.inputRef}
                                value={value} {...propsArray}></input>
                        </td>
                        <td>
                            {this.props.iconName ? <FontAwesomeIcon icon={this.props.iconName} className="iconInput" /> : ""}
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td>
                            <span className="fs-12 text-danger">{this.props.validationError}</span>
                            <span className="fs-12 text-danger">{this.state.validationError}</span>
                        </td>
                    </tr>
                </tbody>
            </table>);

    }
}
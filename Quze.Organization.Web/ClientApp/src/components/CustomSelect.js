import React, { Component } from 'react';
import {  isValidIsraeliID, isValidPhone} from '../helpers/User';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { FormGroup, FormControl, Row } from "react-bootstrap";


export class CustomSelect extends Component {
    constructor(props) {
        super(props);
        this.state = {
            validationError: this.props.validationError

        }
        this.displayPropsError = true
        this.inputBlur = this.inputBlur.bind(this);
        this.getOption = this.getOption.bind(this);
        this.validation = this.validation.bind(this);
        this.value = 0;
    }

    inputBlur(e) {
        console.log("blur");
        var event = e;
        this.value = e.target.value;
        this.validation();
        
            if (this.props.onChange)
                this.props.onChange(event);

    }

    validation() {
        var validationError = "";
        if (!this.value || this.value == 0) {
            if (this.props.mandatory && this.props.mandatory == true)
                validationError = "שדה חובה";
        }

        this.setState({ validationError: validationError });
        if (validationError != "")
            return false;
        else
            return true;
    }

    getOption(options, name, value) {
        var optionsTags = [];
        optionsTags.push(<option value={0} >select</option>);

        for (var i = 0; i < options.length; i++) {
            optionsTags.push(<option value={options[i].id} {...options[i].id == value ? "selected" : "" } >{options[i][name]}</option>)
        }
    return<FormControl componentClass="select" placeholder="select" onBlur={e => this.inputBlur(e)} {...this.props} >{optionsTags}</FormControl>;

    }

    render() {
        return (
            <table className="col-12">
                <tbody>
                    <tr>
                        <td>
                            {this.getOption(this.props.options, this.props.optionName, this.props.value)}
                        </td>
                        <td>
                            <FontAwesomeIcon icon={this.props.iconName} className="iconInput" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span className="fs-12 text-danger">{this.props.validationError}</span>
                            <span className="fs-12 text-danger">{this.state.validationError}</span>
                        </td>
                    </tr>
                </tbody>
            </table>);

    }
}
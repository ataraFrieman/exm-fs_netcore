import React, { Component } from "react";
import { FormGroup, FormControl, Row } from "react-bootstrap";
import { Address } from "./address";
import { CheckBoxesList } from './CheckBoxesList';
import { CustomInput } from './CustomInput';
import { CustomSelect } from "./CustomSelect";


export class FormInputs extends Component {
    constructor(props) {
        super(props);
        this.state = {
            branch: "13"
        };
        this.renderType = this.renderType.bind(this);
        this.inputRef = React.createRef();

    }

    renderType(i) {
        if (this.props.proprieties[i].type == "select")
            return (<CustomSelect {...this.props.proprieties[i]} ref={this.inputRef}/>)
        if (this.props.proprieties[i].type == "address")
            return (<Address {...this.props.proprieties[i]} />);
        if (this.props.proprieties[i].type == "checkBoxesList")
            return (<CheckBoxesList {...this.props.proprieties[i]} />);
        else 
            return (<CustomInput {...this.props.proprieties[i]} ref={this.inputRef}/>);
    }

    render() {
        var row = [];
        for (var i = 0; i < this.props.ncols.length; i++) {
            row.push(
                <div key={i} className={this.props.ncols[i]}>
                    <FormGroup>
                        {/* <FormControl></FormControl> instead <ControlLabel></ControlLabel> in the new version */}
                        <span>{this.props.proprieties[i].label}</span>
                        {this.renderType(i)}
                    </FormGroup>
                </div>
            );
        }
        return <Row>{row}</Row>;
    }
}

export default FormInputs;

import React, { Component } from 'react'

export class Input extends Component {
    constructor(props) {
        super(props);
        this.state = {
            // validationError: this.props.error || "",
            value: this.props.value
            // errorOccured: this.props.errorOccured || function () { }
        }
        this.inputChange = this.inputChange.bind(this);


    }

    inputChange(e) {
        this.setState({ value: e.target.value })
        if (this.props.onChange)
            this.props.onChange(e);
    }

    render() {
        var style = this.props.style
        var color = style.color ? style.color : ""
        var margin = style.margin ? style.margin : ""
        return (
            <div className={this.props.divClassName}>
                <label className="mb-0">
                    <span style={{color, margin}}>{this.props.span}</span>
                    {this.props.label}
                </label>
                <input type={this.props.type}
                    id={this.props.id}
                    className={this.props.className}
                    autoComplete={this.props.autoComplete}
                    value={this.props.value}
                    onChange={e => this.inputChange(e)} />
            </div>
        )
    }
}

export default Input

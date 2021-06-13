import React, { Component } from 'react';
import "../css/inputStyle.css"

export class ReportedDelay extends Component {

    constructor(props) {
        super(props);

        this.state = {
        };
        this.setNumDelay = this.setNumDelay.bind(this);
        this.updateDelayAppointment = this.updateDelayAppointment.bind(this);
    }

    setNumDelay(e) {
        this.props.appointment.operation.delay = parseInt(e.target.value)
    }
    updateDelayAppointment(app)
    {
        this.props.updateDelayApointment(this.props.appointment)
    }
    appointment
    render() {
        return (
            <div className='col'>
                <div className="row justify-content-center">
                    <input className="row inputInerModal" type="number"
                        placeholder="Several minutes delay"
                        onChange={this.setNumDelay}
                        min={0}
                    ></input>
                </div>
                <br></br>
                <div className="row justify-content-center">
                    <input className="row inputInerModal" type="text" placeholder="Cause of delay"></input>
                </div>
                <div className="row modal-footer justify-content-end m-0 p-0 mt-2">
                    <button className="btn btn-outline-primary font-pr m-2" aria-hidden="true"
                        onClick={()=>{this.updateDelayAppointment(this.props.appointment)}}>
                        save
                        </button>
                </div>
            </div>
        );
    }
}
export default ReportedDelay;



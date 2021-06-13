import React, { Component } from "react";
import { getTime, getDate } from '../helpers/TimeService';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';



export class CurrentAppointment extends Component {
    render() {
        return (
            <tr id={this.props.id} key={this.props.id} className="h-2" tabindex="0" >
                <td className="timeInCard">{getTime(this.props.appointment.beginTime)}</td>
                <td className="mainTitleInCard">{this.props.appointment.fellow ? this.props.appointment.fellow.firstName + ' ' + this.props.appointment.fellow.lastName : ''}</td>

                <td className=""><FontAwesomeIcon className="fc-green" icon="circle"/></td>
            </tr>
        );
    }
}

export default CurrentAppointment;


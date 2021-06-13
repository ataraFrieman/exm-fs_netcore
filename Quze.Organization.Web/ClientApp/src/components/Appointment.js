
﻿import React, { Component } from "react";
import { Grid, Row, Col, Table, Tab, Tabs } from "react-bootstrap";
import Card from "./Card";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Selectable } from './Calendar';
// import { ServiceTypesCards } from './ServiceType';
import { ServiceQGraphs } from './ServiceQGraphs';
import * as http from '../helpers/Http'
import history from '../helpers/History';
import "../css/site.css";
import { getTime, getDate } from '../helpers/TimeService';


export class Appointment extends Component {
    render() {
        return (
            <tr id={this.props.id} key={this.props.id} className="h-2" tabIndex="0">
                <td className="p-1">{getTime(this.props.appointment.beginTime)}</td>
                <td className="p-1 ">{this.props.appointment.fellow ? this.props.appointment.fellow.firstName + ' ' + this.props.appointment.fellow.lastName : ''}</td>

                <td className="p-1">{this.props.appointment.served ? 
                    <i className='pe-7s-check text-success fs-16 font-weight-bold' /> 
                    : 
                    <FontAwesomeIcon className={ this.props.appointment.noShow > 0 ?
                        (this.props.appointment.noShow < 5 ? 
                        "fc-yellow" 
                        : 
                            this.props.appointment.noShow < 8 ?
                            "text-warning" 
                            :
                            "text-danger") 
                        : 
                        "text-muted"
                    } 
                    icon="circle" />
                   }
                </td>
            </tr>
        );
    }
}

export default Appointment;


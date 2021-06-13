import React, { Component } from 'react';
import { Grid, Row, Col, Table, Tab, Tabs } from "react-bootstrap";
import Card from "./Card";
import { Selectable } from './Calendar';
// import { ServiceTypesCards } from './ServiceType';
import { CurrentAppointment } from './CurrentAppointment';
import { Appointment } from './Appointment';
import { ServiceQGraphs } from './ServiceQGraphs';
import history from '../helpers/History';
import "../css/site.css";


export class Appointments extends Component {
    displayName = "Appointments"
    constructor(props) {
        super(props);
        console.log(props);
        this.state = {
            selectedQ: props.selectedQ
        };
        this.renderGridDisplay = this.renderGridDisplay.bind(this);

    }


    secondsToHms(d) {
        d = Number(d);

        var h = Math.floor(d / 3600);
        var m = Math.floor(d % 3600 / 60);
        var s = Math.floor(d % 3600 % 60);

        return ('0' + h).slice(-2) + ":" + ('0' + m).slice(-2) + ":" + ('0' + s).slice(-2);
    }


    renderGridDisplay(Q, isCurrent, isPassed, spName, appointments) {
        return (

            <Table className="" striped hover >
                <thead >
                </thead >
                <tbody >
                    {appointments.map(item =>
                        item.id == this.props.selectedQ.currentAppointementId ?
                            <CurrentAppointment key={item.id} id={item.id} appointment={item}/> :
                            <Appointment key={item.id} id={item.id} appointment={item} />
                                    
                    )}
                </tbody>
            </Table>

        );
    }

    render() {
        var Q = this.props.selectedQ;
        if (!Q)
            return "";
        var SP = Q.serviceProvider;
        var appointments = this.props.selectedQ.appointments ? this.props.selectedQ.appointments : [];
        var spName = SP?((SP.title ? SP.title : "") + " " + SP.firstName + " " + SP.lastName):"";
        var isPassed = Q.passed == true;
        var isCurrent = Q.currentAppointement && Q.currentAppointement.id;
        var display = this.renderGridDisplay(Q, isCurrent, isPassed, spName, appointments);
        //var stList = Q.appointments.map(e => e.serviceType);
        //var STList = stList.filter((obj, pos, arr) => { return arr.map(mapObj => mapObj["id"]).indexOf(obj["id"]) === pos; });
        //TODO:display service type and now get service num:
        return (<div className="">
                    {display}
                </div>
                );
    }
}

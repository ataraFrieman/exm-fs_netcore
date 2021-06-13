import React, { Component } from 'react';
import { Grid, Row, Col, Table, Tab, Tabs } from "react-bootstrap";
import Card from "./Card";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Selectable } from './Calendar';
import { SchedulePlanning } from './SchedulePlanning';
import { ServiceTypesCards } from './ServiceType';
import { ServiceQGraphs } from './ServiceQGraphs';
// import HeaderPage from './HeaderPage'
import * as http from '../helpers/Http';

export class ServiceProvider extends Component {
    displayName = "ServiceProvider"
    constructor(props) {
        super(props);
        console.log(props);
        this.state = {
            serviceProvider: props.location.state.serviceProvider, 
            activeTabIndex: 3,
            serviceQueue: [],
            serviceTypes: []
        };
        //var dt = new Date('December 30, 2018 00:00:00');

       // this.state.serviceProvider.serviceProvidersServiceTypes.map(item => this.state.serviceTypes.push(item.serviceType));

    }
    componentWillMount() {

    }

    handleClickTab(event, index, thisObj) {
        event.stopPropagation();
        event.nativeEvent.stopImmediatePropagation();
        thisObj.setState({ activeTabIndex: index })
    }

    render() {
        
        return <div className="">
        
            {/* <HeaderPage /> */}

            <div className="row">
                <span className="col-6 text-right pt-3 mr-5">   {this.state.serviceProvider.title + " " +
                    this.state.serviceProvider.firstName + " "
                    + this.state.serviceProvider.lastName}
                </span>
            </div>


            <ul className="nav nav-tabs" id="myTab" role="tablist">
                <li className="nav-item">
                    <a className={"nav-link " + (this.state.activeTabIndex == 3 ? " active " : "")}
                        id="contact-tab" data-toggle="myTabContent" onClick={(event) => { this.handleClickTab(event, 3, this) }} role="tab" aria-controls="contact" aria-selected="false">סטטיסטיקות וגרפים</a>
                </li>
                <li className="nav-item">
                    <a className={"nav-link " + (this.state.activeTabIndex == 0 ? " active " : "")} id="home-tab" data-toggle="myTabContent" onClick={(event) => { this.handleClickTab(event, 0, this) }} role="tab" aria-controls="home" aria-selected="true" serviceQueue={this.state.serviceQueue}>לוח זמנים</a>
                </li>
                <li className="nav-item">
                    <a className={"nav-link " + (this.state.activeTabIndex == 1 ? " active " : "")} id="home-tab" data-toggle="myTabContent" onClick={(event) => { this.handleClickTab(event, 1, this) }} role="tab" aria-controls="home" aria-selected="true">לוח זמנים עקרוני</a>
                </li>
                <li className="nav-item">
                    <a className={"nav-link " + (this.state.activeTabIndex == 2 ? " active " : "")}
                        id="profile-tab" data-toggle="myTabContent" onClick={(event) => { this.handleClickTab(event, 2, this) }} role="tab" aria-controls="profile" aria-selected="false">סוגי שרות</a>
                </li>

            </ul>
            <div className="tab-content h-75" id="myTabContent" >
                {this.state.activeTabIndex == 0 ? <div className="tab-pane fade active show" role="tabpanel" ><Selectable id="Selectable" serviceQueue={this.state.serviceQueue} branch={0} serviceProviderId={this.state.serviceProvider.id} serviceProviderTitledFullName={this.state.serviceProvider.titledFullName} timeTable={false} history={this.props.history} /></div> : ""}
                {this.state.activeTabIndex == 1 ? <div className="tab-pane fade active show" role="tabpanel" ><SchedulePlanning id="Selectable" serviceQueue={this.state.serviceQueue} branch={0} branches={this.props.location.state.branches} serviceProvider={this.state.serviceProvider} serviceProviderId={this.state.serviceProvider.id} serviceProviderTitledFullName={this.state.serviceProvider.titledFullName} /></div> : ""}
                {this.state.activeTabIndex == 2 ? <div className="tab-pane fade active show" role="tabpanel" >
                    <ServiceTypesCards serviceTypes={this.state.serviceTypes} />
                </div> : ""}
                {this.state.activeTabIndex == 3 ? <div className="tab-pane fade active show" role="tabpanel" ><ServiceQGraphs /></div> : ""}
            </div>

        </div>;
    }
}

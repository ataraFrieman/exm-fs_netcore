import React, { Component } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import 'react-bootstrap-table2-paginator/dist/react-bootstrap-table2-paginator.min.css';
import '../css/general.css';
import { Selectable } from './Calendar';
import { ServiceTypesCards } from './ServiceType';
import ServiceProvidersList from './ServiceProviders';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Modal } from './Modal';
import { ServiceQGraphs } from './ServiceQGraphs';
import * as $ from 'jquery';
import * as http from '../helpers/Http'
import { getBranchModalData } from "../helpers/ModalData"



export class Branch extends Component {
    displayName = Branch.name

    constructor(props) {
        super(props);
        this.state = {
            branch: this.props.location.state.input.branch,
            serviceType: [],
            loadingServiceType: true,
            serviceProviders: this.props.location.state.input.serviceProviders,
            organizationName: this.props.location.state.input.organizationName,


            streets: this.props.location.state.input.streets,
            cities: this.props.location.state.input.cities,
            activeTabIndex: 4
        };
        console.log(this.state.serviceProviders);
        this.renderHeader = this.renderHeader.bind(this);
        this.renderTabs = this.renderTabs.bind(this);
        this.handleClickTab = this.handleClickTab.bind(this);
        this.onSave = this.onSave.bind(this);
        this.saveEvent = this.saveEvent.bind(this);
        this.cityChanged = this.cityChanged.bind(this);
        this.streetChanged = this.streetChanged.bind(this);



        //fetch('api/Branches/GetServiceType?id=' + this.state.branch..id)
        //    .then(response => response.json())
        //    .then(data => {
        //        this.setState({ serviceType: data, loadingServiceType: false });
        //    });

        //http.get('api/Branches/GetServiceType?id=' + this.state.branch.id)
        //    .then(function (response) {
        //        if (!response || !response.length)
        //            return;

        //        this.setState({
        //            serviceType: response,
        //            loadingServiceType: response
        //        });
        //    });


    }

    componentWillMount() {
        this.setState({
            serviceProviders:
                this.state.serviceProviders.filter(s => this.state.branch.serviceProvidersBranches.filter(spb => spb.serviceProviderId == s.id).length > 0)
        });

    }
    onSave() {

    }
    cityChanged() {


    }

    streetChanged() {


    }




    saveEvent(e, item) {
        console.log(item);
        $('#editBranch' + this.state.branch.id).modal('hide');
        this.setState({ branch: item });
        console.log(item);
        console.log(this.state.branch);
        http.put('api/Branches', item, null);
    }

    handleClickTab(event, index, thisObj) {
        event.stopPropagation();
        event.nativeEvent.stopImmediatePropagation();
        thisObj.setState({ activeTabIndex: index })
    }

    renderHeader() {
        var modalData = getBranchModalData(this.state.branch, this.state.streets, this.state.cities, this.saveEvent);
        return (
            <div className="row">
                <div className="col-4">
                    <div className="row">
                        <h6 className="mr-3 col-12 mt-3 mb-2 text-muted text-right fs-16 ">
                            <div >
                                <span className="p-1 text-right" data-toggle="modal" data-target={"#editBranch" + this.state.branch.id}>
                                    <i id="open" className={"icon fs-24 pe-7s-note text-right"} data-toggle="tooltip" title="עריכה" />
                                </span>
                                <span >{this.state.branch.name}</span>
                                {modalData ? <Modal item={this.state.branch} saveEvent={this.saveEvent} modalId={"editBranch" + this.state.branch.id} showButton={false} formInputsData={modalData}></Modal> : ""}

                            </div>
                        </h6>
                    </div>
                    <div className="row">
                        <h6 className="mr-5 col-12 mb-3 text-muted text-right fs-16 ">{this.state.branch.street.name} {this.state.branch.houseNumber} {this.state.branch.street.city.name}</h6>
                    </div>
                </div>

                <div className="col-6">
                    <div className="row">
                        <h6 className="col-12 mt-3 mb-2 mr-3 text-muted text-right fs-16  basicColorBlue">טלפון: {this.state.branch.phonNumber}</h6>
                    </div>
                    <div className="row">
                        <h6 className="col-12 mb-3 text-muted text-right fs-16  basicColorBlue">כתובת מייל: {this.state.branch.emailAddress}</h6>
                    </div>
                </div>

            </div>
        );
    }

    //getSpst() {
    //    var spst = [];
    //    this.state.serviceType.map((item) => {
    //        var serviceType = { serviceType: item, minimalKitRules: [] };
    //        spst.push(serviceType)
    //    });
    //    return spst;
    //}
    renderTabs() {
        var thisObj = this;
        var st = [];
        return (
            <div>
                <ul className="nav nav-tabs" id="myTab" role="tablist">
                    <li className="nav-item" >
                        <a className={"nav-link " + (this.state.activeTabIndex == 4 ? " active " : "")}
                            onClick={(event) => { this.handleClickTab(event, 4, this) }} role="tab">
                            סטטיסטיקות וגרפים</a>
                    </li>
                    <li className="nav-item">
                        <a className={"nav-link " + (this.state.activeTabIndex == 0 ? " active " : "")} id="home-tab" data-toggle="myTabContent" onClick={(event) => { this.handleClickTab(event, 0, this) }} role="tab" aria-controls="home" aria-selected="true"> לוח זמנים עקרוני</a>
                    </li>
                    <li className="nav-item">
                        <a className={"nav-link " + (this.state.activeTabIndex == 1 ? " active " : "")} id="home-tab" data-toggle="myTabContent" onClick={(event) => { this.handleClickTab(event, 1, this) }} role="tab" aria-controls="home" aria-selected="true">לוח זמנים</a>
                    </li>
                    <li class="nav-item">
                        <a className={"nav-link " + (this.state.activeTabIndex == 2 ? " active " : "")}
                            id="profile-tab" data-toggle="myTabContent" onClick={(event) => { this.handleClickTab(event, 2, this) }} role="tab" aria-controls="profile" aria-selected="false">סוגי שרות</a>
                    </li>
                    <li className="nav-item">
                        <a className={"nav-link " + (this.state.activeTabIndex == 3 ? " active " : "")}
                            id="contact-tab" data-toggle="myTabContent" onClick={(event) => { this.handleClickTab(event, 3, this) }} role="tab" aria-controls="contact" aria-selected="false">נותני שרות</a>
                    </li>

                </ul>
                <div className="tab-content h-75" id="myTabContent" >
                    {this.state.activeTabIndex == 0 ? <div className="tab-pane fade active show" role="tabpanel" > <Selectable id="Selectable" /></div> : ""}
                    {this.state.activeTabIndex == 1 ? <div className="tab-pane fade active show" role="tabpanel" > <Selectable id="Selectable" /></div> : ""}
                    {this.state.activeTabIndex == 2 ? <div className="tab-pane fade active show" role="tabpanel" >
                        <ServiceTypesCards serviceTypes={st} />
                    </div> : ""}
                    {this.state.activeTabIndex == 3 ? <div className="tab-pane fade active show" role="tabpanel" ><ServiceProvidersList serviceProviders={this.state.serviceProviders} organizationName="123..." /></div> : ""}
                    {this.state.activeTabIndex == 4 ?
                        <div className="tab-pane fade active show" >
                            <ServiceQGraphs selectedQ={this.props.location.state.input.QueuesList[1]} queuesList={this.props.location.state.input.QueuesList} />
                        </div> : ""}
                </div>
            </div >)
    }

    render() {
        let contents = this.state.loading || this.state.loadingAddress
            ? <p><em>Loading...</em></p>
            : this.renderHeader();
        let tabs = this.renderTabs();

        return (
            <div>
                {contents}
                {tabs}
            </div>
        );
    }
}



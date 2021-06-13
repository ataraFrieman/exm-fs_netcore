import React, { Component } from 'react';
import { Grid, Row, Col, Table } from "react-bootstrap";
import Card from "./Card";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import history from '../helpers/History';
import { getServiceProviderModalData } from "../helpers/ModalData";
import { Modal } from './Modal';
import { CheckBoxesList } from './CheckBoxesList';
import * as http from '../helpers/Http';
import * as $ from 'jquery';
import { observer } from 'mobx-react';
import Users from './Users';
import { withI18n } from "react-i18next";
 class ServiceProvidersList extends Component {
    displayName = "ServiceProviders"
    constructor(props) {
        super(props);
        console.log(props);
        this.state = {
            serviceProviders: props.serviceProviders?props.serviceProviders:[],
            filteredServiceProviders: [],
            organizationName: "",
            newServiceProvider: null,
            modalData: null,
            showModal:false

        };
        this.renderNav = this.renderNav.bind(this);
        this.getEmptyServiceProvider = this.getEmptyServiceProvider.bind(this);
        this.saveEvent = this.saveEvent.bind(this);
        this.updateEvent = this.updateEvent.bind(this);
        this.filterAll = this.filterAll.bind(this);

    }
    componentWillMount() {
        if (this.props.serviceProviders && this.props.serviceProviders.length)
            this.setState({ serviceProviders: this.props.serviceProviders, filteredServiceProviders: this.props.serviceProviders, organizationName: this.props.organizationName });

        this.setState({
            newServiceProvider: this.getEmptyServiceProvider(),
            modalData: getServiceProviderModalData(this.getEmptyServiceProvider(), this.props.serviceTypes,this.props.t)
        });
    }

    getEmptyServiceProvider() {
        var emtpyServiceProvider = { id: 0, organizationId: this.props.organizationId, identityNumber: "", firstName: "", lastName: "", serviceProvidersServiceTypes: [] };
        return emtpyServiceProvider;

    }

    saveEvent(e, newServiceProvider) {
        console.log(newServiceProvider);
        $('#newServiceProvider0').modal('hide');
        this.setState({
            newServiceProvider: this.getEmptyServiceProvider(),
            modalData: getServiceProviderModalData(this.getEmptyServiceProvider(), this.props.serviceTypes,this.props.t)
        });
        http.post('api/Organization/CreateServiceProvider', newServiceProvider).then(data => {
            console.log(data);
            newServiceProvider = data;
            var serviceProviders = this.state.serviceProviders;
            serviceProviders.push(newServiceProvider);
            this.setState({ serviceProviders: serviceProviders });
            var e = { target: { value: $('#filterAllServiceProviders').val() } };
            this.filterAll(e);
        });

    }

    updateEvent(newServiceProvider) {
        console.log(newServiceProvider);       
        this.setState({
            newServiceProvider: newServiceProvider,
            modalData: getServiceProviderModalData(newServiceProvider, this.props.serviceTypes,this.props.t)
        });
    }

    filterAll(e) {
        this.setState({
            filteredServiceProviders: this.state.serviceProviders
        });
        if (e.target.value != "") {
            var filteredServiceProviders = this.state.serviceProviders.filter(
                b => {
                    var serviceType = !b.serviceProvidersServiceTypes && !b.serviceProvidersServiceTypes.length ? "" :
                        b.serviceProvidersServiceTypes.map(st => {
                            return st.serviceType.parentServiceId == 1 ? st.serviceType.description + " " : "";
                        });
                    serviceType = serviceType.join("").trim();
                    return (b.titledFullName && b.titledFullName.includes(e.target.value)) ||
                        (b.identityNumber && b.identityNumber.includes(e.target.value)) ||
                        serviceType.includes(e.target.value);
                }
            );
            this.setState({ filteredServiceProviders: filteredServiceProviders });
        }
    }

    renderNav() {
        var t = this.props.t;
        return (
            <row>
                <div class="row">
                    <row>
                        <nav className="col-12 navbar p-0 justify-content-start">
                            <span className="mh-20px" data-toggle="modal" data-target={"#newServiceProvider0"}>
                           
                            
                                <i id="open" className={"icon fs-24 pe-7s-plus p-0"} data-toggle="tooltip" title={t("ServiceProvidersList.Newservice")} />
                            </span>
                            {this.state.modalData && this.state.showModal ? <Modal t={this.props.t} item={this.state.newServiceProvider} saveEvent={this.saveEvent} updateEvent={this.updateEvent} modalId={"newServiceProvider0"} showButton={false} formInputsData={this.state.modalData}></Modal> : ""}
                            <input type="text" id="filterAllServiceProviders" onChange={e => this.filterAll(e)} placeholder={t("ServiceProvidersList.Search")} />
                        </nav>
                    </row>

                </div>
            </row>)
    }

     render() {
         return <Users t={this.props.t} type={1} serviceProviders={this.state.serviceProviders} />;
     }

}
export default withI18n()(observer(ServiceProvidersList));
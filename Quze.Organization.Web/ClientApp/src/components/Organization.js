import React, { Component } from 'react';
import * as http from "../helpers/Http";
import { Grid, Row, Col } from "react-bootstrap";
import { StatsCard } from "./StatsCard"
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import Branches from './Branches';
import { Branch } from './Branch';
import  FellowsList  from './Fellows';
import  ServiceProvidersList  from './ServiceProviders';
import { QueuesList } from './QueuesList';
import QueuesComponent  from './QueueComponent';
import history from '../helpers/History';
import { getOrganizationId } from '../helpers/AccountService';
import { ServiceTypesCards } from './ServiceType';
import GlobalSearch from '../components/task/AutoCompleteSearch';
import Search from '../components/Autocomplete';
import HeaderTitlePage from './HeaderTitlePage';


export class Organizations extends Component {
    displayName = Organizations.name;
    constructor(props) {
        super(props);
        this.state = {
            organizationId: "",
            organizationName: "",
            branches: [],
            serviceProviders: [],
            serviceTypes: [],
            fellows: [],
            selectedQ:[],
            QueuesList: null,
            activeTabIndex: 1
        };
    }
    componentWillMount() {
        this.loadOrganizationDatails();
    }
    loadOrganizationDatails() {
        var organizationId = getOrganizationId();
        if (!organizationId) {
           {
                this.setState({ noOrganizationId: true });
                return;
            }
        }
        var thisObj = this;
        http.get('api/Organization?organizationId=' + organizationId)
            .then(function (response) {
                console.log(response);
                if (!response || !response.entity || !response.entity.id)
                    return;
                let organization = response.entity;
                thisObj.setState({
                    organizationId: organizationId
                    , organizationName: organization.name
                    , branches: organization.branches
                    , fellows: organization.fellows
                    , serviceProviders: organization.serviceProviders
                    , serviceTypes: organization.serviceTypes
                    , serviceQueues: organization.serviceQueues
                });
            });
        http.get('api/qManagament?organizatonId=' + organizationId)
            .then(function (response) {
                if (!response || !response.length)
                    return;
                response.sort(function (a, b) {
                    return new Date(b.actualBeginTime) - new Date(a.actualBeginTime);
                });

                thisObj.setState({
                    selectedQ: response[0],
                    QueuesList: response
                });
            });
    }
    
    render() {
        var t = this.props.t;
        return (
            /* יש בעיה עם העיצוב שתןצאןת החיפוש לא עולות על האלמנטים בדף הזה */ 
            //dir="rtl" 
            <div className="">
                    {/* d-flex flex-column col-8 col-sm-7 */}

                        <HeaderTitlePage title={t("organization.title")}/>
                {/* <Search /> */}
                {/* <Row className="m-0 ">
                    <span className="text-right fs-24 col-12"> {this.state.organizationName}</span>
                </Row> */}
                <Row className="m-2 ">
                    <Col lg={2} sm={6} onClick={e => this.setState({ activeTabIndex: 1 })}>
                        <StatsCard
                            bigIcon={<FontAwesomeIcon icon="hospital" />}
                            statsText={t("organization.Branches")}
                            statsValue={this.state.branches ? this.state.branches.length : 0}
                            active={this.state.activeTabIndex === 1}
                        />
                    </Col>
                    <Col lg={2} sm={6} onClick={e => this.setState({ activeTabIndex: 2 })}>
                        <StatsCard
                            bigIcon={<FontAwesomeIcon icon="user-md" />}
                            statsText={t("organization.doctors")}
                            statsValue={this.state.serviceProviders ? this.state.serviceProviders.length : 0}
                            active={this.state.activeTabIndex === 2}
                        />
                    </Col>
                    <Col lg={2} sm={6} onClick={e => this.setState({ activeTabIndex: 3 })}>
                        <StatsCard
                            bigIcon={<FontAwesomeIcon icon="user-circle" />}
                            statsText={t("organization.Colleagues")}
                            statsValue={this.state.fellows ? this.state.fellows.length : 0}
                            active={this.state.activeTabIndex === 3}
                        />
                    </Col>
                    <Col lg={2} sm={6} onClick={e => this.setState({ activeTabIndex: 4 })}>
                        <StatsCard
                            bigIcon={<FontAwesomeIcon icon="users" />}
                            statsText={t("organization.Queues")}
                            statsValue={this.state.serviceQueues ? this.state.serviceQueues.length : 0}
                            active={this.state.activeTabIndex === 4}
                        />
                    </Col>
                    <Col lg={2} sm={6} onClick={e => this.setState({ activeTabIndex: 5 })}>
                        <StatsCard
                            bigIcon={<FontAwesomeIcon icon="users" />}
                            statsText={t("organization.TypesOfService")}//"סוגי שרות"
                            statsValue={this.state.serviceTypes ? this.state.serviceTypes.length : 0}
                            active={this.state.activeTabIndex === 5}
                        />
                    </Col>
                </Row>
                <div className="row m-0">
                    {this.state.activeTabIndex === 1 && this.state.branches && this.state.branches.length ? <Branches branches={this.state.branches} serviceProviders={this.state.serviceProviders} organizationName={this.state.organizationName} organizationId={this.state.organizationId} QueuesList={this.state.QueuesList} selectedQ={this.state.selectedQ} history={this.props.history} /> : ""}
                    {this.state.activeTabIndex === 2 ? <ServiceProvidersList serviceProviders={this.state.serviceProviders} organizationName={this.state.organizationName} organizationId={this.state.organizationId} serviceTypes={this.state.serviceTypes} branches={this.state.branches} history={this.props.history} /> : ""}
                    {this.state.activeTabIndex === 3 ? <FellowsList fellows={this.state.fellows} organizationName={this.state.organizationName} organizationId={this.state.organizationId} /> : ""}
                    {this.state.activeTabIndex === 4 && this.state.QueuesList && this.state.QueuesList.length ? <QueuesComponent organizationName={this.state.organizationName} organizationId={this.state.organizationId} QueuesList={this.state.QueuesList} selectedQ={this.state.selectedQ} /> : ""}
                    {this.state.activeTabIndex === 5 && this.state.serviceTypes && this.state.serviceTypes.length ? <ServiceTypesCards serviceTypes={this.state.serviceTypes} organizationId={this.state.organizationId} /> : ""}
                   
                </div>
            </div>
        );
    }
}

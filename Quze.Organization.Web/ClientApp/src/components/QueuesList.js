import React, { Component } from 'react';
import { Grid, Row, Col, Table, Tab, Tabs } from "react-bootstrap";
import Card from "./Card";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Selectable } from './Calendar';
// import { ServiceTypesCards } from './ServiceType';
import { ServiceQGraphs } from './ServiceQGraphs';
import * as http from '../helpers/Http';
import history from '../helpers/History';
import * as moment from 'moment';
import 'moment/locale/pt-br';
import { getTime } from '../helpers/TimeService';

export class QueuesList extends Component {
    displayName = "ServiceProviders"
    constructor(props) {
        super(props);
        console.log(props);
        this.state = {
            selectedQ: null,
            QueuesList: props.queuesList ? props.queuesList : []
        };

    }


    render() {
        var t=this.props.t;
        if (!this.state.QueuesList || !this.state.QueuesList.length)
            return <span>{t("QueuesL.load")}</span>;
        return <div className="">
            <div className="row m-0">
                {this.state.QueuesList.map((item) => {
                    console.log(item);
                    return item.id ?
                     <div className="card col-3 m-3 ml-0 p-0" key={item.id}>
                        <div className="card-body font-pr text-align" dir="ltr">
                            <div className="row justify-content-end">
                                <div className="p-1" onClick={(e) => { this.props.openCurrentQ(item, 2) }}><i className="fs-24 pe-7s-note icon"></i></div>
                                <div className="p-1" onClick={(e) => { this.props.openCurrentQ(item, 2) }}><i className="fs-24 pe-7s-graph2 icon"></i></div>
                            </div>
                            <h6 className="card-subtitle mb-2 mr-4 text-muted text-left fs-16  basicColorBlue">{t("QueuesL.ServiceT")}: {item.serviceType ? item.serviceType.description : " "} </h6>
                                <h6 className="card-subtitle mb-2 mr-4 text-muted text-left fs-16  basicColorBlue">{t("QueuesL.ServiceProvider")}: {item.serviceProvider ? item.serviceProvider.title : ""}
                                    {item.serviceProvider?item.serviceProvider.fullName:""} </h6>
                            <h6 className="card-subtitle mb-2 mr-4 text-muted text-left fs-16  basicColorBlue">{t("QueuesL.Branch")}: {item.branch ? item.branch.name : "?"}</h6>
                            <h6 className="card-subtitle mb-2 mr-4 text-muted text-left fs-16  basicColorBlue ">{t("QueuesL.Date")}: {new Date(item.beginTime).toLocaleDateString()} </h6>

                            <div className="row mt-4">
                                <div className=" col-2 p-0 ">
                                    <i className="fs-24 pe-7s-clock" />
                                </div>
                                <span className=" col-10 p-0 text-left text-muted fs-12">

                                    {getTime(item.endTime)} - {getTime(item.beginTime)}  </span>
                            </div>
                            <div className="row">
                                <div className=" col-2 p-0 ">
                                    <i className="fs-24 pe-7s-users" />
                                </div>
                                    <span className=" col-10 p-0 text-left text-muted fs-12">{t("QueuesL.NumberOfInvitees")} {item.appointments ? item.appointments.length : 0}</span>
                            </div>



                        </div>


                    </div>
                    : "j";
            })}
            </div>
            <button className="btn btn-link font-pr basicColorBlue" onClick={this.props.Queues.loadMore}>{t("QueuesL.Loadmore")}</button>

        </div>;
    }
}

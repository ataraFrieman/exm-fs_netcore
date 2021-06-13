import React, { Component } from 'react';
import { Grid, Row, Col, Table, Tab, Tabs } from "react-bootstrap";
import Card from "./Card";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Selectable } from './Calendar';
// import { ServiceTypesCards } from './ServiceType';
import { ServiceQGraphs } from './ServiceQGraphs';
import * as http from '../helpers/Http';
import history from '../helpers/History';
import "../css/site.css";
import { getTime, getDate, getDateYYYMMDD } from '../helpers/TimeService';
import { AddAppointment } from './modals/AddAppointmentModal';
import { QueueTableView } from './QueueTableView';
import Results from './results/results';
import { observer } from 'mobx-react';
import { RealTimeCalendar } from './RealTimeCalendar';
import "../css/manageQ.css"
import { withI18n } from 'react-i18next';

class ManageQ extends Component {
    displayName = "ManageQ"
    constructor(props) {
        super(props);
        this.state = {
            selectedQ: props.selectedQ,
            queuesList: props.queuesList,
            display: 1,
            calendarStepSize: 30,
            fellow: null,
            showQueue: false,
            queueTime: null, start: null, end: null, showRealTime: false, viewType: 1
        };
        this.renderGridDisplay = this.renderGridDisplay.bind(this);
        this.renderScheduleDisplay = this.renderScheduleDisplay.bind(this);
        this.setStepSize = this.setStepSize.bind(this);
        this.isActiveStep = this.isActiveStep.bind(this);
        this.setFellow = this.setFellow.bind(this);
        this.setShowQueue = this.setShowQueue.bind(this);
        this.setDateQueue = this.setDateQueue.bind(this);
        this.onAppointmentRequest = this.onAppointmentRequest.bind(this);
    }

    setFellow(fellow) {
        this.setState({ fellow: fellow, disableRequestQueue: false });
    }

    onAppointmentRequest() {
        console.log(this.state.serviceQueue);
    }

    secondsToHms(d) {
        d = Number(d);

        var h = Math.floor(d / 3600);
        var m = Math.floor(d % 3600 / 60);
        var s = Math.floor(d % 3600 % 60);

        return ('0' + h).slice(-2) + ":" + ('0' + m).slice(-2) + ":" + ('0' + s).slice(-2);
    }

    advanceQ() {
        http.post('api/qManagament/AdvanceQ',
            {
                ServiceQueueId: this.state.selectedQ.id
            }, null)
            .then(data => {
                var newSelectedQ = this.state.selectedQ;
                newSelectedQ.appointments = data.appointments;
                newSelectedQ.currentAppointement = data.currentAppointement;
                newSelectedQ.currentAppointementId = data.currentAppointementId;
                this.setState({ selectedQ: newSelectedQ });
                //update Q list in mobix
                this.props.queues.setQueue(newSelectedQ);
            });
    }

    renderGridDisplay(Q, isCurrent, isPassed, spName, appointments) {
        return <QueueTableView t={this.props.t} Q={Q} secondsToHms={this.secondsToHms} isCurrent={isCurrent} isPassed={isPassed} spName={spName} appointments={appointments} />;
    }

    setStepSize(value) {
        this.setState({ calendarStepSize: value });
    }

    isActiveStep(value) {
        var t=this.props.t;
        return this.state.calendarStepSize == value ?t("MQ.active"):"";
    }

    setShowQueue(val) { this.setState({ showQueue: val ? val : !this.state.showQueue }) }

    setDateQueue(s, e) { this.setState({ start: s, end: e }) }

    renderScheduleDisplay() {
        var t=this.props.t;
        return (
            <div className="">
                <button className="btn btn-link font-pr p-0 border-0 col-12 text-align" onClick={this.props.showList}> {t("MQ.backtolist")}</button>
                <div className=" p-0 font-pr text-align calendarBtns">
                    <div className="btn-group btn-group-toggle font-pr p-1" data-toggle="buttons" >
                        <label className={"btn btn-outline-secondary btn-sm " + this.isActiveStep(15)} onClick={(e) => { this.setStepSize(15); }}>
                            <input type="radio" name="options" autoComplete="off" value={15} /> 15
                        </label>
                        <label className={"btn btn-outline-secondary btn-sm " + this.isActiveStep(30)} onClick={(e) => { this.setStepSize(30); }}>
                            <input type="radio" name="options" autoComplete="off" value={30} /> 30
                        </label>
                        <label className={"btn btn-outline-secondary btn-sm " + this.isActiveStep(60)} onClick={(e) => { this.setStepSize(60); }} >
                            <input type="radio" name="options" autoComplete="off" value={60} /> 60
                        </label>
                    </div>
                    <div className="btn-group btn-group-toggle font-pr p-1" data-toggle="buttons" >
                        <label className={"btn btn-outline-secondary btn-sm " + (this.state.showRealTime ? t("MQ.active") : "")} onClick={(e) => { this.setState({ showRealTime: true }) }}>
                            <input type="radio" name="optionsC" autoComplete="off" value={true} /> {t("MQ.Actual")}
                        </label>
                        <label className={"btn btn-outline-secondary btn-sm " + (!this.state.showRealTime ? t("MQ.active") : "")} onClick={(e) => { this.setState({ showRealTime: false }) }}>
                            <input type="radio" name="optionsC" autoComplete="off" value={false} /> {t("MQ.Planed")}
                        </label>
                    </div>
                    <div className="btn-group btn-group-toggle font-pr p-1" data-toggle="buttons" >
                        <label className={"btn btn-outline-secondary btn-sm " + (this.state.viewType == 1 ? t("MQ.active") : "")} onClick={(e) => { this.setState({ viewType: 1 }) }}>
                            <input type="radio" name="optionsVT" value={"day"} /> {t("MQ.Day")}
                        </label>
                        <label className={"btn btn-outline-secondary btn-sm " + (!this.state.viewType == 2 ? t("MQ.active") : "")} onClick={(e) => { this.setState({ viewType: 2 }) }}>
                            <input type="radio" name="optionsVT" value={"agenda"} /> {t("MQ.Agenda")}
                        </label>
                    </div>
                </div>
                <div className="w-100 p-0" role="tabpanel" >
                    {this.state.showRealTime ? <RealTimeCalendar serviceQueue={this.state.selectedQ} stepSize={this.state.calendarStepSize} setShowQueue={this.setShowQueue} setDateQueue={this.setDateQueue} viewType={this.state.viewType == 1 ?"day":"agenda"} />
                        : <Selectable id="Selectable" serviceQueue={this.state.selectedQ} stepSize={this.state.calendarStepSize} setShowQueue={this.setShowQueue} setDateQueue={this.setDateQueue} viewType={this.state.viewType == 1 ?"day":"agenda"} />}
                </div>
            </div>);
    }

    renderAddQueue() {
        var Q = this.state.selectedQ;
        this.props.filterStore.setFilterItem("serviceProviderSelect", Q.serviceProvider.fullName, Q.serviceProvider.id);
        this.props.filterStore.setFilterItem("dateRangeSelect", this.state.start ? new Date(this.state.start) : new Date(), this.state.end ? new Date(this.state.end) : new Date());
        this.props.filterStore.setFilterItem("branchSelect", Q.branch.name, Q.branch.id);
        this.props.filterStore.setFilterItem("serviceTypeSelect", Q.serviceType.descripton, Q.serviceType.id);
        return <Results t={this.props.t} filtersArray={this.props.filtersList} serviceQueue={Q} startTime={new Date(this.state.start)} hideAfterSchedual={true} queues={this.props.queues} hideResultFunc={e => { this.setShowQueue(false); }} resultClass="col"/>;
    }

    render() {
        var t=this.props.t;
        var Q = this.state.selectedQ;
        if (!Q)
            return "";
        var SP = Q.serviceProvider;
        var appointments = this.state.selectedQ.appointments ? this.state.selectedQ.appointments : [];
        var spName = SP?((SP.title ? SP.title : "") + " " + SP.firstName + " " + SP.lastName):"";
        var isPassed = (Q.passed == true || Q.actualEndTime) ? true : false;
        var isCurrent = Q.currentAppointement && Q.currentAppointement.id;
        var next = null;
        if (isCurrent && appointments.length) {
            var currId = Q.currentAppointement.id;
            var index = appointments.findIndex(a => a.id == currId);
            next = index && (index + 1) < appointments.length ? appointments[index + 1] : 0;
        }
        var display = this.state.display == 1 ? this.renderScheduleDisplay() : this.renderGridDisplay(Q, isCurrent, isPassed, spName, appointments);

        return (
            
            <div>
                <Row className="p-2 m-0 d-none">
                    <span>{t("MQ.display")}: </span>
                    <button type="button" className="btn btn-secondary btn-sm" onClick={e => { this.setState({ display: 0 }); }}>{t("MQ.table")}</button>
                    <button type="button" className="btn btn-secondary btn-sm" onClick={e => { this.setState({ display: 1 }); }}>{t("MQ.diary")}</button>
                </Row>


                <Row className="pt-2 pb-1 m-0">

                    <div className="border col-11 p-1 ml-1">
                        <div className="row m-0">
                            <span className="col-1 p-0 font-pr align-self-center">
                                <i className="fs-16 pe-7s-date" />
                                {getDateYYYMMDD(Q.beginTime)}
                            </span>
                            <span className="col-2 mxw-150 p-0 font-pr align-self-center">
                                <i className="fs-16 pe-7s-clock" />
                                {(getTime(Q.beginTime) + "-" + getTime(Q.endTime))}
                            </span>
                            <span className="col-1 p-0 font-pr align-self-center">
                                <i className="fs-16 pe-7s-map-marker" />
                                {Q.branch ? Q.branch.name : ""}
                            </span>
                            <span className="col-1 p-0 font-pr miw-150 align-self-center">
                                <i className="fs-16 pe-7s-id" />
                                {" " +( SP ? SP.fullName:"" )+ " "}
                            </span>
                            <div className="col-6 p-0 font-pr text-success align-self-center">
                                <div className="row">
                                    <div className="card col-6 rounded-0">
                                        <div className="card-body pt-0 pb-0">
                                            <span className="m-0 h1">{isCurrent && !isPassed ? "" + (Q.currentAppointement ? Q.currentAppointement.fellow.id : ' ') : ""}</span>
                                            <span >
                                            {isPassed ? t("MQ.Durationqueue") + (getTime(Q.actualBeginTime) + " - " + getTime(Q.actualEndTime))
                                                :
                                                isCurrent && !isPassed ? "" + (Q.currentAppointement ? ("-" + Q.currentAppointement.fellow.fullName) : ' ') : ""}</span>
                                        </div>
                                    </div>
                                    <div className="col-6 align-self-center h-100">
                                        {!isPassed ? <button className="btn btn-danger rounded-0 basicBackground h-100 btn-lg btn-block  font-pr p-1"
                                            onClick={(event) => { this.advanceQ() }}><h4>{t("MQ.Next") + (next && next.fellow ? ("(" + next.fellow.fullName + ")") : "")}</h4></button> : ""}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                </Row>

                <Row className="m-0">
                    <div className="col-10 border ">
                        <div className="row">
                            <div className="col-12">{display}</div>
                            {this.state.showQueue ? <div className="modal d-block" tabindex="-1" role="dialog" >

                                <div className="modal-dialog" role="document" >
                                    <div className="modal-content">
                                        <button type="button" className="close" data-dismiss="modal" onClick={e => { this.setState({ showQueue: false }); }}>
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                        {this.renderAddQueue()}
                                    </div>
                                </div>
                            </div> : ""}

                        </div>
                    </div>
                </Row>
            </div>);
    }
}
export default withI18n()(observer(ManageQ));
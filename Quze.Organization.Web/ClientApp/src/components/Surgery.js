import "../../node_modules/react-big-calendar/lib/css/react-big-calendar.css";
import React, { Component } from 'react';
import 'react-datepicker/dist/react-datepicker.css';
import { observer } from 'mobx-react';
import { withI18n } from "react-i18next";
import { withRouter } from 'react-router-dom';
import * as http from '../helpers/Http';
import '../css/Surgery.css';
import { RenderSurgery } from './RenderSurgery';
import { RenderSurgeryByDepartments } from './RenderSurgeryByDepartments'
import { CalanderSurgery } from './calanderSurgery';
import providersStore from '../stores/serviceProviders';
import serviceTypesStore from '../stores/serviceTypes';
import fellowsStore from '../stores/fellows';
import { PropagateLoader } from 'react-spinners';
import { Row } from "react-bootstrap";
import $ from 'jquery';
import { bool } from "prop-types";
import { getDateYYYMMDD } from '../helpers/TimeService'
import DatePicker, { registerLocale } from "react-datepicker";
import { NavCalander } from './surgeryComponent/NavCalander.js'
import operation from '../stores/Operation';

window.store = { providersStore, serviceTypesStore, fellowsStore };

var resourceList = [];
var eventList = [];

class Surgery extends Component {

    constructor(props) {
        var operationStore = operation.fromJS();
        super(props);
        this.state = {
            state: "",
            serviceQueues: [],
            ServiceQueueDate: "",
            operationQueue: null,
            operationQueueOrig: [],
            operationQueueScedule: [],
            operationQueueActual: [],
            conflictList: {},
            fellows: fellowsStore.FellowsList,
            conflictId: [],
            loading: false,
            reschedulingState: false,
            actualState: false,
            isShowDetailsModal: false,
            resultDetailsModal: {},
            fadeStatuse: true,
            fadeStatuseList: true,
            roomsOrganization: [],
            serviceProvidersOrganization: [],
            selectServiceQByDate: [],
            serviceQListByDate: [],
            showListOfServiceQ: false,
            startDateForDatePicker: new Date(),
            shiftDateTime: "",
            startShift: "",
            endShift: "",
            pickedSq: false,
            dateToDatePicker: [],
            appointmentsWithNewStatus: [],
            operationStore: operationStore
        };
        this.Upload = this.Upload.bind(this);
        this.AddSurgery = this.AddSurgery.bind(this);
        this.load =


            this.load.bind(this);
        this.NewRescheduling = this.NewRescheduling.bind(this);
        this.Actual = this.Actual.bind(this);
        this.OriginalReschedule = this.OriginalReschedule.bind(this);
        this.UploadActual = this.UploadActual.bind(this);
        this.reschedual = this.reschedual.bind(this);
        this.handelChange = this.handelChange.bind(this);
        this.serviceQueueId = this.handelChange.bind(this);
        this.getRoomsOrganization = this.getRoomsOrganization.bind(this);
        this.getServiceQeueByDate = this.getServiceQeueByDate.bind(this);
        this.getServiceQById = this.getServiceQById.bind(this);
        this.setResourceList = this.setResourceList.bind(this);
        this.updateAppointment = this.updateAppointment.bind(this);
        this.findAppointment = this.updateAppointment.bind(this);
        window.Surgery = this
    }
    componentWillMount() {
        this.getRoomsOrganization();
    }
    componentDidMount() {
        // TODO: get the id of the first SQ and go to the server to bring all his operations
        let firstServiceQueueId = "";
        var thisObj = this;
        this.load();
        http.get('api/Operations/GetQueuesDates')
            .then(res => {
                res.forEach((element, i) => {
                    let time = element.beginTime.split("T")
                    let displayTime = time[1].split(":")
                    element.time = new Date(time).toLocaleDateString() // + ", " + displayTime
                    this.load();
                });
                this.getServiceQeueByDate();
                thisObj.setState({ serviceQueues: res, dateToDatePicker: this.props.serviceQueues.map(s => new Date(s.beginTime)) });
            })
            .catch(error => { console.log("Error:", error) });
    }
    updateAppointment(appointment) {
        var app = this.findAppointment(appointment);
        app = appointment;
        this.setState({ operationQueue: this.state.operationQueue });
    }
    findAppointment(appointment) {
        var app = this.state.operationQueue.operationsList.find(opp => opp.id === appointment.id);
        return app;
    }
    getRoomsOrganization() {
        http.get('api/Operations/GetRooms')
            .then(res => {
                res.entities.forEach(room => {
                    var resource =
                    {
                        resourceId: room.id,
                        resourceTitle: room.description
                    };
                    this.state.roomsOrganization.push(resource);
                })
            })
            .catch(error => { console.log("Error:", error) });
    }

    getServiceProvidersOrganization() {
        http.get('ServiceProviders/getAll')
            .then(res => {
                res.entities.forEach(serviceProvider => {
                    var resource =
                    {
                        resourceId: serviceProvider.id,
                        resourceTitle: serviceProvider.fullName
                    };
                    this.state.serviceProvidersOrganization.push(resource);
                })
            })
            .catch(error => { console.log("Error:", error) });
    }

    load() {
        this.setState({ loading: !this.state.loading });
    }

    Upload(res) {
        this.setState({ operationQueue: res, state: "O", loading: false, operationQueueOrig: res });
        this.createConflictList(res.conflictList);
    }
    AddSurgery(res) {
        res.operationsList.sort(((a, b) => { return a.operation.roomId - b.operation.roomId }));
        this.setState({ operationQueue: res, state: "O", loading: false, operationQueueOrig: res, reschedulingState: false });
        this.createConflictList(res.conflictList);
    }

    rescheduleSetState = (res) => {
        var thisObj = this;
        thisObj.setState({ operationQueue: res, state: "S",loading: false, reschedulingState: true, operationQueueScedule: res, loading: false });
    }

    reschedual() {
        this.setState({loading: true});
        this.state.operationStore.reschedual(this.state.operationQueue, this.props.equipments, this.rescheduleSetState)
    }

    setStateAfterUpdateAppReschedule = (res) => {
        this.setState({ operationQueueOrig: res, operationQueue: res });//save changes in orig screen
        this.reschedual();//send to reschedule
    }

    UploadActual(res) {
        var OQ = this.state.operationQueue;
        var OL = this.state.operationQueue.operationsList;
        for (var i = 0; i < OL.length; i++) {
            var operation = res.find(o => o.id === OL[i].operationId);
            if (operation)
                OL[i].operation = operation;
        }
        OQ.operationsList = OL;
        this.setState({ operationQueue: OQ, state: "A", loading: false, actualState: true, operationQueueActual: OQ });
    }

    ResetList = () => {
        eventList = [];
        resourceList.splice(0, resourceList.length)
    }

    setEventsList = (events) => {
        eventList = events;
    }
    setResourceList(resoursList) {
        var g = resoursList
        g.forEach((r) => {
            resourceList.push(r);
        });

    }
    createConflictList = (conflictList) => {
        if (!conflictList || !conflictList.length)
            return;
        var conflictWasOver = [conflictList.lengh];
        for (var i = 0; i < conflictList.length; i++) {
            var conflict = conflictList[i];
            this.state.conflictList[i.toString()] = [];
            if (conflictWasOver[i] !== true)
                this.state.conflictList[i.toString()].push(conflict.id);
            for (var j = i + 1; j < conflictList.length; j++) {
                if ((JSON.stringify(conflict.appointmentA) === JSON.stringify(conflictList[j].appointmentA) ||
                    JSON.stringify(conflict.appointmentA) === JSON.stringify(conflictList[j].appointmentB)) ||
                    (JSON.stringify(conflict.appointmentB) === JSON.stringify(conflictList[j].appointmentA) ||
                        JSON.stringify(conflict.appointmentB) === JSON.stringify(conflictList[j].appointmentB))) {
                    this.state.conflictList[i.toString()].push(conflictList[j].id);
                    conflictWasOver[j] = true;
                }
            }
        }
        var t = this.state.conflictList;
    }

    showDetailsSurgeryModal = (result) => {
        this.setState({ resultDetailsModal: result, isShowDetailsModal: true });
    }

    changeEvent = (beginT, endT, id, t, resourceId) => {
        var e = {
            id: id,
            title: t,
            start: new Date(beginT),
            end: new Date(endT),
            resourceId: resourceId
        }
        eventList.push(e);
        return eventList;
    }

    changeResource = (serviceProviderId, serviceProvider) => {
        var isExsist = false;
        // --------------------------------------
        var r = {
            resourceId: serviceProviderId,
            resourceTitle: serviceProvider
        }
        if (resourceList.length === 0) {
            resourceList.push(r);//first is always pushed
        }
        for (var i = 0; i < resourceList.length; i++) {
            if (resourceList[i].resourceId === serviceProviderId)//exist
            {
                isExsist = true;
            }
        }
        if (!isExsist && !(resourceList.length === 0)) {
            resourceList.push(r);
        }
    }
    NewRescheduling() {
        this.setState({ state: "S", operationQueue: this.state.operationQueueScedule });
        this.state.appointmentsWithNewStatus.forEach(app => {
            var appointment = this.state.operationQueueScedule.operationsList.find(ap => ap.id === app.id)
            appointment.operation.status = app.operation.status;
            if (appointment.operation.status === 7 || appointment.isDeleted)//operation cancel delete from new reschedual
                this.setState({ reschedulingState: false });
        })

    }
    OriginalReschedule() {
        this.setState({ state: "O", operationQueue: this.state.operationQueueOrig });
        this.state.appointmentsWithNewStatus.forEach(app => {
            var appointment = this.state.operationQueueOrig.operationsList.find(ap => ap.id === app.id);
            if (appointment)
                appointment.operation.status = app.operation.status;
        })
    }
    Actual() {
        this.setState({ state: "A", operationQueue: this.state.operationQueueActual });
        this.state.appointmentsWithNewStatus.forEach(app => {
            var appointment = this.state.operationQueueActual.operationsList.find(ap => ap.id === app.id);
            appointment.operation.status = app.operation.status;
        })
    }
   
    changeLocations = (events) => {
        this.state.roomsOrganization.forEach(item => {
            resourceList.push({
                resourceId: item.resourceId,
                resourceTitle: item.resourceTitle
            })
        })
        var sum = 0;
        if (!events)
            return;
        for (var i = 0; i < events.length; i++) {
            this.creaeteEventLocation(events[i])
            sum += (events[i].lengh) + 1;
        }
        return eventList;
    }

    creaeteEventLocation = (operationSurgery) => {
        switch (this.state.state) {
            case "O":
                {
                    this.changeEvent(operationSurgery.operation.surgeryOrigBeginTime, operationSurgery.operation.surgeryOrigEndTime,
                        (operationSurgery.operationId), "surgeon " + operationSurgery.operation.surgeon.firstName + " " + operationSurgery.serviceType.description
                        , (operationSurgery.operation.roomId));
                    // this.changeEvent(operationSurgery.operation.cleanOrigBeginTime, operationSurgery.operation.cleanOrigEndTime,
                    //     (operationSurgery.operationId), "clean room" + operationSurgery.operation.roomId + " after " + operationSurgery.serviceType.description
                    //     , (operationSurgery.operation.roomId));
                    //this.changeEvent(operationSurgery.operation.anesthesiaOrigBeginTime, operationSurgery.operation.anesthesiaOrigEndTime,
                    //    (operationSurgery.operationId), operationSurgery.fellow.firstName, (operationSurgery.operation.roomId));
                }
                break;
            case "S":
                {
                    this.changeEvent(operationSurgery.operation.surgeryBeginTime, operationSurgery.operation.surgeryEndTime,
                        (operationSurgery.operationId), "surgeon " + operationSurgery.operation.surgeon.firstName + " " + operationSurgery.serviceType.description
                        , (operationSurgery.operation.roomId));
                    // this.changeEvent(operationSurgery.operation.cleanBeginTime, operationSurgery.operation.cleanEndTime,
                    //     (operationSurgery.operationId), "clean room" + operationSurgery.operation.roomId + " after " + operationSurgery.serviceType.description
                    //     , (operationSurgery.operation.roomId));
                    //this.changeEvent(operationSurgery.operation.anesthesiaBeginTime, operationSurgery.operation.anesthesiaEndTime,
                    //    (operationSurgery.operationId), operationSurgery.fellow.firstName, (operationSurgery.operation.roomId));
                }
                break;
            case "A":
                {
                    this.changeEvent(operationSurgery.operation.surgeryActualBeginTime, operationSurgery.operation.surgeryActualEndTime,
                        (operationSurgery.operationId), "surgeon " + operationSurgery.operation.surgeon.firstName + " " + operationSurgery.serviceType.description
                        , (operationSurgery.operation.roomId));
                    // this.changeEvent(operationSurgery.operation.cleanActualBeginTime, operationSurgery.operation.cleanActualEndTime,
                    //     (operationSurgery.operationId), "clean room" + operationSurgery.operation.roomId + " after " + operationSurgery.serviceType.description
                    //     , (operationSurgery.operation.roomId));
                    //this.changeEvent(operationSurgery.operation.anesthesiaActualBeginTime, operationSurgery.operation.anesthesiaActualEndTime,
                    //    (operationSurgery.operationId), " " + operationSurgery.fellow.firstName, (operationSurgery.operation.roomId));
                }
                break;
            default:

        }
    }

    addConflictId = (indexConflict) => {
        var ids = this.state.conflictList[parseInt(indexConflict)];
        this.setState({ conflictId: ids })
    }

    changeConflict = (conflict, resourceIdA, resourceIdB, index) => {
        conflict.appointmentA.beginTime = conflict.conflictBeginTime;
        conflict.appointmentB.beginTime = conflict.conflictBeginTime;
        conflict.appointmentA.endTime = conflict.conflictEndTime;
        conflict.appointmentB.endTime = conflict.conflictEndTime;
        this.changeEvent(conflict.conflictBeginTime, conflict.conflictEndTime,
            conflict.id, "C " + index,
            resourceIdA);
        this.changeEvent(conflict.appointmentB.beginTime, conflict.conflictEndTime,
            conflict.id, "C " + index,
            resourceIdB);
        return eventList
    }

    onCloseModal = () => {
        this.setState({ isShowDetailsModal: false });
    }
    

    getCalander = () => {
        return <CalanderSurgery
            operationQueue={this.state.operationQueue}
            operationQueueOrig={this.state.operationQueueOrig}
            eventList={eventList}
            resourceList={resourceList}
            conflicts={this.state.conflictId}
            showDetailsSurgeryModal={this.showDetailsSurgeryModal}
            state={this.state.state}
            cancelationReasons={this.props.cancelationReasons.cancelationReasonsList}
            t={this.props.t}
            ChangOperationStatus={this.ChangOperationStatus}
            removeApoointment={this.removeApoointment}
            updateDelayApointment={this.updateDelayApointment}
            equipments={this.props.equipments}
            updateAppointment={this.updateAppointment}
            reschedualEquipment={this.reschedualEquipment}
            departments={this.props.departments}
            serviceTypes={this.props.serviceTypes}
            serviceProviders={this.props.serviceProviders}
            AddSurgery={this.AddSurgery}
            loading={this.load}
            setEventsList={this.setEventsList}
            fellows={this.props.fellows}
            rescheduleSetState={this.setStateAfterUpdateAppReschedule}
            equipments={this.props.equipments}
        />
    }


    reschedualEquipment = () => {
        var data = {
            operationQueue: this.state.operationQueue,
            BeginTime: new Date(this.state.operationQueue.serviceQueue.beginTime),
            Equipments: this.props.equipments.EquipmentsList
        };
        var thisObj = this;
        http.post('api/NewInlay', data)
            .then(res => {
                if (this.state.state === "S")
                    thisObj.setState({ operationQueue: res })
                thisObj.setState({ reschedulingState: true, operationQueueScedule: res });
            })
            .catch(error => { console.log("Error:", error) });
    }

    handelChange(e) {
        let valueInput = e.target.value;
        this.setState({ ServiceQueueDate: e.target.value })
    }

    getServiceQById(id) {
        http.get('api/Operations/GetServiceQueueById/' + id)
            .then(res => {
                this.setState({ operationQueue: res, state: "O", loading: false, operationQueueOrig: res, reschedulingState: false, startDateForDatePicker: res.serviceQueue.beginTime });
                this.createConflictList(res.conflictList);
                console.log(res);
            })
            .catch(error => { console.log("Error:", error) });
    }

    getServiceQByDate = date => {
        //  let selectedDate = new Date(date).toString("yyyy MMMM ");;
        if (date && new Date(date) != "Invalid Date") {
            var d
            this.setState({ startDateForDatePicker: date, serviceQListByDate: [] });

            this.props.serviceQueues.forEach(sq => {
                d = getDateYYYMMDD(new Date(sq.beginTime));
                if (getDateYYYMMDD(new Date(sq.beginTime)) === date) {
                    this.state.serviceQListByDate.push(sq);
                }
            });
            if (this.state.serviceQListByDate.length != 0) {
                if (date !== "") {
                    this.state.serviceQListByDate.sort((a, b) => { return new Date(a.beginTime).getTime() - new Date(b.beginTime).getTime() })
                    this.getServiceQById(this.state.serviceQListByDate[0].id);
                }
            }
            else
                this.setState({ operationQueue: null, state: "", loading: false, operationQueueOrig: null });


        }
    }

    getServiceQeueByDate() {
        this.setState({ loading: true });
        if (this.props.serviceQueues) {
            var res = null;
            this.props.serviceQueues.forEach(sq => {
                if (new Date(sq.beginTime).toLocaleDateString() === new Date().toLocaleDateString()) {
                    this.state.serviceQListByDate.push(sq);
                }
            });
            if (this.state.serviceQListByDate.length === 0)//no serviceQ in this date
            {
                //give the last service queque
                this.props.serviceQueues.sort((a, b) => {
                    return new Date(b.beginTime) - new Date(a.beginTime);
                });
                // find all last sQ
                this.props.serviceQueues.forEach(sq => {
                    if (new Date(sq.beginTime).toLocaleDateString() === new Date(this.props.serviceQueues[0].beginTime).toLocaleDateString()) {
                        this.state.serviceQListByDate.push(sq);
                    }
                });

            }
            else {
                //just one sQ
                if (this.state.serviceQListByDate.length === 1) {
                    res = {
                        operationsList: [],
                        conflictList: null,
                        serviceQueue: this.state.serviceQListByDate[0]
                    }
                }
            }
            if (res !== null)
                this.getServiceQById(res.serviceQueue.id);
            else {
                //few sQ
                this.state.serviceQListByDate.sort((a, b) => { return new Date(a.beginTime).getTime() - new Date(b.beginTime).getTime() })
                this.getServiceQById(this.state.serviceQListByDate[0].id);
            }
        }
        else {
            this.setState({ operationQueue: null, state: "", loading: false, operationQueueOrig: null });
        }
    }


    getFadeInput = () => {
        if (this.state.fadeStatuse) {
            $('#datePicker').fadeIn(1000);
        }
        else {
            $('#datePicker').fadeOut(1000);
        }
        this.setState({ fadeStatuse: !this.state.fadeStatuse });
    }


    getServiceQ = (selectServiceQ) => {
        var serviceOid = parseInt(selectServiceQ.target[selectServiceQ.target.selectedIndex].id);
        var serviceaq = this.state.serviceQListByDate.find(sQ => {
            return sQ.serviceQueue.id === serviceOid;
        })
        this.setState({ operationQueue: serviceaq, state: "O", loading: false, operationQueueOrig: serviceaq, reschedulingState: false });
    }




    render() {
        var t = this.props.t;
        var a = this.props.serviceQueues;
        let current_datetime = this.state.operationQueue && this.state.operationQueue.serviceQueue ?
            getDateYYYMMDD(new Date(this.state.operationQueue.serviceQueue.beginTime)) : this.state.startDateForDatePicker ?
                getDateYYYMMDD(new Date(this.state.startDateForDatePicker)) : getDateYYYMMDD(new Date());
        return <div>
            {
                this.state.loading ?
                    <div className="ProgressStyle ">
                        <PropagateLoader color={'#3c519d'} />
                    </div>
                    :
                    <span />
            }
            <div className="row">
                <div className="col-2 m-0 p-0">
                    <div>
                        <NavCalander operationQueue={this.state.operationQueue}
                            getServiceQById={this.getServiceQById}
                            serviceQueues={this.state.serviceQueues}
                            dateToDatePicker={this.state.dateToDatePicker}
                            startDateForDatePicker={this.state.startDateForDatePicker}
                            getServiceQByDate={this.getServiceQByDate}
                            operationQueue={this.state.operationQueue}
                            serviceQListByDate={this.state.serviceQListByDate}
                            state={this.state.state}
                            reschedual={this.reschedual}
                            current_datetime={current_datetime}
                            t={t}
                            load={this.load}
                            Upload={this.Upload}
                            UploadActual={this.UploadActual}
                            serviceProviders={this.props.serviceProviders}
                            serviceTypes={this.props.serviceTypes}
                            fellows={this.props.fellows}
                            rooms={this.props.rooms}
                            departments={this.props.departments}
                            AddSurgery={this.AddSurgery}
                        ></NavCalander>

                    </div>
                </div>

                {/* <div className="m-1"></div> */}
                {this.state.operationQueue && this.state.state && this.state.operationQueue.operationsList.length > 0 ?
                    <div className="col-10 m-0 px-2">
                        <RenderSurgery
                            state={this.state.state}
                            ResetList={this.ResetList}
                            operationQueue={this.state.operationQueue ? this.state.operationQueue : this.state.serviceQueues}
                            changeLocations={this.changeLocations}
                            changeConflict={this.changeConflict}
                            changeResource={this.changeResource}
                            changeEvent={this.changeEvent}
                            t={this.props.t}
                            getCalander={this.getCalander}
                            f={this.state.f}
                            addConflictId={this.addConflictId}
                            conflicts={this.state.conflictList}
                            eventData={eventList}
                            NewRescheduling={this.NewRescheduling}
                            OriginalReschedule={this.OriginalReschedule}
                            isShowTable={this.state.isShowTable}
                            reschedulingState={this.state.reschedulingState}
                            actualState={this.state.actualState}
                            Actual={this.Actual}
                            endNav={this.endNav}
                            departments={this.props.departments}
                        >
                        </RenderSurgery>
                    </div>
                    :
                    this.state.operationQueue ?
                        <Row className="m-0 px-2 col-10">
                            <div className="col-12">
                                <CalanderSurgery
                                    operationQueue={this.state.operationQueue}
                                    operationQueueOrig={this.state.operationQueueOrig}
                                    eventList={[]}
                                    resourceList={this.state.roomsOrganization}
                                    conflicts={this.state.conflictId}
                                    showDetailsSurgeryModal={this.showDetailsSurgeryModal}
                                    state={this.state.state}
                                    cancelationReasons={this.props.cancelationReasons.cancelationReasonsList}
                                    t={this.props.t}
                                    //צריך לסדר שהמודל יעבור
                                    serviceProviders={this.props.serviceProviders}
                                    serviceTypes={this.props.serviceTypes}
                                    fellows={this.props.fellows}
                                    rooms={this.props.rooms}
                                    departments={this.props.departments}
                                    AddSurgery={this.AddSurgery}
                                    loading={this.load}
                                    setEventsList={this.setEventsList}
                                    rescheduleSetState={this.setStateAfterUpdateAppReschedule}
                                >
                                </CalanderSurgery>
                            </div>
                        </Row>
                        :
                        ""
                }

            </div>


            {
                //{this.state.operationQueue ?
                //    <SurgeryModal
                //        t={this.props.t}
                //        serviceProviders={this.props.serviceProviders}
                //        serviceTypes={this.props.serviceTypes}
                //        fellows={this.props.fellows}
                //        rooms={this.props.rooms}
                //        departments={this.props.departments}
                //        AddSurgery={this.AddSurgery}
                //        load={this.load}
                //        serviceQueueId={this.state.operationQueue ? this.state.operationQueue.serviceQueue.id : -1}
                //        operationQueue={this.state.operationQueue ? this.state.operationQueue : null}
                //        dateTime={this.state.shiftDateTime} //{this.state.pickedSq ? this.state.shiftDateTime }

                //    /> : ""}
                //{this.state.operationQueue ?

                //    <UploadSurgeryModal
                //        t={this.props.t}
                //        Upload={this.Upload}
                //        UploadActual={this.UploadActual}
                //        load={this.load}
                //        state={this.state.state}
                //        operationQueue={this.state.operationQueue ? this.state.operationQueue : null}
                //        serviceQueueId={this.state.operationQueue ? this.state.operationQueue.serviceQueue.id : -1}
                //    /> : ""}
            }
        </div >


    }

}
export default withI18n()(withRouter((observer(Surgery))));

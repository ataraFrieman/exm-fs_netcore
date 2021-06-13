import React, { Component } from 'react';
import { Calendar, momentLocalizer, Views } from 'react-big-calendar';
import moment from 'moment';
import * as http from '../helpers/Http';
import $ from 'jquery';
import '../css/minimalKit.css';
import { Statuses } from './Statuses.js';
import { EditSurgeryModal } from './modals/EditSurgeryModal.js'
import operation from '../stores/Operation';
import MinimalKit from './MinimalKit';
import PermissionForSurgery from './PermissionForSurgery';
import { getTime } from '../helpers/TimeService'
import withDragAndDrop from 'react-big-calendar/lib/addons/dragAndDrop'
import UpdateCleanModal from './modals/UpdateCleanModal';
import { PropagateLoader } from 'react-spinners';
import '../css/Modal.css';


const localizer = momentLocalizer(moment);
const DragAndDropCalendar = withDragAndDrop(Calendar)
export class CalanderSurgery extends Component {
    displayName = "CalanderSurgery"
    constructor(props) {
        var operationStore = operation.fromJS();
        super(props);
        console.log("CalanderSurgery props: ", this.props)
        this.state = {
            isShowDetailsModal: false,
            resultDetailsModal: {},
            overSurgery: [],
            overSurgeryAfterClick: [],
            isShowBtn: true,
            isShowMk: false,
            fadeStatuse: true,
            isCancelModal: false,
            reasonId: "",
            reasonText: "",
            reasonsList: [{ description: "Select reason:", id: 0 }, ...this.props.cancelationReasons],
            moreDetailes: false,
            operationQueue: [],
            operationStore: operationStore,
            showMKmodal: false,
            loading: false//modal
        };
        // this.showMkDetails = this.showMkDetails.bind(this);
        this.getValueIdFromE = this.getValueIdFromE.bind(this);
        this.getFadeInput = this.getFadeInput.bind(this);
        this.onCloseCancelingModal = this.onCloseCancelingModal.bind(this);
        this.saveCancelingOperation = this.saveCancelingOperation.bind(this);
        this.getSlotStyleTimeAfterServiceQ = this.getSlotStyleTimeAfterServiceQ.bind(this)
        this.getIconStyleByPriority = this.getIconStyleByPriority.bind(this);
        this.getColorByReadyForSurgery = this.getColorByReadyForSurgery.bind(this);

        this.moveEvent = this.moveEvent.bind(this);
        // this.updateMinimalKit = this.updateMinimalKit.bind(this);
    }


    moveEvent({ event, start, end, resourceId }) {
        var resultDataOfSurgery =this.findAppointmentByEvent(event);
        var valueInput = getTime(start);
        let date = this.props.operationQueue.serviceQueue.beginTime.split("T");
        let beginTime = date[0] + "T" + valueInput;
        if (valueInput < date[1].slice(0, 5) || valueInput > new Date(this.props.operationQueue.serviceQueue.endTime).toLocaleTimeString()) {
            beginTime = "";
            valueInput = "";
            alert("Time out of range");
        }
        var data = {
            BeginTime: beginTime,                       //Required
            Code: "1000",                                            //Required
            HostingDepartmentCode: resultDataOfSurgery.operation.hostingDepartmentId, //Required
            SurgicalDepartmentCode: resultDataOfSurgery.operation.surgicalDepartmentId, //Required
            HostingDepartmentId: resultDataOfSurgery.operation.hostingDepartment.code,
            SurgicalDepartmentId: resultDataOfSurgery.operation.surgicalDepartment.code,
            NursingUnitDepartmentCode: resultDataOfSurgery.operation.nursingUnitId?resultDataOfSurgery.operation.nursingUnitId:null,
            NursingUnitDepartmentId:resultDataOfSurgery.operation.nursingUnit?resultDataOfSurgery.operation.nursingUnit.code:null,
            IsTraining: true,//db
            LocationCode: resourceId,                 //Required
            OperationTypeCode: resultDataOfSurgery.serviceType.code,         //Required
            Priority: resultDataOfSurgery.operation.priority, //Required
            FellowName: resultDataOfSurgery.fellow.fullName,                     //Required
            FellowCode: resultDataOfSurgery.fellow.identityNumber,                     //Required
            FellowAge:resultDataOfSurgery.fellow.age?resultDataOfSurgery.fellow.age:0, //db                      //Required
            FellowWeight: resultDataOfSurgery.fellow.weight? resultDataOfSurgery.fellow.weight:0,
            FellowHeight:resultDataOfSurgery.fellow.height?resultDataOfSurgery.fellow.height:0,
            IsFellowDiabetic: true,//db
            IsHBP: true,                         //Required
            IsXrayDeviceRequired: true,
            FellowGender: resultDataOfSurgery.fellow.gender,                //Required
            SurgeonCode: resultDataOfSurgery.operation.surgeon.id,                   //Required
            SurgicalNursCode: resultDataOfSurgery.operation.nurse ? resultDataOfSurgery.operation.nurse.id : 0,
            AnesthesiologistCode: resultDataOfSurgery.operation.anesthesiologistId, //Required
            SurgeonId: resultDataOfSurgery.operation.surgeon.identityNumber,
            SurgicalNursId: resultDataOfSurgery.operation.nurse ? resultDataOfSurgery.operation.nurse.identityNumber : null,
            AnesthesiologistId: resultDataOfSurgery.operation.anesthesiologist.identityNumber, //Required
            CleanTeamCode: 0
        };

        this.state.operationStore.editOperation(data, this.props.operationQueue, this.props.equipments, resultDataOfSurgery, this.props.state, this.props.loading, this.props.AddSurgery, this.props.rescheduleSetState)
    }
    //style on calander
    getColorByStatus = (status) => {
        switch (status) {
            case "2":
                return "rgba(150, 156, 162, 0.23)";
            case "3":
                return "rgba(124, 221, 104, 0.23)";
            case "4":
                return "rgba(140, 116, 221, 0.41)";
            case "5":
                return "rgba(26, 249, 254, 0.41)";
            case "7":
                return "rgb(250, 246, 251)";
            case "8":
                return "linear-gradient(rgba(213, 138, 66, 0.41) 50%, black 52%, rgba(213, 138, 66, 0.41) 50%)";
        }
    }
    getSlotStyle = (data) => {
        var style = {
            background: "red",

        };
        return {
            style: style
        };
    }
    componentDidMount() {
        this.getSlotStyleTimeAfterServiceQ()
        // this.props.eventList.forEach((event) => {
        //     this.getIconStyleByPriority(event);
        // });
    }

    getSlotStyleTimeAfterServiceQ() {
        //if event is over the end serviceQ time the time background in calander is red 
        var timeOfSq = this.props.operationQueue.serviceQueue.endTime.split("T")[1].split(":");
        $(".rbc-time-slot").map(function () {
            var thisTime = this.textContent.split(":")
            if (parseFloat(thisTime[0] + "." + thisTime[1]) > parseFloat(timeOfSq[0] + "." + timeOfSq[1]))
                this.className = this.className + " basicColorRedBackground"
        });
    }
    getIconStyleByPriority(event) {
        if (event.title.indexOf('surgeon') >= 0) {
            var start = getTime(event.start), end = getTime(event.end);
            var d = $("div[title|='" + start + " – " + end + ": " + event.title + "']");
            if (d) {
                var i = document.createElement("p");
                i.className = "priority"
                d.append(i);
            }
        }
    }
    getColorByReadyForSurgery(event, element) {
        //some one is null
        if (!element.operation.isTeamReady){
            //||!element.operation.isEqpReady) {
            if (event.id === element.operationId)
                return "#ed1c4038"
        }
        else if (element.operation.isTeamReady != "Error" ){
        //&& (element.operation.isEqpReady != "Error")) {
            return null;
        }
        else {
            return "#ed1c4038"
        }
    }

    getEventStyle = (event) => {
        //get enent style
        var type = "";
        var color = event.title.indexOf('clean') >= 0 ? "#637697" :
            event.title.indexOf('surgeon') >= 0 ? "rgba(155, 156, 165, 0.25)" : "rgba(155, 156, 165, 0.25)";
        // if (event.title.indexOf('surgeon') >= 0)
        //     this.getIconStyleByPriority(event)

        if (this.props.operationQueue.conflictList)
            type = event.title.indexOf('C ') >= 0 ? "conflict" : "";
        var style = {
            background: color,
            color: "black",
            height: "20px",
            left: 0,
            fontSize: "13px",
            borderRadius: 0,
            borderColor: "gray",
            borderWidth: "thin"
        };
        if (type === "conflict") {
            style.background = "repeating-linear-gradient(45deg,rgba(155, 156, 165, 0.57),rgba(155, 156, 165, 0.57) 7px, transparent 5px, red 9px)";
            style.zIndex = "150";
        }
        this.props.operationQueue.operationsList.forEach(element => {
            var backgroundForNotReady = this.getColorByReadyForSurgery(event, element);
            if (backgroundForNotReady) {
                style.background = backgroundForNotReady;
            }
            if (element.operation.delay > 30) {
                if (event.id === element.operationId) {
                    style.borderColor = "#637697";
                    style.borderWidth = "2px";
                }
            }
            if (element.served) {
                if (event.id === element.operationId)
                    style.background = "rgb(250, 246, 251)";
            }
            else {
                if (element.operation.status) {
                    if (event.id === element.operationId)
                        style.background = this.getColorByStatus((element.operation.status).toString());
                }
            }

        });
        this.props.conflicts.forEach(c => {
            if (event.id === c) {
                style.borderColor = "red";
                style.borderWidth = "thick";
                style.zIndex = "200";
            }
        });

        return {
            style: style
        };
    }

    showDetailsSurgery = (event) => {
        var resultDataOfSurgery;
        if (event.title.indexOf('clean') >= 0) {
            $("#UpdateCleanModal").modal('show');
            resultDataOfSurgery = this.props.operationQueue.operationsList.find(surgery => surgery.operationId === event.id);
        }
        else {
            resultDataOfSurgery=this.findAppointmentByEvent(event);
            this.setState({ moreDetailes: false, isShowDetailsModal: true });
            this.state.overSurgery.push(event.id)
        }
        this.setState({ resultDetailsModal: resultDataOfSurgery });

    }

    findAppointmentByEvent=(event)=>
    {
        var resultDataOfSurgery;
        if (!(event.title.startsWith('C'))) {//no conflict
            resultDataOfSurgery = this.props.operationQueue.operationsList.find(surgery => surgery.operationId === event.id);
        }
        else {
            var conflictsByRoom = this.props.operationQueue.conflictList.filter(conflictAppointment => {
                if ((conflictAppointment.appointmentA.operation.roomId === event.resourceId) ||
                    (conflictAppointment.appointmentB.operation.roomId === event.resourceId)) {
                    return conflictAppointment;
                }
            });
            conflictsByRoom.forEach(element => {
                if (element.appointmentA.operation.roomId === event.resourceId) {
                    if (new Date(element.appointmentA.beginTime).getTime() === event.start.getTime()
                        && new Date(element.appointmentA.endTime).getTime() === event.end.getTime()) {
                        resultDataOfSurgery = element.appointmentA;
                    }
                }
                else {
                    if (new Date(element.appointmentB.beginTime).getTime() === event.start.getTime()
                        && new Date(element.appointmentB.endTime).getTime() === event.end.getTime()) {
                        resultDataOfSurgery = element.appointmentB;
                    }
                }
            });
        }
        return resultDataOfSurgery;
    }
    // modal start:

    onCloseModal = () => {
        this.setState({ isShowDetailsModal: false });
    }
    load = () => {
        this.setState({ loading: !this.state.loading });
    }
    getFadeInput() {
        if (this.state.fadeStatuse) {
            $('#seletcCancelationReasons').fadeIn(1000);
        }
        else {
            $('#seletcCancelationReasons').fadeOut(1000);
        }
        this.setState({ fadeStatuse: !this.state.fadeStatuse });
    }

    getValueIdFromE(e) {
        let valueInput = e.target.value;
        let reasonId = e.target[e.target.selectedIndex].id;
        console.log("value: ", valueInput, " reasonId: ", reasonId)
        if (reasonId !== 0) {
            this.setState({ isCancelModal: true, reasonId: reasonId, reasonText: valueInput })
        }
        else {
            this.setState({ reasonId: "", reasonText: "" })
        }
    }

    onCloseCancelingModal = () => {
        this.setState({ isCancelModal: false, reasonId: "", reasonText: "" });
    }

    saveCancelingOperation() {
        let currentOperatin = this.state.resultDetailsModal;
        let data = {
            OperationId: this.state.resultDetailsModal.operation.id,
            ReasonId: parseInt(this.state.reasonId)
        };

        http.post("api/Operations/CancelOperationById", data)
            .then(res => {
                if (res !== null) {
                    console.log("res: ", res.entity)
                    currentOperatin.operation.cancelationReasonId = res.entity.cancelationReasonId
                    currentOperatin.operation.status = res.entity.status
                    currentOperatin.operation.statusCanceled = true
                    currentOperatin.operation.canceledDate = res.entity.canceledDate
                    this.setState({ isCancelModal: false, isShowDetailsModal: false, resultDetailsModal: currentOperatin });
                }
            })
            .catch(error => { console.log("Error:", error) });
    }



    saveOperationStatus = (appointment) => {
        this.state.operationStore.changStatuseOperation(this.props.operationQueue,appointment,this.props.state,this.props.rescheduleSetState,this.props.AddSurgery);
        this.onCloseModal();
    }

    cancelAppoinment = (appointment) => {
        var operationQueue=this.props.state==="O"?this.props.operationQueue:this.props.operationQueueOrig;
        appointment.operation.status = 7;//status cancel
        this.state.operationStore.cancelApointment(operationQueue,appointment,this.props.state,this.props.rescheduleSetState,this.props.AddSurgery);
        this.onCloseModal();
    }

    removeApoointment = (appointment) => {
        this.state.operationStore.deleteApointment(this.props.operationQueue,appointment,this.props.state,this.props.rescheduleSetState,this.props.AddSurgery);
        this.onCloseModal();
    }
    performedAppointment = (appointment) => {
        var operationQueue=this.props.state==="O"?this.props.operationQueue:this.props.operationQueueOrig;
        appointment.operation.status = 8;
        this.state.operationStore.changStatuseOperation(operationQueue,appointment,this.props.state,this.props.rescheduleSetState,this.props.AddSurgery);
        this.onCloseModal();
    }
    openMinimalKitModal = () => {

        this.setState({ showMKmodal: true });
    }

    updateDelayApointment = (appointment) => {
        this.state.operationStore.updateDelayApointment(appointment);
        this.props.updateDelayApointment(appointment)
    }

    /*TO Complete:
    updateMinimalKit(status, mkLists) {
        let oldDetails = this.state.resultDetailsModal
        ...
        console.log("status: ", status, ", mkLists: ", updateMK)
    }*/

    showModal = () => {
        var t = this.props.t;
        let duration = this.state.resultDetailsModal.operation.duration;
        let startTimeMinutes = new Date(this.state.resultDetailsModal.beginTime).getMinutes();
        let displayEnd = new Date(this.state.resultDetailsModal.beginTime)
        let endTimeMinutes;
        let endTimeHours;
        if (duration !== null || duration !== undefined || duration !== "") {
            displayEnd.setMinutes(displayEnd.getMinutes() + duration);
            endTimeHours = displayEnd.getHours();
            endTimeMinutes = displayEnd.getMinutes();
        }
        else {
            endTimeHours = new Date(this.state.resultDetailsModal.endTime).getHours()
            endTimeMinutes = new Date(this.state.resultDetailsModal.endTime).getMinutes()
        }

        if (startTimeMinutes <= 9)
            startTimeMinutes = "0" + startTimeMinutes;

        if (endTimeMinutes <= 9)
            endTimeMinutes = "0" + endTimeMinutes;


        return <div open={this.state.isShowDetailsModal} onClose={this.onCloseModal} className="modal d-block" tabIndex="-1" role="dialog" data-backdrop="false">
            <div className="modal-dialog direction " role="document" >
                <div className="modal-content modal" >
                    <div className="modal-header border-1 py-0 row">
                        <p className="fs-14 fs-14 col-3 m-0 p-0">
                            <i className=" pe-7s-date fs-16  basicColorRed p-2 ml-0 "></i>
                            {new Date(this.state.resultDetailsModal.beginTime).toLocaleDateString()}
                        </p>
                        <div className="col-3"></div>
                        {this.state.resultDetailsModal.operation.status !== 7 && this.state.resultDetailsModal.operation.status !== 8 ?
                            <div className="col-1 mt-2">
                                <i id="edit" className="fs-16 pe-7s-note  basicColorRed" title={t("Users.edit")} data-toggle="modal" data-target="#EditSurgeryModal" style={{ cursor: "pointer" }} onClick={this.openModal} />
                            </div> : ""}
                        <div className="col-1 mt-2">
                            <i id="delete" className="fs-16 pe-7s-trash  basicColorRed" data-toggle="tooltip" title={t("Users.delete")}
                                style={{ cursor: "pointer" }} onClick={evnt => { this.removeApoointment(this.state.resultDetailsModal) }} />
                        </div>
                        {this.state.resultDetailsModal.operation.status !== 7 && this.state.resultDetailsModal.operation.status !== 8 ?
                            <div className="col-1 mt-2">
                                <i id="cancel" className="pe-7s-close-circle  basicColorRed" data-toggle="tooltip" title={t("Users.cancel")} style={{ cursor: "pointer" }} onClick={(evt) => { this.cancelAppoinment(this.state.resultDetailsModal) }} />
                            </div> : ""}
                        {this.state.resultDetailsModal.operation.status !== 7 && this.state.resultDetailsModal.operation.status !== 8 ?
                            <div className="col-1 mt-2">
                                <i id="done" className="pe-7s-check  basicColorRed" data-toggle="tooltip" title={t("Users.done")} style={{ cursor: "pointer" }} onClick={() => {
                                    this.performedAppointment(this.state.resultDetailsModal);
                                }} />
                            </div> : ""}
                        {this.state.resultDetailsModal.operation.status !== 7 && this.state.resultDetailsModal.operation.status !== 8
                            && (this.state.resultDetailsModal.appointmentTasks && this.state.resultDetailsModal.appointmentTasks.length || this.state.resultDetailsModal.appointmentDocs && this.state.resultDetailsModal.appointmentDocs.length
                                || this.state.resultDetailsModal.appointmentTests && this.state.resultDetailsModal.appointmentTests.length) ?
                            <div className="col-1 mt-2" data-toggle="tooltip" title={t("Users.minimalkit")}>
                                <i id="miniMalkit" className="fs-16 pe-7s-note2  basicColorRed" /* data-target="#MinimalKitModal" data-toggle="modal" */ style={{ cursor: "pointer" }} onClick={this.openMinimalKitModal} />
                            </div> : ""}
                        <div className="col-1">

                            <button type="button" className="close " data-dismiss="modal" onClick={this.onCloseModal}>
                                &times;
                            </button>
                        </div>

                    </div>
                    <div className="modal-body font-pr fs-14 row">
                        <div className="col-10">
                            {
                                this.state.loading ?
                                    <div className="ProgressStyle ">
                                        <PropagateLoader color={'#3c519d'} />
                                    </div>
                                    :
                                    <span />
                            }
                            <div className='row m-0 p-0 mt-2'>
                                <p className="mb-0">
                                    <i className="pe-7s-graph3 fs-16  basicColorBlue p-2 ml-0"></i>
                                    Priority {this.state.resultDetailsModal.operation.priority ?
                                        this.state.resultDetailsModal.operation.priority : ""}</p>
                            </div>
                            <div className='row m-0 p-0 d-flex justify-content-between'>
                                <p className="mb-0">
                                    <i className=" pe-7s-user fs-16  basicColorBlue p-2 ml-0"></i>
                                    {this.state.resultDetailsModal.fellow.fullName} </p>
                            </div>
                            <div className='row m-0 p-0'>
                                <p className="mb-0">
                                    <i className=" pe-7s-display1 fs-16  basicColorBlue p-2 ml-0"></i>
                                    {this.state.resultDetailsModal.serviceType.description}</p>
                            </div>
                            <div className='row m-0 p-0'>
                                <p className="mb-0">
                                    <i className="pe-7s-id fs-16  basicColorBlue p-2 ml-0"></i>
                                    Surgeon {this.state.resultDetailsModal.operation.surgeon.fullName}</p>
                            </div>
                            <div className='row m-0 p-0 '>
                                <p className="mb-0">
                                    <i className="pe-7s-alarm fs-16  basicColorBlue p-2 ml-0"></i>
                                    {` 
                                ${new Date(this.state.resultDetailsModal.beginTime).getHours()}:${startTimeMinutes}
                                - ${endTimeHours}:${endTimeMinutes}`}
                                </p>
                            </div>
                            {this.state.resultDetailsModal.operation.delay ?
                                <div className='row m-0 p-0 '>
                                    <p className="mb-0">
                                        <i className=" pe-7s-timer fs-16  basicColorBlue p-2 ml-0"></i>
                                        Expected delay of {this.state.resultDetailsModal.operation.delay > 0 ? this.state.resultDetailsModal.operation.delay : 0} minutes </p>
                                </div> : ""}


                            {this.state.moreDetailes ?
                                <div>
                                    {this.state.resultDetailsModal.operation.duration ?
                                        <div className='row m-0 p-0 '>
                                            <p className="mb-0">
                                                <i className="pe-7s-clock fs-16  basicColorBlue p-2 ml-0"></i>
                                                Duration of surgery {this.state.resultDetailsModal.operation.duration} minutes</p>
                                        </div> : ""}
                                    <div className='row m-0 p-0'>
                                        <p className="mb-0">
                                            <i className="pe-7s-map-marker fs-16  basicColorBlue p-2 ml-0"></i>
                                            Room {this.state.resultDetailsModal.operation.roomId<10?this.state.resultDetailsModal.operation.roomId:this.state.resultDetailsModal.operation.roomId-10}</p>
                                    </div>

                                    <div className='row m-0 p-0'>
                                        <p className="mb-0">
                                            <i className="pe-7s-date  basicColorBlue p-2 ml-0"></i>
                                            Fellow age {this.state.resultDetailsModal.fellow.age?this.state.resultDetailsModal.fellow.age:
                                            Math.floor((new Date() - new Date(this.state.resultDetailsModal.fellow.birthDate).getTime()) / 3.15576e+10) + 1}</p>
                                    </div>
                                    {this.state.resultDetailsModal.operation.nurse ?
                                        <div className='row m-0 p-0'>
                                            <p className="mb-0">
                                                <i className="pe-7s-id fs-16  basicColorBlue p-2 ml-0"></i>
                                                Nurse {this.state.resultDetailsModal.operation.nurse.fullName}</p>
                                        </div> : ""}
                                    <div className='row m-0 p-0'>
                                        <p className="mb-0">
                                            <i className="pe-7s-id fs-16  basicColorBlue p-2 ml-0"></i>
                                            Anesthetic {this.state.resultDetailsModal.operation.anesthesiologist.fullName}</p>
                                    </div>
                                </div>
                                : ""}
                        </div>
                        {this.state.resultDetailsModal.operation.status !== 8 && this.state.resultDetailsModal.operation.status !== 7 ?
                            <div className='col-2 m-0 p-0 d-flex justify-content-between'>
                                {/* מוכנות לניתוח */}
                                <PermissionForSurgery
                                    appointment={this.state.resultDetailsModal}
                                    statusePmk={this.state.resultDetailsModal.operation.isMkReady}
                                    statuseTeam={this.state.resultDetailsModal.operation.isTeamReady}
                                    statuseEqp={this.state.resultDetailsModal.operation.isEqpReady}
                                    t={this.props.t}
                                    equipments={this.props.equipments}
                                    updateAppointment={this.props.updateAppointment}
                                    onCloseModal={this.onCloseModal}
                                    load={this.load}
                                // updateMinimalKit={this.updateMinimalKit}
                                ></PermissionForSurgery>
                            </div> : ""}
                    </div>
                    {!this.state.moreDetailes ?
                        <div className='row m-0 p-0  mt-2 justify-content-center'>
                            <i className="pe-7s-angle-down fs-24 basicColorBlue p-0 m-0 text-center" onClick={this.AddDetailsToModalSurgery}></i>
                            <p className='text-center m-0 p-0 mt-1'>
                                More details
                            </p>
                        </div> :
                        <div className='row m-0 p-0  mt-2 justify-content-center'>
                            <i className="pe-7s-angle-up fs-24 basicColorBlue p-0 m-0 text-center" onClick={this.AddDetailsToModalSurgery}></i>
                            <p className='text-center m-0 p-0 mt-1'>
                                Less details
                                </p>
                        </div>
                    }
                    {/* WE BE IN THE FUTURE: 
                    {
                        this.state.showMKmodal ?
                            <MinimalKit
                                appointment={this.state.resultDetailsModal}
                                t={this.props.t}
                            />
                            :
                            ""
                    } 
                    */}

                    {
                        <EditSurgeryModal
                            appointment={this.state.resultDetailsModal}
                            saveStatus={this.saveOperationStatus}
                            updateDelayApointment={this.updateDelayApointment}
                            serviceProviders={this.props.serviceProviders}
                            serviceTypes={this.props.serviceTypes}
                            fellows={this.props.fellows}
                            rooms={this.props.rooms}
                            departments={this.props.departments.departmentsList}
                            t={this.props.t}
                            operationQueue={this.props.operationQueue}
                            state={this.props.state}
                            AddSurgery={this.props.AddSurgery}
                            loading={this.props.loading}
                            onCloseModal={this.onCloseModal}
                            rescheduleSetState={this.props.rescheduleSetState}
                            equipments={this.props.equipments}
                        ></EditSurgeryModal>
                    }




                </div>
            </div>
        </div >
    }

    AddDetailsToModalSurgery = () => {
        this.setState({ moreDetailes: !this.state.moreDetailes });
    }
    //end modal

    isToday = (date) => {
        var newDate = new Date();
        if (newDate.toLocaleDateString() === date.toLocaleDateString()) {
            return true;
        }
        return false;
    }

    //create event on click calander
    handleSelect = (event) => {
        $("#operationModal").modal('show');
    }


    render() {
        var max = this.props.operationQueue && this.props.operationQueue.operationsList && this.props.operationQueue.operationsList.length != 0 ?
            this.props.operationQueue.operationsList.reduce(function (a, b) { return a.endTime > b.endTime ? a : b; }) : null;
        var date = new Date(this.props.operationQueue.serviceQueue.beginTime);
        var Today = this.isToday(date);
        //if some event is end after serviceQ end the calander time end in this time else calander end in serviceQ end
        var maxHour = max && new Date(max.endTime) > new Date(this.props.operationQueue.serviceQueue.endTime) ?
            new Date(max.endTime) : new Date(this.props.operationQueue.serviceQueue.endTime);
        var dep = this.props.departments
        var minHour = new Date(date);
        return (
            <div>
                {this.state.isShowDetailsModal ?
                    this.showModal() : <span></span>}
                <div className="BigCalendarContent calendar-font" dir="ltr">
                    <DragAndDropCalendar
                        //  selectable
                        min={minHour}
                        max={maxHour}
                        events={this.props.eventList}
                        localizer={localizer}
                        defaultView={Views.DAY}
                        views={['day']}
                        step={15}
                        resources={this.props.resourceList}
                        resourceIdAccessor="resourceId"
                        resourceTitleAccessor="resourceTitle"
                        eventPropGetter={(this.getEventStyle)}
                        //  slotPropGetter={(date) =>{this.getSlotStyle}}
                        toolbar={false}
                        defaultDate={date}
                        date={Today ? new Date() : date}
                        onSelectEvent={this.showDetailsSurgery}
                        scroll={true}
                        onEventDrop={this.moveEvent}

                    //onSelectSlot={this.handleSelect}

                    // onEventDrop={this.moveEvent}

                    />
                </div>
                <UpdateCleanModal
                    t={this.props.t}

                ></UpdateCleanModal>
            </div>
        )
    }
}
export default CalanderSurgery;

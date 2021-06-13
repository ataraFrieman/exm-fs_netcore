import React, { Component } from 'react';
import * as $ from "jquery";
import * as http from '../../helpers/Http';
import { withI18n } from "react-i18next";
// import { t } from 'i18next';
import { getDateYYYMMDD, getTime } from '../../helpers/TimeService';
import { getOrganizationId } from '../../helpers/AccountService';
import InputField from './Input'

export class SurgeryModal extends Component {
    constructor(props) {
        super(props)
        console.log("props SurgeryModal: ", props)
        this.state = {
            fellows: this.props.fellows,
            surgeons: this.props.serviceProviders,
            serviceTypes: this.props.serviceTypes,
            nurses: this.props.serviceProviders,
            anesthesiologists: this.props.serviceProviders,
            cleaningTeams: this.props.serviceProviders,
            rooms: this.props.rooms.serviceStationList,
            departments: this.props.departments.departmentsList,
            HostingDepartmentCode: "",
            HostingDepartmentName: "",
            SurgicalDepartmentCode: "",
            SurgicalDepartmentName: "",
            NursingUnitName: "",
            NursingUnitCode: "",
            NursingUnitId: "",
            HostingDepartmentId: "",
            SurgicalDepartmentId: "",
            IsTraining: "",
            SelectedTraining: false,
            LocationCode: "",
            LocationName: "",
            OperationTypeName: "",
            OperationTypeId: "",
            Priority: "",
            SelectedPriority: "",
            // OperationDuration: "",
            BeginTime: "",
            Time: "",
            FellowName: "",
            FellowCode: "",
            FellowAge: "",
            FellowWeight: "",
            FellowHeight: "",
            IsFellowDiabetic: "",
            SelectedDiabetic: false,
            IsHBP: "",
            SelectedHBP: false,
            IsXrayDeviceRequired: "",
            SelectedXray: false,
            FellowGender: "",
            SelectedGender: "M",
            SurgeonName: "",
            SurgeonCode: "",
            NurseName: "",
            SurgicalNursCode: "",
            AnesthesiologistName: "",
            AnesthesiologistCode: "",
            CleanTeamName: "",
            CleanTeamCode: "",
            SurgeonId: "",
            SurgicalNursId: "",
            AnesthesiologistId: ""
        }

        this.handelChange = this.handelChange.bind(this);
        this.handelClose = this.handelClose.bind(this);
        this.addSurgery = this.addSurgery.bind(this);
        this.checkInput = this.checkInput.bind(this);


        // window.Operation = this
    }

    _checkValue(valueInput, e) {
        if (valueInput === "")
            this.setState({ [e.target.id]: e.target.value })
        else
            this.setState({ [e.target.id]: parseInt(e.target.value) })

    }

    handelChange(e) {
        let valueInput = e.target.value;
        switch (e.target.id) {
            case "Time":
                let date = this.props.operationQueue.serviceQueue.beginTime.split("T");
                let beginTime = date[0] + "T" + valueInput;

                if (valueInput < date[1].slice(0, 5) || valueInput > new Date(this.props.operationQueue.serviceQueue.endTime).toLocaleTimeString()) {
                    beginTime = "";
                    valueInput = "";
                    alert("Time out of range");
                }
                //== new Date(this.state.BeginTime).toLocaleDateString();
                //console.log("Time", date[0] + "T" + valueInput)
                this.setState({ Time: valueInput, BeginTime: beginTime })
                break;
            //bool
            case "TrainingFalse":
                this.setState({ IsTraining: false, SelectedTraining: false })
                break;
            case "TrainingTrue":
                this.setState({ IsTraining: true, SelectedTraining: true })
                break;
            //int
            case "Priority":
                this.setState({ SelectedPriority: e.target.value })
                break;
            // int
            case "FellowName":
                let id = "", age = "", weight = "", gender = "", height = "";
                for (var i = 0; i < this.state.fellows.FellowsList.length; i++) {
                    if (this.state.fellows.FellowsList[i].fullName === e.target.value) {
                        id = this.state.fellows.FellowsList[i].identityNumber;
                        age = this.state.fellows.FellowsList[i].age;
                        //console.log("age: ", age)
                        weight = this.state.fellows.FellowsList[i].weight ? this.state.fellows.FellowsList[i].weight : null;
                        height = this.state.fellows.FellowsList[i].height ? this.state.fellows.FellowsList[i].height : null;
                        gender = this.state.fellows.FellowsList[i].gender;
                    }
                }
                // if (valueInput != "") {
                //this._checkValue(valueInput, e)
                this.setState({
                    FellowName: valueInput,
                    FellowCode: id,
                    FellowAge: age,
                    FellowWeight: weight,
                    FellowHeight: height,
                    FellowGender: gender !== "" ? gender : "M",
                    SelectedGender: gender !== "" ? gender : "M"
                })
                // }
                break;
            // int
            case "FellowCode":
                if (valueInput.length >= 0 && valueInput.length <= 10) {
                    this._checkValue(valueInput, e)
                }
                else {
                    alert("Unvalid Code!");
                }
                break;
            // int
            case "FellowAge":
                if (valueInput >= 0 && valueInput <= 120) {
                    this._checkValue(valueInput, e)
                }
                else {
                    alert("Unvalid Age!");
                }
                break;
            // int
            case "FellowWeight":
                if (valueInput >= 0 && valueInput <= 300) {
                    this._checkValue(valueInput, e)
                }
                else {
                    alert("Unvalid Weight!");
                }
                break;
            // int
            case "FellowHeight":
                if(valueInput >= 0 && valueInput <= 300) {
                    this._checkValue(valueInput, e)
                } else {
                    alert("Unvalid Height!");
                }
            //bool
            case "DiabeticFalse":
                this.setState({ IsFellowDiabetic: false, SelectedDiabetic: false })
                break;
            case "DiabeticTrue":
                this.setState({ IsFellowDiabetic: true, SelectedDiabetic: true })
                break;
            //bool
            case "HbpFalse":
                this.setState({ IsHBP: false, SelectedHBP: false })
                break;
            case "HbpTrue":
                this.setState({ IsHBP: true, SelectedHBP: true })
                break;
            //bool
            case "XrayFalse":
                this.setState({ IsXrayDeviceRequired: false, SelectedXray: false })
                break;
            case "XrayTrue":
                this.setState({ IsXrayDeviceRequired: true, SelectedXray: true })
                break;
            //string
            case "FellowGenderMale":
                this.setState({ FellowGender: e.target.value, SelectedGender: e.currentTarget.value })
                break;
            case "FellowGenderFemale":
                this.setState({ FellowGender: e.target.value, SelectedGender: e.currentTarget.value })
                break;
            default:
                this.setState({ [e.target.id]: e.target.value })
                break;
        }
    }

    checkInput(e) {
        let isInTheList;
        switch (e.target.id) {
            /*connect it to DB*/
            case "HostingDepartmentName":
                debugger;
                let HostingDepartment_id, HostingDepartment_code;
                for (var i = 0; i < this.state.departments.length; i++) {
                    if (this.state.departments[i].description === e.target.value) {
                        isInTheList = true;
                        HostingDepartment_id = this.state.departments[i].id;
                        HostingDepartment_code = this.state.departments[i].code;
                        break;
                    }
                }
                if (isInTheList) {
                    this.setState({ [e.target.id]: e.target.value, HostingDepartmentCode: `${HostingDepartment_id}`, HostingDepartmentId: HostingDepartment_code })
                }
                else {
                    this.setState({ [e.target.id]: "", HostingDepartmentCode: "" })
                }
                break;
            case "NursingUnitName":
                let NursingUnit_id, NursingUnit_code;
                for (i = 0; i < this.state.departments.length; i++) {
                    if (this.state.departments[i].description === e.target.value) {
                        // console.log("room: " + e.target.value)
                        isInTheList = true;
                        NursingUnit_id = this.state.departments[i].id;
                        NursingUnit_code = this.state.departments[i].code;
                        break;
                    }
                }
                if (isInTheList) {
                    this.setState({ [e.target.id]: e.target.value, NursingUnitCode: `${NursingUnit_id}`, NursingUnitId: `${NursingUnit_code}` })
                }
                else {
                    this.setState({ [e.target.id]: "", NursingUnitCode: "" })
                }
                break;
            // int

            /*connect it to DB*/
            case "SurgicalDepartmentName":
                let SurgicalDepartment_id, SurgicalDepartment_code;
                for (i = 0; i < this.state.departments.length; i++) {
                    if (this.state.departments[i].description === e.target.value) {
                        // console.log("room: " + e.target.value)
                        isInTheList = true;
                        SurgicalDepartment_id = this.state.departments[i].id;
                        SurgicalDepartment_code = this.state.departments[i].code;
                        break;
                    }
                }
                if (isInTheList) {
                    this.setState({ [e.target.id]: e.target.value, SurgicalDepartmentCode: `${SurgicalDepartment_id}`, SurgicalDepartmentId: SurgicalDepartment_code })
                }
                else {
                    this.setState({ [e.target.id]: "", SurgicalDepartmentCode: "" })
                }
                break;
            // int
            case "Priority": {
                // if ((valueInput > 0 && valueInput <= 3) || valueInput == "" || valueInput == '')
                //     this.setState({ [e.target.id]: e.target.value })
                if (e.target.value === "Lifesaver") {
                    this.setState({ [e.target.id]: 1, SelectedPriority: e.target.value })
                    break;
                }
                else if (e.target.value === "Emergency") {
                    this.setState({ [e.target.id]: 2, SelectedPriority: e.target.value })
                    break;
                }
                else if (e.target.value === "Regular") {
                    this.setState({ [e.target.id]: 3, SelectedPriority: e.target.value })
                    break;
                }
                else {
                    // alert("Unvalid Priority!");
                    this.setState({ [e.target.id]: "", SelectedPriority: "" })
                    break;
                }
            }

            /*connect it to DB*/
            case "LocationName":
                let room_id;
                debugger
                for (i = 0; i < this.state.rooms.length; i++) {
                    if (this.state.rooms[i].description === e.target.value) {
                        // console.log("room: " + e.target.value)
                        isInTheList = true;
                        room_id = this.state.rooms[i].id
                        break;
                    }
                }
                if (isInTheList) {
                    this.setState({ [e.target.id]: e.target.value, LocationCode: room_id })
                }
                else {
                    this.setState({ [e.target.id]: "", LocationCode: "" })
                    // alert("Unvalid Room number!");
                    // this.setState({ [e.target.id]: e.target.value, LocationCode: "" })
                }
                break;

            case "OperationTypeName":
                let st_id;
                for (i = 0; i < this.state.serviceTypes.serviceTypesList.length; i++) {
                    if (this.state.serviceTypes.serviceTypesList[i].description === e.target.value) {
                        // console.log("ST: " + this.state.serviceTypes[i].value)
                        isInTheList = true;
                        st_id = this.state.serviceTypes.serviceTypesList[i].code;
                        break;
                    }
                }
                if (isInTheList) {
                    this.setState({ [e.target.id]: e.target.value, OperationTypeId: `${st_id}` })
                }
                else {
                    this.setState({ [e.target.id]: "", OperationTypeId: "" })
                    // alert("Unvalid ST number!");
                }
                break;
            case "SurgeonName":
                let surgeon_id, surgeon_code;
                for (i = 0; i < this.state.surgeons.length; i++) {
                    if (this.state.surgeons[i].fullName === e.target.value) {
                        // console.log("surgeon: " + this.state.surgeons[i])
                        isInTheList = true;
                        surgeon_id = this.state.surgeons[i].id;
                        surgeon_code = this.state.surgeons[i].identityNumber;
                        break;
                    }
                }
                if (isInTheList) {
                    this.setState({ [e.target.id]: e.target.value, SurgeonCode: surgeon_id, SurgeonId: surgeon_code })
                }
                else {
                    this.setState({ [e.target.id]: "", SurgeonCode: "" })
                    // alert("Unvalid surgeon number!");
                }
                break;
            case "NurseName":
                let nurse_id, nurse_code;
                for (i = 0; i < this.state.nurses.length; i++) {
                    if (this.state.nurses[i].fullName === e.target.value) {
                        // console.log("Nurse: " + this.state.nurses[i].id)
                        isInTheList = true;
                        nurse_id = this.state.nurses[i].id;
                        nurse_code = this.state.nurses[i].identityNumber;
                        break;
                    }
                }
                if (isInTheList) {
                    this.setState({ [e.target.id]: e.target.value, SurgicalNursCode: nurse_id, SurgicalNursId: nurse_code })
                }
                else {
                    this.setState({ [e.target.id]: "", SurgicalNursCode: "" })
                    // alert("Unvalid nurse number!");
                }
                break;
            case "AnesthesiologistName":
                let anes_id, anes_code;
                for (i = 0; i < this.state.anesthesiologists.length; i++) {
                    if (this.state.anesthesiologists[i].fullName === e.target.value) {
                        // console.log("anes: " + this.state.anesthesiologists[i].id)
                        isInTheList = true;
                        anes_id = this.state.anesthesiologists[i].id;
                        anes_code = this.state.anesthesiologists[i].identityNumber;
                        break;
                    }
                }
                if (isInTheList) {
                    this.setState({ [e.target.id]: e.target.value, AnesthesiologistCode: anes_id, AnesthesiologistId: anes_code })
                }
                else {
                    this.setState({ [e.target.id]: "", AnesthesiologistCode: "" })
                    // alert("Unvalid anesthesiologist number!");
                }
                break;
            case "CleanTeamName":
                let clt_id;
                for (i = 0; i < this.state.cleaningTeams.length; i++) {
                    if (this.state.cleaningTeams[i].fullName === e.target.value) {
                        // console.log("clt: " + this.state.cleaningTeams[i].id)
                        isInTheList = true;
                        clt_id = this.state.cleaningTeams[i].id;
                        break;
                    }
                }
                if (isInTheList) {
                    this.setState({ [e.target.id]: e.target.value, CleanTeamCode: clt_id })
                }
                else {
                    this.setState({ [e.target.id]: "", CleanTeamCode: "" })
                    // alert("Unvalid Cleaning Teams number!");
                }
                break;
        }
    }

    addSurgery() {
        var data = {
            BeginTime: this.state.BeginTime,                       //Required
            Code: 1000,                                            //Required
            HostingDepartmentCode: this.state.HostingDepartmentCode, //Required
            SurgicalDepartmentCode: this.state.SurgicalDepartmentCode, //Required
            NursingUnitDepartmentCode: this.state.NursingUnitCode, //Required
            HostingDepartmentId: this.state.HostingDepartmentId,
            SurgicalDepartmentId: this.state.SurgicalDepartmentId,
            NursingUnitDepartmentId: this.state.NursingUnitId,
            IsTraining: this.state.SelectedTraining,
            LocationCode: this.state.LocationCode,                 //Required
            OperationTypeCode: this.state.OperationTypeId,         //Required
            Priority: this.state.Priority !== "" ? this.state.Priority : 3, //Required
            // OperationDuration: this.state.OperationDuration,       //Required
            FellowName: this.state.FellowName,                     //Required
            FellowCode: this.state.FellowCode,                     //Required
            FellowAge: this.state.FellowAge,                       //Required
            FellowWeight: this.state.FellowWeight !== "" ? this.state.FellowWeight : 0,
            FellowHeight: this.state.FellowHeight !== "" ? this.state.FellowHeight : 0,
            IsFellowDiabetic: this.state.SelectedDiabetic,
            IsHBP: this.state.SelectedHBP,                         //Required
            IsXrayDeviceRequired: this.state.SelectedXray,
            FellowGender: this.state.SelectedGender,                //Required
            SurgeonCode: this.state.SurgeonCode,                   //Required
            SurgicalNursCode: this.state.SurgicalNursCode !== "" ? this.state.SurgicalNursCode : 0,
            AnesthesiologistCode: this.state.AnesthesiologistCode, //Required
            SurgeonId: this.state.SurgeonId,                   //Required
            SurgicalNursId: this.state.SurgicalNursCode !== "" ? this.state.SurgicalNursId : null,
            AnesthesiologistId: this.state.AnesthesiologistId, //Required
            CleanTeamCode: this.state.CleanTeamCode !== "" ? this.state.CleanTeamCode : 0
        };
        //== new Date(this.state.BeginTime).toLocaleDateString();
        var isOperationQueu = new Date(this.props.operationQueue.serviceQueue.beginTime).toLocaleDateString() === new Date(this.state.BeginTime).toLocaleDateString();
        let request = {
            Entity: data,
            //BeginTime: new Date(this.state.BeginTime).toLocaleString(),
            ServiceQueueId: this.props.serviceQueueId !== -1 && isOperationQueu ? this.props.serviceQueueId : -1,
            SortOrderByBeginTime: false,
            OperationQueue: this.props.operationQueue && isOperationQueu ? this.props.operationQueue : null,
            State: this.props.state
        }

        let count = 0
        //const tifOptions = Object.keys(data.OperationsList[0]).map(key => {
        // if (data.OperationsList[0][key] === "") {
        const tifOptions = Object.keys(data).map(key => {
            if (data[key] !== "FellowWeight" || data[key] !== "FellowHeight" || data[key] !== "CleanTeamCode")
                if (data[key] === "") {
                    console.log("err, ", key)
                    count++;
                }
        })
        if (count === 0) {
            $("#operationModal").modal('hide');
            this.setState({
                HostingDepartmentCode: "",
                HostingDepartmentName: "",
                SurgicalDepartmentCode: "",
                SurgicalDepartmentName: "",
                NursingUnitName: "",
                IsTraining: "",
                SelectedTraining: false,
                LocationCode: "",
                LocationName: "",
                OperationTypeName: "",
                OperationTypeId: "",
                Priority: "",
                SelectedPriority: "",
                // OperationDuration: "",
                BeginTime: "",
                Time: "",
                FellowName: "",
                FellowCode: "",
                FellowAge: "",
                FellowWeight: "",
                FellowHeight: "",
                IsFellowDiabetic: "",
                SelectedDiabetic: false,
                IsHBP: "",
                SelectedHBP: false,
                IsXrayDeviceRequired: "",
                SelectedXray: false,
                FellowGender: "",
                SelectedGender: "M",
                SurgeonName: "",
                SurgeonCode: "",
                NurseName: "",
                SurgicalNursCode: "",
                AnesthesiologistName: "",
                AnesthesiologistCode: "",
                CleanTeamName: "",
                CleanTeamCode: ""
            })

            this.props.load();
            http.post('api/Operations/AddOperation', request)
                .then(res => {
                    console.log(res);
                    this.props.AddSurgery(res);
                    // var fellowExsist = this.props.fellows.find((f) => {
                    //    return f.id ===this.state.FellowCode;
                    // })
                    // if (!fellowExsist)
                    //     this.props.fellows.push({})
                })
                .catch(err => {
                    console.log("Error: ", err)
                    //this.setState({ loading: false })
                })

        }
        else {
            alert("Missing details")
        }
    }

    handelClose() {
        this.setState({
            HostingDepartmentCode: "",
            HostingDepartmentName: "",
            SurgicalDepartmentCode: "",
            SurgicalDepartmentName: "",
            NursingUnitName: "",
            IsTraining: "",
            SelectedTraining: false,
            LocationCode: "",
            LocationName: "",
            OperationTypeName: "",
            OperationTypeId: "",
            Priority: "",
            SelectedPriority: "",
            // OperationDuration: "",
            BeginTime: "",
            Time: "",
            FellowName: "",
            FellowCode: "",
            FellowAge: "",
            FellowWeight: "",
            FellowHeight: "",
            IsFellowDiabetic: "",
            SelectedDiabetic: false,
            IsHBP: "",
            SelectedHBP: false,
            IsXrayDeviceRequired: "",
            SelectedXray: false,
            FellowGender: "",
            SelectedGender: "M",
            SurgeonName: "",
            SurgeonCode: "",
            NurseName: "",
            SurgicalNursCode: "",
            AnesthesiologistName: "",
            AnesthesiologistCode: "",
            CleanTeamName: "",
            CleanTeamCode: ""
        })
    }

    render() {
        var t = this.props.t
        return (
            <div className="modal fade" id="operationModal" tabIndex="-1" role="dialog" data-backdrop="false" aria-hidden="true" >
                {/* modal-dialog-scrollable */}
                <div className="modal-dialog modal-xl rounded-0" role="document" style={{ marginTop: "45px", textAlign: "left" }}>
                    <div className="modal-content" >

                        <div className="modal-header p-1">
                            <h5 className="modal-title p-1 pr-3 fc-gray font-weight-light font-pr basicColorBlue">
                                {t("surgeryModal.addOperation")}
                            </h5>
                            <span className="col-5">
                                <label className="mb-0">
                                    <span style={{ color: "red", margin: "5px" }}>*</span>
                                    {t("surgeryModal.time")}
                                </label>
                                <input
                                    //className="col-6" 
                                    //label={t("surgeryModal.time")}
                                    //span="*"
                                    //style={{ color: "red", margin: "5px" }}
                                    //type="datetime-local" //"ex: 2019-11-18T03:00"
                                    //id="BeginTime"
                                    //value={this.state.BeginTime}
                                    type="time"
                                    id="Time"
                                    min={this.props.operationQueue.serviceQueue.beginTime.split("T")[1]}
                                    max={this.props.operationQueue.serviceQueue.endTime.split("T")[1]}
                                    value={this.state.Time}
                                    className="timeInput font-pr basicColorBlue"
                                    autoComplete=""
                                    onChange={this.handelChange}
                                />
                            </span>
                            <div className="row m-0 p-0 ">
                                <button type="button" className="close m-0 p-0 col-1" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true" onClick={this.handelClose}>&times;</span>
                                </button>
                            </div>
                        </div>

                        <div className="modal-body font-pr ">
                            <div className="row">
                                {/* surgery details */}
                                <div className="col-4">
                                    <div className="row p-2">
                                        <i className="fs-24 pe-7s-timer col-1 icon m-0 p-0"></i>
                                        <h5 className="basicColorBlue"> {t("surgeryModal.surgeryDetails")}</h5>
                                    </div>

                                    <div className="row p-1">
                                        <form autoComplete="off">
                                            <label className="mb-0">
                                                <span style={{ color: "red", margin: "5px" }}>*</span>
                                                {t("surgeryModal.hostingDepartment")}
                                            </label>
                                            <input type="search" id="HostingDepartmentName" className="form-control txtAL" list="HostingDep" value={this.state.HostingDepartmentName} onChange={this.handelChange} onBlur={e => { this.checkInput(e) }} />
                                        </form>

                                        <datalist id="HostingDep">
                                            {

                                                this.state.departments ?
                                                    this.state.departments.map((item, i) => {
                                                        if (item.departmentsTypesId === 7)
                                                            return <option key={i}>{item.description}</option>
                                                    })
                                                    :
                                                    "No results"
                                            }
                                        </datalist>
                                    </div>

                                    <div className="row p-1">
                                        <form autoComplete="off">
                                            <label className="mb-0">
                                                <span style={{ color: "red", margin: "5px" }}>*</span>
                                                {t("surgeryModal.surgicalDepartment")}</label>
                                            <input type="search" id="SurgicalDepartmentName" className="form-control txtAL" list="SurgicalDep" value={this.state.SurgicalDepartmentName} onChange={this.handelChange} onBlur={e => { this.checkInput(e) }} />
                                        </form>
                                        <datalist id="SurgicalDep">
                                            {

                                                this.state.departments ?
                                                    this.state.departments.map((item, i) => {
                                                        if (item.departmentsTypesId === 6)
                                                            return <option key={i}>{item.description}</option>
                                                    })
                                                    :
                                                    "No results"
                                            }
                                        </datalist>
                                    </div>

                                    <div className="row p-1">
                                        <form autoComplete="off">
                                            <label className="mb-0">
                                                <span style={{ color: "red", margin: "5px" }}>*</span>
                                                {t("surgeryModal.NursingUnit")}</label>
                                            <input type="search" id="NursingUnitName" className="form-control txtAL" list="NursingUnit" value={this.state.NursingUnitName} onChange={this.handelChange} onBlur={e => { this.checkInput(e) }} />
                                        </form>
                                        <datalist id="NursingUnit">
                                            {

                                                this.state.departments ?
                                                    this.state.departments.map((item, i) => {
                                                        if (item.departmentsTypesId === 8)
                                                            return <option key={i}>{item.description}</option>
                                                    })
                                                    :
                                                    "No results"
                                            }
                                        </datalist>
                                    </div>

                                    <div className="row">
                                        <label className="mb-0">
                                            <span style={{ color: "red", margin: "5px" }}>*</span>
                                            {t("surgeryModal.training")}:</label>
                                        <div className="form-check"> {/* style={{ marginLeft: "-5px" }} || style={{ marginLeft: "0px" }} */}
                                            <input className="form-check-input" name="training" type="radio" id="TrainingFalse" value={false} checked={this.state.SelectedTraining === false} onChange={e => { this.handelChange(e) }} />
                                            <label className="form-check-label" htmlFor="false">{t("surgeryModal.false")}</label>
                                        </div>
                                        <div className="form-check"> {/* style={{ marginLeft: "-5px" }} || style={{ marginLeft: "0px" }} */}
                                            <input className="form-check-input" name="training" type="radio" id="TrainingTrue" value={true} checked={this.state.SelectedTraining === true} onChange={e => { this.handelChange(e) }} />
                                            <label className="form-check-label" htmlFor="true">{t("surgeryModal.true")}</label>
                                        </div>
                                    </div>

                                    <div className="row p-2">
                                        <form autoComplete="off">
                                            <label className="mb-0">
                                                <span style={{ color: "red", margin: "5px" }}>*</span>
                                                {t("surgeryModal.room")}
                                            </label>
                                            <input type="search" id="LocationName" className="form-control clearable txtAL" list="rooms" value={this.state.LocationName} onChange={this.handelChange} onBlur={e => { this.checkInput(e) }} />
                                        </form>
                                        <datalist id="rooms">
                                            {
                                                this.state.rooms ?
                                                    this.state.rooms.map((item, i) => {
                                                        return <option key={i}>{item.description}</option>
                                                    })
                                                    :
                                                    "No results"
                                            }
                                        </datalist>
                                    </div>

                                    <div className="row p-1">
                                        <form autoComplete="off">
                                            <label className="mb-0">
                                                <span style={{ color: "red", margin: "5px" }}>*</span>
                                                {t("surgeryModal.serviceType")}</label>
                                            <input type="search" id="OperationTypeName" className="form-control clearable txtAL" list="servicetypes" value={this.state.OperationTypeName} onChange={this.handelChange} onBlur={e => { this.checkInput(e) }} />
                                        </form>
                                        <datalist id="servicetypes">
                                            {
                                                this.state.serviceTypes.serviceTypesList.length > 0 ?

                                                    this.state.serviceTypes.serviceTypesList.map((item, i) => {
                                                        if (item.description !== "Health")
                                                            return <option key={i}>{item.description}</option>
                                                    })

                                                    :
                                                    ""
                                            }
                                        </datalist>
                                    </div>
                                    {/* <div className="row p-1">
                                        <InputField
                                            label={t("surgeryModal.duration")}
                                            span="*"
                                            style={{ color: "red", margin: "5px" }}
                                            type="number"
                                            id="OperationDuration"
                                            value={this.state.OperationDuration}

                                            className="form-control txtAL"
                                            autoComplete="off"
                                            onChange={this.handelChange}
                                        />
                                    </div> */}

                                    <div className="row p-1">
                                        <form>
                                            <label className="mb-0">
                                                {/* <span style={{ color: "red", margin: "5px" }}>*</span> */}
                                                {t("surgeryModal.priority")}</label>
                                            <input type="search" id="Priority" className="form-control txtAL" list="priorityType" autoComplete="off" value={this.state.SelectedPriority} onChange={this.handelChange} onBlur={e => { this.checkInput(e) }} />
                                        </form>
                                        <datalist id="priorityType">
                                            <option>{t("surgeryModal.lifesaver")}</option>
                                            <option>{t("surgeryModal.emergency")}</option>
                                            <option>{t("surgeryModal.regular")}</option>
                                        </datalist>
                                    </div>

                                </div>





                                <div className="col-4">
                                    <div className="row p-2">
                                        <i className="fs-24 pe-7s-credit col-1 icon m-0 p-0"></i>
                                        <h5 className="basicColorBlue">{t("surgeryModal.fellowDetails")}:</h5>
                                    </div>
                                    <div className="row p-1">
                                        <form autoComplete="off">
                                            <label className="mb-0">
                                                <span style={{ color: "red", margin: "5px" }}>*</span>
                                                {t("surgeryModal.fellowName")}</label>
                                            <input type="search" id="FellowName" className="form-control txtAL" list="fellows" value={this.state.FellowName} onChange={this.handelChange} />
                                        </form>
                                        <datalist id="fellows">
                                            {
                                                this.state.fellows.FellowsList ?
                                                    this.state.fellows.FellowsList.map((item, i) => {
                                                        return <option key={i}>{item.fullName}</option>
                                                    })
                                                    :
                                                    "No results"
                                            }
                                        </datalist>
                                    </div>

                                    <div className="row p-1">
                                        <InputField
                                            label={t("surgeryModal.fellowCode")}
                                            span="*"
                                            style={{ color: "red", margin: "5px" }}
                                            type="number"
                                            id="FellowCode"
                                            value={this.state.FellowCode}
                                            className="form-control txtAL"
                                            autoComplete="off"
                                            onChange={this.handelChange}
                                        />
                                    </div>
                                    <div className="row p-1">
                                        <InputField
                                            label={t("surgeryModal.fellowAge")}
                                            span="*"
                                            style={{ color: "red", margin: "5px" }}
                                            type="number"
                                            id="FellowAge"
                                            value={this.state.FellowAge}
                                            className="form-control txtAL"
                                            autoComplete="off"
                                            onChange={this.handelChange}
                                        />
                                    </div>
                                    <div className="row p-1">
                                        <InputField
                                            label={t("surgeryModal.fellowWeight")}
                                            span=""
                                            style=""
                                            type="number"
                                            id="FellowWeight"
                                            value={this.state.FellowWeight}
                                            className="form-control txtAL"
                                            autoComplete="off"
                                            onChange={this.handelChange}
                                        />
                                    </div>

                                    <div className="row p-1">
                                        <InputField
                                            label={t("surgeryModal.fellowHeight")}
                                            span=""
                                            style=""
                                            type="number"
                                            id="FellowHeight"
                                            value={this.state.FellowHeight}
                                            className="form-control txtAL"
                                            autoComplete="off"
                                            onChange={this.handelChange}
                                        />
                                    </div>

                                    <div className="row p-2">
                                        <label className="mb-0">
                                            <span style={{ color: "red", margin: "5px" }}>*</span>
                                            {t("surgeryModal.gender")}
                                        </label>
                                        <div className="form-check"> {/* style={{ marginLeft: "-5px" }} || style={{ marginLeft: "0px" }} */}
                                            <input className="form-check-input" name="gender" type="radio" id="FellowGenderMale" value="M" checked={this.state.SelectedGender === "M"} onChange={e => { this.handelChange(e) }} />
                                            <label className="form-check-label" htmlFor="male">{t("surgeryModal.male")}</label>
                                        </div>
                                        <div className="form-check"> {/* style={{ marginLeft: "-5px" }} || style={{ marginLeft: "0px" }} */}
                                            <input className="form-check-input" name="gender" type="radio" id="FellowGenderFemale" value="F" checked={this.state.SelectedGender === "F"} onChange={e => { this.handelChange(e) }} />
                                            <label className="form-check-label" htmlFor="female">{t("surgeryModal.female")}</label>
                                        </div>
                                    </div>

                                    <div className="row p-2">
                                        <label className=" mb-0">
                                            <span style={{ color: "red", margin: "5px" }}>*</span>
                                            {t("surgeryModal.fellowHBP")}
                                        </label>
                                        <div className="form-check"> {/* style={{ marginLeft: "-5px" }} || style={{ marginLeft: "0px" }} */}
                                            <input className="form-check-input" name="HBP" type="radio" id="HbpFalse" value={false} checked={this.state.SelectedHBP === false} onChange={e => { this.handelChange(e) }} />
                                            <label className="form-check-label" htmlFor="false">{t("surgeryModal.false")}</label>
                                        </div>
                                        <div className="form-check"> {/* style={{ marginLeft: "-5px" }} || style={{ marginLeft: "0px" }} */}
                                            <input className="form-check-input" name="HBP" type="radio" id="HbpTrue" value={true} checked={this.state.SelectedHBP === true} onChange={e => { this.handelChange(e) }} />
                                            <label className="form-check-label" htmlFor="true">{t("surgeryModal.true")}</label>
                                        </div>
                                    </div>

                                    <div className="row p-2">
                                        <label className=" mb-0">
                                            {/* <span style={{ color: "red", margin: "5px" }}>*</span> */}
                                            {t("surgeryModal.fellowDiabetes")}
                                        </label>
                                        <div className="form-check"> {/* style={{ marginLeft: "-5px" }} || style={{ marginLeft: "0px" }} */}
                                            <input className="form-check-input" name="diabetes" type="radio" id="DiabeticFalse" value={false} checked={this.state.SelectedDiabetic === false} onChange={e => { this.handelChange(e) }} />
                                            <label className="form-check-label" htmlFor="false">{t("surgeryModal.false")}</label>
                                        </div>
                                        <div className="form-check"> {/* style={{ marginLeft: "-5px" }} || style={{ marginLeft: "0px" }} */}
                                            <input className="form-check-input" name="diabetes" type="radio" id="DiabeticTrue" value={true} checked={this.state.SelectedDiabetic === true} onChange={e => { this.handelChange(e) }} />
                                            <label className="form-check-label" htmlFor="true">{t("surgeryModal.true")}</label>
                                        </div>
                                    </div>

                                    <div className="row p-2">
                                        <label className="mb-0">
                                            {/* <span style={{ color: "red", margin: "5px" }}>*</span> */}
                                            {t("surgeryModal.xray")}</label>
                                        <div className="form-check"> {/* style={{ marginLeft: "-5px" }} || style={{ marginLeft: "0px" }} */}
                                            <input className="form-check-input" name="xray" type="radio" id="XrayFalse" value={false} checked={this.state.SelectedXray === false} onChange={e => { this.handelChange(e) }} />
                                            <label className="form-check-label" htmlFor="false">{t("surgeryModal.false")}</label>
                                        </div>
                                        <div className="form-check"> {/* style={{ marginLeft: "-5px" }} || style={{ marginLeft: "0px" }} */}
                                            <input className="form-check-input" name="xray" type="radio" id="XrayTrue" value={true} checked={this.state.SelectedXray === true} onChange={e => { this.handelChange(e) }} />
                                            <label className="form-check-label" htmlFor="true">{t("surgeryModal.true")}</label>
                                        </div>
                                    </div>

                                </div>


                                <div className="col-4">
                                    <div className="row p-2">
                                        <i className="fs-24 pe-7s-id col-1 icon m-0 p-0"></i>
                                        <h5 className="basicColorBlue">{t("surgeryModal.ServiceProvidersDetails")}:</h5></div>
                                    <div className="row p-1"><form autoComplete="off">
                                        <label className="mb-0">
                                            <span style={{ color: "red", margin: "5px" }}>*</span>
                                            {t("surgeryModal.surgeon")}</label>
                                        <input type="search" id="SurgeonName" className="form-control txtAL" list="surgeons" value={this.state.SurgeonName} onChange={this.handelChange} onBlur={e => { this.checkInput(e) }} />
                                    </form>
                                        <datalist id="surgeons">
                                            {
                                                this.state.surgeons ?
                                                    this.state.surgeons.map((item, i) => {
                                                        if (item.serviceProviderType === 1)
                                                            return <option key={i}>{item.fullName}</option>
                                                    })
                                                    :
                                                    "No results"
                                            }
                                        </datalist></div>
                                    <div className="row p-1">
                                        <form autoComplete="off">
                                            <label className="mb-0">
                                                {/* <span style={{ color: "red", margin: "5px" }}>*</span> */}
                                                {t("surgeryModal.nurse")}</label>
                                            <input type="search" id="NurseName" className="form-control txtAL" list="nurses" value={this.state.NurseName} onChange={this.handelChange} onBlur={e => { this.checkInput(e) }} />
                                        </form>
                                        <datalist id="nurses">
                                            {
                                                this.state.nurses ?
                                                    this.state.nurses.map((item, i) => {
                                                        if (item.serviceProviderType === 3)
                                                            return <option key={i}>{item.fullName}</option>
                                                    })
                                                    :
                                                    "No results"
                                            }
                                        </datalist>
                                    </div>
                                    <div className="row p-1"><form autoComplete="off">
                                        <label className="mb-0">
                                            <span style={{ color: "red", margin: "5px" }}>*</span>
                                            {t("surgeryModal.anesthesiologist")}</label>
                                        <input type="search" id="AnesthesiologistName" className="form-control txtAL" list="anesthesiologists" value={this.state.AnesthesiologistName} onChange={this.handelChange} onBlur={e => { this.checkInput(e) }} />
                                    </form>
                                        <datalist id="anesthesiologists">
                                            {
                                                this.state.anesthesiologists ?
                                                    this.state.anesthesiologists.map((item, i) => {
                                                        if (item.serviceProviderType === 2)
                                                            return <option key={i}>{item.fullName}</option>
                                                    })
                                                    :
                                                    "No results"
                                            }
                                        </datalist></div>
                                    <div className="row p-1"><form autoComplete="off">
                                        {/* <span style={{ color: "red", margin: "5px" }}>*</span> */}
                                        <label className="mb-0">{t("surgeryModal.cleaningTeam")}</label>
                                        <input type="search" id="CleanTeamName" className="form-control txtAL" list="cleaningTeams" value={this.state.CleanTeamName} onChange={this.handelChange} onBlur={e => { this.checkInput(e) }} />
                                    </form>
                                        <datalist id="cleaningTeams">
                                            {
                                                this.state.cleaningTeams ?
                                                    this.state.cleaningTeams.map((item, i) => {
                                                        if (item.serviceProviderType === 4)
                                                            return <option key={i}>{item.fullName}</option>
                                                    })
                                                    :
                                                    "No results"
                                            }
                                        </datalist></div>
                                </div>

                            </div>
                        </div>

                        <div className="modal-footer" style={{ justifyContent: "space-between" }}>
                            <button type="button" className="btn btn-secondary font pr" data-dismiss="modal" onClick={this.handelClose}>{t("surgeryModal.close")}</button>
                            <button type="button" className="btn btn-primary font-pr" onClick={this.addSurgery}>{t("surgeryModal.add")}</button>
                        </div>
                    </div>

                </div >
            </div >
        )
    }
}
export default SurgeryModal

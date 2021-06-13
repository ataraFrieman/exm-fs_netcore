import React, { Component } from 'react';
import { SurgeryDetails } from './SurgeryDetails.js'
import { EditFellowDetails } from './EditFellowDetails.js';
import { ServiceProvidersDetails } from './ServiceProvidersDetails.js';
import { getTime } from '../helpers/TimeService'
import * as http from '../helpers/Http';
import operation from '../stores/Operation';


export class EditSurgeryDetails extends Component {
    constructor(props) {
        super(props)
        var operationStore = operation.fromJS();
        this.state = {
            editMode: "SurgeryDetails",
            BeginTime: this.getBeginTime(),
            Code: "",                  //Required
            HostingDepartmentCode: "", //Required
            SurgicalDepartmentCode: "", //Required
            IsTraining: "",
            LocationCode: "",                 //Required
            OperationTypeCode: "",         //Required
            Priority: "", //Required
            FellowName: "",                     //Required
            FellowCode: "",                     //Required
            FellowAge: "",                       //Required
            FellowWeight: "",
            IsFellowDiabetic: "",
            IsHBP: "",                         //Required
            IsXrayDeviceRequired: "",
            FellowGender: "",                //Required
            SurgeonCode: "",                   //Required
            SurgicalNursCode: "",
            AnesthesiologistCode: "", //Required
            CleanTeamCode: "",
            state: this.props.state,
            Time: this.getBeginTime(),
            operationStore: operationStore,
            HostingDepartmentId:"",
            SurgicalDepartmentId: "",
            NursingUnitCode: "",
            NursingUnitId: ""
        }
        this.editSurgeryDetailsContent = this.editSurgeryDetailsContent.bind(this);
        this.setContentEditSurgery = this.setContentEditSurgery.bind(this);
        this.setFellowDetails = this.setFellowDetails.bind(this)
        this.setServiceProvidersDetails = this.setServiceProvidersDetails.bind(this);
        this.getBeginTime = this.getBeginTime.bind(this);
        this.editBeginTime = this.editBeginTime.bind(this);
        this.setSurgeryDetails = this.setSurgeryDetails.bind(this);
        this.setDataSurgeryDetails = this.setDataSurgeryDetails.bind(this);
    }

    setContentEditSurgery = (e) => {

        this.setState({ editMode: e })
    }
    setFellowDetails(name, id, age, weight, gender, bloodPressure, diabetes, Xray) {
        this.setState({
            FellowName: name,                     //Required
            FellowCode: id,                     //Required
            FellowAge: age,                       //Required
            FellowWeight: weight,
            FellowGender: gender,
            IsHBP: bloodPressure,
            IsFellowDiabetic: diabetes,
            IsXrayDeviceRequired: Xray
        })
    }
    setSurgeryDetails(hostingDepartmentCode, surgicalDepartmentCode, hostingDepartmentId, surgicalDepartmentId, nursingUnitCode, nursingUnitId,isTraining, locationCode, operationTypeCode, priority) {
        debugger;
        this.setState({
            HostingDepartmentCode: hostingDepartmentCode, //Required
            SurgicalDepartmentCode: surgicalDepartmentCode, //Required
            HostingDepartmentId:hostingDepartmentId,
            SurgicalDepartmentId:surgicalDepartmentId,
            NursingUnitCode:nursingUnitCode,
            NursingUnitId: nursingUnitId,
            IsTraining: isTraining,
            LocationCode: locationCode,                 //Required
            OperationTypeCode: operationTypeCode,         //Required
            Priority: priority, //Required,
            
        })
    }
    setServiceProvidersDetails(surgeonCode, surgicalNursCode, anesthesiologistCode, cleanTeamCode,surgeonId, surgicalNursId, anesthesiologistId) {
        this.setState({

            SurgeonCode: surgeonCode,
            SurgicalNursCode: surgicalNursCode,
            AnesthesiologistCode: anesthesiologistCode,
            CleanTeamCode: cleanTeamCode,

        })
        this.setDataSurgeryDetails(surgeonCode, surgicalNursCode, anesthesiologistCode,surgeonId, surgicalNursId, anesthesiologistId)
    }

    getBeginTime() {
        switch (this.props.state) {
            case "O":
                {

                    return getTime(this.props.appointment.operation.surgeryOrigBeginTime)
                }
                break;
            case "S":
                {
                    return getTime(this.props.appointment.operation.surgeryBeginTime)
                }
                break;
            case "A":
                {
                    return getTime(this.props.appointment.operation.anesthesiaBeginTime)
                }
                break;
            default:

        }
    }
    editBeginTime(e) {

        switch (this.state.state) {///יש לשנות

            case "O":
                {
                    this.setState({ Time: e.target.value, BeginTime: e.target.value })
                }
                break;
            case "S":
                {
                    this.setState({ Time: e.target.value, BeginTime: e.target.value })
                }
                break;
            case "A":
                {
                    this.setState({ Time: e.target.value, BeginTime: e.target.value })
                }
                break;
            default:

        }
    }
    setDataSurgeryDetails(SurgeonCode, SurgicalNursCode, AnesthesiologistCode,surgeonId, surgicalNursId, anesthesiologistId ) {
        this.props.onCloseModal();
        this.props.onCloseBigModal();
        var valueInput = this.state.BeginTime;
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
            HostingDepartmentCode: this.state.HostingDepartmentCode, //Required
            SurgicalDepartmentCode: this.state.SurgicalDepartmentCode, //Required
            HostingDepartmentId:this.state.HostingDepartmentId,
            SurgicalDepartmentId:this.state.SurgicalDepartmentId,
            NursingUnitDepartmentCode:this.state.NursingUnitCode,
            NursingUnitDepartmentId: this.state.NursingUnitId,
            IsTraining: this.state.IsTraining,
            LocationCode: this.state.LocationCode,                 //Required
            OperationTypeCode: this.state.OperationTypeCode,         //Required
            Priority: this.state.Priority !== "" ? this.state.Priority : 0, //Required
            FellowName: this.state.FellowName,                     //Required
            FellowCode: this.state.FellowCode,                     //Required
            FellowAge: this.state.FellowAge,                       //Required
            FellowWeight: this.state.FellowWeight !== "" ? this.state.FellowWeight : 0,
            // FellowHeight: this.state.FellowHeight !== "" ? this.state.FellowHeight : 0,
            IsFellowDiabetic: this.state.IsFellowDiabetic,
            IsHBP: this.state.IsHBP,                         //Required
            IsXrayDeviceRequired: this.state.IsXrayDeviceRequired,
            FellowGender: this.state.FellowGender,                //Required
            SurgeonCode: SurgeonCode,                   //Required
            SurgicalNursCode: SurgicalNursCode !== "" ? SurgicalNursCode : null,
            AnesthesiologistCode: AnesthesiologistCode, //Required
            SurgeonId: surgeonId,                   //Required
            SurgicalNursId: surgicalNursId !== "" ? surgicalNursId : null,
            AnesthesiologistId: anesthesiologistId, //Required
            CleanTeamCode: this.state.CleanTeamCode !== "" ? this.state.CleanTeamCode : 0
        };
        debugger
        this.state.operationStore.editOperation(data,this.props.operationQueue,this.props.equipments,this.props.appointment,this.props.state,
             this.props.loading,this.props.AddSurgery,this.props.rescheduleSetState )   
    }

    editSurgeryDetailsContent() {
        switch (this.state.editMode) {
            case "SurgeryDetails":
                {
                    return <SurgeryDetails
                        setContentEditSurgery={this.setContentEditSurgery}
                        appointment={this.props.appointment}
                        serviceProviders={this.props.serviceProviders}
                        serviceTypes={this.props.serviceTypes.serviceTypesList}
                        fellows={this.props.fellows}
                        rooms={this.props.rooms}
                        departments={this.props.departments}
                        setSurgeryDetails={this.setSurgeryDetails}
                    >
                    </SurgeryDetails>
                }
            case "EditFellowDetails":
                {
                    return <EditFellowDetails
                        appointment={this.props.appointment}
                        setContentEditSurgery={this.setContentEditSurgery}
                        fellows={this.props.fellows}
                        setFellowDetails={this.setFellowDetails}
                        setContentEditSurgery1={this.setContentEditSurgery1}
                    >

                    </EditFellowDetails>
                }
            case "ServiceProvidersDetails":
                {
                    return <ServiceProvidersDetails
                        appointment={this.props.appointment}
                        setContentEditSurgery={this.setContentEditSurgery}
                        serviceProviders={this.props.serviceProviders}
                        serviceTypes={this.props.serviceTypes}
                        fellows={this.props.fellows}
                        rooms={this.props.rooms}
                        departments={this.props.departments}
                        set={this.setFellowDetails}
                        setServiceProvidersDetails={this.setServiceProvidersDetails}
                        setDataSurgeryDetails={this.setDataSurgeryDetails}
                    >
                    </ServiceProvidersDetails>
                }
        }
    }

    render() {
        return (
            <div>
                <div className="modal-header p-1 m-0 row-center">

                    <input
                        type="time"
                        id="Time"
                        min={this.props.operationQueue.serviceQueue.beginTime.split("T")[1]}
                        max={this.props.operationQueue.serviceQueue.endTime.split("T")[1]}
                        value={this.state.Time}
                        className="timeInput font-pr basicColorBlue"
                        onChange={this.editBeginTime}
                    />
                </div>
                <br></br>
                <div>
                    {this.state.editMode ? this.editSurgeryDetailsContent() : ""}
                </div>
            </div>
        );

    }
}

export default EditSurgeryDetails;





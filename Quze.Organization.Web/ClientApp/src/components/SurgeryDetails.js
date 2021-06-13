import React, { Component } from 'react';
import { forEachChild } from 'typescript';

export class SurgeryDetails extends Component {
    constructor(props) {
        super(props)
        this.state = {
            nextPage: "EditFellowDetails",
            IsTraining: false,
            HospitalizationDepatment: this.props.appointment.operation.hostingDepartment.description, //Required
            SurgicalDepartment: this.props.appointment.operation.surgicalDepartment.description, //Required
            LocationCode: "Room" + this.props.appointment.operation.roomId,       // Required
            OperationType: this.props.appointment.serviceType.description,      //Required
            Priority: this.props.appointment.operation.priority === 1 ? "Regular" :
                this.props.appointment.operation.priority === 2 ? "Emergency" : "Lifesaver",
            HospitalizationDepatmentCode: this.props.appointment.operation.hostingDepartment.id,
            SurgicalDepartmentCode: this.props.appointment.operation.surgicalDepartment.id,
            HospitalizationDepatmentId: this.props.appointment.operation.hostingDepartment.code,
            SurgicalDepartmentId: this.props.appointment.operation.surgicalDepartment.code,
            OperationTypeCode: this.props.appointment.serviceType.code,
            roomId: this.props.appointment.operation.roomId,
            nurseingUnit: this.props.appointment.operation.nursingUnit?this.props.appointment.operation.nursingUnit.description:"",
            NursingUnitCode: this.props.appointment.operation.nursingUnit?this.props.appointment.operation.nursingUnit.id:null,
            NursingUnitId: this.props.appointment.operation.nursingUnit?this.props.appointment.operation.nursingUnit.code:null,
            filterList: false
            //listTypsNursingUnit:this.typsDepartment(),
            //listTypsMedicalUnit:this.typsDepartment(),
            //listTypsDepartmentOfSurgery:this.typsDepartment()

            //Required
        };
        this.saveSurgeryDetails = this.saveSurgeryDetails.bind(this);
        this.setSurgeryDetails = this.setSurgeryDetails.bind(this);
        //this.typsDepartment = this.typsDepartment.bind(this);
        this.findElementId = this.findElementId.bind(this);
    }
    saveSurgeryDetails() {
        this.props.setSurgeryDetails(this.state.HospitalizationDepatmentCode, this.state.SurgicalDepartmentCode, this.state.HospitalizationDepatmentId, this.state.SurgicalDepartmentId,this.state.NursingUnitCode,this.state.NursingUnitId, this.state.IsTraining, this.state.roomId, this.state.OperationTypeCode, this.state.Priority === "Regular" ? 3 : this.state.Priority === "Emergency" ? 2 : 1)
        this.props.setContentEditSurgery(this.state.nextPage)
    }

    findElementId(listElement, element) {
        const result = listElement.find((e) => element === e.description);
        return result;
    }

    //typsDepartment() {
    //    var listDepartment =this.props.departments
    //    var listTypsDepartmentOfSurgery = listDepartment.find((e) => e.departmentsTypesId === 6)
    //    return listTypsDepartmentOfSurgery;
    //    var listTypsMedicalUnit = listDepartment.find((e) => e.departmentsTypesId === 7)
    //    return listTypsMedicalUnit;
    //    var listTypsNursingUnit = listDepartment.find((e) => e.departmentsTypesId === 8)
    //    this.setState({ listTypsNursingUnit: listTypsNursingUnit}) 
    //    return listTypsNursingUnit;
    //}

    setSurgeryDetails(e) {
        this.setState({ filterList: false });

        switch (e.target.id) {
            case "HospitalizationDepatment":
                {
                    this.setState({ HospitalizationDepatment: e.target.value });
                    if (e.target.value != "") {//null
                        var deparment = this.findElementId(this.props.departments, e.target.value);
                        this.setState({ filterList: true });
                        if (deparment) {
                            this.props.appointment.operation.hostingDepartment = deparment;
                            this.setState({ HospitalizationDepatmentCode: deparment.id, HospitalizationDepatmentId: deparment.code });
                        }
                    }
                    break;
                }
            case "SurgicalDepartment":
                {
                    this.setState({ SurgicalDepartment: e.target.value });
                    if (e.target.value != "") {//null
                        var deparment = this.findElementId(this.props.departments, e.target.value);
                        this.setState({ filterList: true });
                        if (deparment) {
                            this.props.appointment.operation.hostingDepartment = deparment;
                            this.setState({ SurgicalDepartmentCode: deparment.id, SurgicalDepartmentId: deparment.code });
                        }
                    }
                    break;
                }
            case "NursingUnit":
                {

                    this.setState({ nurseingUnit: e.target.value });

                    if (e.target.value != "") {//null
                        this.setState({ filterList: true });
                        var nurseingUnit = this.findElementId(this.props.departments, e.target.value);
                        if (nurseingUnit) {
                            this.props.appointment.operation.nurseingUnit = nurseingUnit;
                            this.setState({ NursingUnitCode: nurseingUnit.id, NursingUnitId: nurseingUnit.code });
                        }
                    }
                    break;
                }


            case "TrainingFalse":
                {
                    this.setState({ IsTraining: false })
                    // this.props.appointment.operation.served = e.target.value;
                    break;
                }
            case "TrainingTrue":
                {
                    this.setState({ IsTraining: true })
                    // this.props.appointment.operation.served = e.target.value;
                    break;
                }
            case "RoomNumber":
                {
                    if (e.target.value != "") {
                        var roomId = parseInt(e.target.value.split(" ")[1]);
                        this.setState({ roomId: roomId });
                        // this.props.appointment.operation.roomId=
                        //this.setState({ filterList: true });
                    }
                    this.setState({ LocationCode: e.target.value });
                    break;
                }
            case "ServiceType":
                {
                    this.setState({ OperationType: e.target.value });
                    if (e.target.value != "") {//null
                        var serviceType = this.findElementId(this.props.serviceTypes, e.target.value);
                        this.setState({ filterList: true });
                        if (serviceType) {
                            this.props.appointment.serviceType = serviceType;
                            this.setState({ OperationTypeCode: serviceType.code });
                        }
                    }
                    break;
                }
            case "Priority":
                {
                    var priority = e.target.value === "Lifesaver" ? 1 :
                        e.target.value === "Emergency" ? 2 : 3
                    this.setState({ Priority: e.target.value });
                    this.props.appointment.operation.priority = priority;
                    break;
                }
        }
    }
    render() {
        var appointment = this.props.appointment

        return (

            <div className="container">
                <div className="row row-centered">
                    <div className="col-lg-12">
                        <div className="form-group">
                            <div className='row justify-content-center'>

                                <input id="HospitalizationDepatment" type="search" className="form-control txtAL"
                                    placeholder={appointment && appointment.hostingDepartment ? appointment.operation.hostingDepartment.description :
                                        "Hospitalization Depatment"}
                                    list={this.state.filterList ? "HostingDep" : ""}
                                    onChange={this.setSurgeryDetails}
                                    value={this.state.HospitalizationDepatment}
                                    title="Hospitalization Depatment"
                                />

                            </div>
                            <datalist id="HostingDep">
                                {

                                    this.props.departments ?
                                        this.props.departments.map((item, i) => {
                                            if (item.departmentsTypesId === 7)
                                                return <option key={i}>{item.description}</option>
                                        })
                                        :
                                        ""
                                }
                            </datalist>

                            <br></br>
                            <div className='row justify-content-center'>
                                <input id="SurgicalDepartment" type="search"
                                    className="form-control txtAL"
                                    placeholder={appointment && appointment.surgicalDepartment ? appointment.surgicalDepartment.description :
                                        "Surgical Department"} list={this.state.filterList ? "SurgicalDep" : ""}
                                    onChange={this.setSurgeryDetails}
                                    value={this.state.SurgicalDepartment}
                                    title="Surgical Department" />

                            </div>
                            <datalist id="SurgicalDep">
                                {

                                    this.props.departments ?
                                        this.props.departments.map((item, i) => {
                                            if (item.departmentsTypesId === 6)
                                                return <option key={i}>{item.description}</option>
                                        })
                                        :
                                        ""
                                }
                            </datalist>
                            <br></br>
                            <div className='row justify-content-center'>
                                <input id="NursingUnit" type="search"
                                    className="form-control txtAL"
                                    placeholder={appointment &&  appointment.operation.nursingUnit ?  appointment.operation.nursingUnit.description : "Nursing Unit"}
                                    onChange={this.setSurgeryDetails}
                                    value={this.state.nurseingUnit}
                                    list={this.state.filterList ? "NursingUnitList" : ""}
                                    title="Nursing Unit"
                                />

                            </div>
                            <datalist id="NursingUnitList">
                                {

                                    this.props.departments ?
                                        this.props.departments.map((item, i) => {
                                            if (item.departmentsTypesId === 8)
                                                return <option key={i}>{item.description}</option>
                                        })
                                        :
                                        ""
                                }
                            </datalist>
                            <br></br>
                            <div className='row justify-content-center'>
                                Training:
                            </div>
                            <br></br>
                            <div className='row justify-content-center'>
                                <input id="TrainingTrue" className="col-1" type="radio" value={true} name="radioModal"
                                    checked={appointment.operation.served}
                                    onChange={this.setSurgeryDetails}
                                    value={this.state.IsTraining}
                                    title="Training True"
                                />
                                True
                <input id="TrainingFalse" className="col-1" type="radio" value={false} name="radioModal" checked={!appointment.operation.served}
                                    onChange={this.setSurgeryDetails}
                                    value={this.state.IsTraining}
                                    title="Training False"
                                />False
              </div>
                            <br></br>
                            <div className='row justify-content-center'>
                                <input id="RoomNumber" className="form-control txtAL" type="search"
                                    placeholder={appointment && appointment.operation.roomId ?
                                        "Room" + " " + appointment.operation.roomId : "Room Number"} list="room"
                                    onChange={this.setSurgeryDetails}
                                    value={this.state.LocationCode}
                                    title="Room Number"
                                />

                            </div>
                            <datalist id="rooms">
                                {
                                    this.state.rooms ?
                                        this.state.rooms.map((item, i) => {
                                            return <option key={i}>{item.description}</option>
                                        })
                                        :
                                        ""
                                }
                            </datalist>
                            <br></br>
                            <div className='row justify-content-center'>
                                <input id="ServiceType" className="form-control txtAL" type="search"
                                    placeholder={appointment && appointment.serviceType ? appointment.serviceType.description :
                                        "Service Type"} list="servicetypes"
                                    onChange={this.setSurgeryDetails}
                                    value={this.state.OperationType}
                                    title="Service Type"
                                />

                            </div>
                            <datalist id="servicetypes">
                                {
                                    this.props.serviceType ?

                                        this.props.serviceType.map((item, i) => {
                                            return <option key={i}>{item.description}</option>
                                        })
                                        :
                                        ""
                                }
                            </datalist>
                            <br></br>
                            <div className='row justify-content-center'>
                                <input id="Priority" className="form-control txtAL" type="search"
                                    placeholder={appointment && appointment.operation.priority === 1 ? "Lifesaver" :
                                        appointment.operation.priority === 2 ? "Emergency" : "Regular"} list="priority"
                                    onChange={this.setSurgeryDetails}
                                    value={this.state.Priority}
                                    title="Priority"
                                />
                            </div>
                            <datalist id="priority">
                                <option>{"Regular"}</option>
                                <option>{"Emergency"}</option>
                                <option>{"Lifesaver"}</option>
                            </datalist>

                            <br></br>
                            <div>
                                <br></br>
                                <div class="row modal-footer justify-content-end m-0 p-0 mt-2">
                                    <button className="btn btn-outline-primary font-pr m-2" name="butNext" value="EditFellowDetails" onClick={this.saveSurgeryDetails}
                                    > Next
                                </button>
                                </div>
                            </div >
                        </div >
                    </div >
                </div ></div >
        )
    }
}
export default SurgeryDetails;




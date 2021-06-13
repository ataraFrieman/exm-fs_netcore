import React, { Component } from 'react';
import { observer } from 'mobx-react';
import { withI18n } from 'react-i18next';
import { Row } from "react-bootstrap";
import BigCalendar from 'react-big-calendar'
import * as http from '../helpers/Http';
import '../css/RendeSurgery.css';

export class RenderSurgery extends Component {
    displayName = "navSurgery"
    constructor(props) {
        super(props);
        console.log("RenderSurgery props: ", props)
        this.state = {
            selectedOption: 'location',
            indexEvent: 0
        };
        this.createResourseDepartments = this.createResourseDepartments.bind(this);
    }
    createResourseDepartments() {
        this.props.departments.departmentsList.forEach((r) => {
            this.props.changeResource(r.id, r.description);
        });
    }
    getListByhandleOptionChange = () => {
        var operationData = this.props.operationQueue;
        if (!operationData.operationsList)
            return;
        switch (this.state.selectedOption) {
            case "location":
                {
                    if (operationData.operationsList.length !== 0) {
                        this.props.ResetList();
                        this.props.changeLocations(operationData.operationsList);
                        if (operationData.conflictList && operationData.conflictList.length !== 0 && this.props.state === "O") {
                            console.log(operationData);
                            for (var i = 0; i < operationData.conflictList.length; i++) {
                                {
                                    this.props.changeConflict(operationData.conflictList[i],
                                        operationData.conflictList[i].appointmentA.operation.roomId,
                                        operationData.conflictList[i].appointmentB.operation.roomId, i);
                                }
                            }
                        }
                    }
                }
                break;
            case "Anesthesiologists":
                {
                    this.props.ResetList();
                    i = 0;
                    for (; i < operationData.operationsList.length; i++) {
                        this.creaeteEventAnastesia(operationData.operationsList[i]);
                        this.props.changeResource(operationData.operationsList[i].operation.anesthesiologistId,
                            operationData.operationsList[i].operation.anesthesiologist.fullName);
                        if (operationData.conflictList && this.props.state === "O") {
                            for (var j = 0; j < operationData.conflictList.length; j++) {
                                if (operationData.conflictList[j].conflictType === "Anesthesia") {
                                    this.props.changeConflict(operationData.conflictList[j],
                                        operationData.conflictList[j].appointmentA.operation.anesthesiologistId,
                                        operationData.conflictList[j].appointmentB.operation.anesthesiologistId, i);
                                }
                            }
                        }
                    }
                }
                break;
            case "Surgeons":
                {
                    this.props.ResetList();
                    i = 0;
                    for (; i < operationData.operationsList.length; i++) {
                        this.creaeteEventSurgion(operationData.operationsList[i])
                        this.props.changeResource(operationData.operationsList[i].operation.surgeonId,
                            operationData.operationsList[i].operation.surgeon.fullName);
                        if (operationData.conflictList && this.props.state === "O")
                            for (j = 0; j < operationData.conflictList.length; j++) {
                                if (operationData.conflictList[j].conflictType === "Surgery") {
                                    this.props.changeConflict(operationData.conflictList[j],
                                        operationData.conflictList[j].appointmentA.operation.surgeonId,
                                        operationData.conflictList[j].appointmentB.operation.surgeonId, i);
                                }
                            }
                    }
                }
                break;
            // case "Cleaners":
            //     {
            //         this.props.ResetList();
            //         i = 0;
            //         for (; i < operationData.operationsList.length; i++) {
            //             this.creaeteEventCleaners(operationData.operationsList[i]);
            //             this.props.changeResource(operationData.operationsList[i].operation.cleanTeamId,
            //                 "clean" + i.toString());
            //             if (operationData.conflictList && this.props.state === "O")
            //                 for (j = 0; j < operationData.conflictList.length; j++) {
            //                     if (operationData.conflictList[j].conflictType === "Clean") {
            //                         this.props.changeConflict(operationData.conflictList[j],
            //                             operationData.conflictList[j].appointmentA.operation.cleanTeamId,
            //                             operationData.conflictList[j].appointmentB.operation.cleanTeamId, i);
            //                     }
            //                 }
            //         }
            //     }
            //     break;
                case "Departments":
                {
                    this.props.ResetList();
                    this.createResourseDepartments();
                     i = 0;
                    for (; i < operationData.operationsList.length; i++) {
                        this.creaeteEventSurgionByDepartments(operationData.operationsList[i])
                        if (operationData.conflictList && this.props.state === "O")
                            for ( j = 0; j < operationData.conflictList.length; j++) {
                                if (operationData.conflictList[j].conflictType === "Surgery") {
                                    this.props.changeConflict(operationData.conflictList[j],
                                        operationData.conflictList[j].appointmentA.operation.surgeonId,
                                        operationData.conflictList[j].appointmentB.operation.surgeonId, i);
                                }
                            }
                    }
                }
                break;
            default:
                {
                    this.props.ResetList();
                }
        }
        return this.props.getCalander()
    }

    handleOptionChange = (changeEvent) => {
        this.setState({
            selectedOption: changeEvent.target.value
        }, () => { console.log('new state', this.state.selectedOption); })
        this.getListByhandleOptionChange();
    }


    sameConflict = (event) => {
        if (this.props.operationQueue && this.props.state) {
            var operationData = this.props.operationQueue;
            if (operationData.conflictList)
                this.props.addConflictId(event.target.value);
        }
    }
//by service provider
    creaeteEventSurgion = (operationSurgery) => {
        switch (this.props.state) {
            case "O":
                {
                    this.props.changeEvent(operationSurgery.operation.surgeryOrigBeginTime,
                        operationSurgery.operation.surgeryOrigEndTime
                        , operationSurgery.operationId,
                        'surgon ' + operationSurgery.fellow.firstName,
                        operationSurgery.operation.surgeonId);
                }
                break;
            case "S":
                {

                    this.props.changeEvent(operationSurgery.operation.surgeryBeginTime,
                        operationSurgery.operation.surgeryEndTime
                        , operationSurgery.operationId,
                        'surgon ' + operationSurgery.fellow.firstName,
                        operationSurgery.operation.surgeonId);
                }
                break;
            case "A":
                {
                    this.props.changeEvent(operationSurgery.operation.surgeryActualBeginTime,
                        operationSurgery.operation.surgeryActualEndTime
                        , operationSurgery.operationId,
                        'surgon ' + operationSurgery.fellow.firstName,
                        operationSurgery.operation.surgeonId);
                }
                break;

        }

    }

//by service departments
    creaeteEventSurgionByDepartments = (operationSurgery) => {
        switch (this.props.state) {
            case "O":
                {
                    this.props.changeEvent(operationSurgery.operation.surgeryOrigBeginTime,
                        operationSurgery.operation.surgeryOrigEndTime
                        , operationSurgery.operationId,
                        'surgon ' + operationSurgery.operation.surgeon.fullName,
                        operationSurgery.operation.surgicalDepartment.id);
                }
                break;
            case "S":
                {

                    this.props.changeEvent(operationSurgery.operation.surgeryBeginTime,
                        operationSurgery.operation.surgeryEndTime
                        , operationSurgery.operationId,
                        'surgon ' + operationSurgery.operation.surgeon.fullName,
                        operationSurgery.operation.surgicalDepartment.id);
                }
                break;
            case "A":
                {
                    this.props.changeEvent(operationSurgery.operation.surgeryActualBeginTime,
                        operationSurgery.operation.surgeryActualEndTime
                        , operationSurgery.operationId,
                        'surgon ' + operationSurgery.operation.surgeon.fullName,
                        operationSurgery.operation.surgicalDepartment.id);
                }
                break;

        }

    }
   
    creaeteEventAnastesia = (operationSurgery) => {
        switch (this.props.state) {
            case "O":
                {
                    this.props.changeEvent(operationSurgery.operation.anesthesiaOrigBeginTime,
                        operationSurgery.operation.anesthesiaOrigEndTime
                        , operationSurgery.operationId,
                        operationSurgery.fellow.firstName,
                        operationSurgery.operation.anesthesiologistId);
                }
                break;
            case "S":
                {

                    this.props.changeEvent(operationSurgery.operation.anesthesiaBeginTime,
                        operationSurgery.operation.anesthesiaEndTime
                        , operationSurgery.operationId,
                        operationSurgery.fellow.firstName,
                        operationSurgery.operation.anesthesiologistId);
                }
                break;
            case "A":
                {
                    this.props.changeEvent(operationSurgery.operation.anesthesiaActualBeginTime,
                        operationSurgery.operation.anesthesiaActualEndTime
                        , operationSurgery.operationId,
                        operationSurgery.fellow.firstName,
                        operationSurgery.operation.anesthesiologistId);
                }
                break;

        }
    }

    creaeteEventCleaners = (operationSurgery) => {
        switch (this.props.state) {
            case "O":
                {
                    this.props.changeEvent(operationSurgery.operation.cleanOrigBeginTime,
                        operationSurgery.operation.cleanOrigEndTime
                        , operationSurgery.operationId,
                        'clean ' + operationSurgery.fellow.firstName,
                        operationSurgery.operation.cleanTeamId);
                }
                break;
            case "S":
                {

                    this.props.changeEvent(operationSurgery.operation.cleanBeginTime,
                        operationSurgery.operation.cleanEndTime
                        , operationSurgery.operationId,
                        'clean ' + operationSurgery.fellow.firstName,
                        operationSurgery.operation.cleanTeamId);
                }
                break;
            case "A":
                {

                    this.props.changeEvent(operationSurgery.operation.cleanActualBeginTime,
                        operationSurgery.operation.cleanActualEndTime
                        , operationSurgery.operationId,
                        'clean ' + operationSurgery.fellow.firstName,
                        operationSurgery.operation.cleanTeamId);
                }
                break;

        }

    }
    isActive(state) {
        return this.props.state === state ? "active" : "";
    }
    isDesable(state) {
        return this.props.reschedulingState ? "" : "disabled";
    }

    render() {
        var t = this.props.t;
        console.log(this.props.operationQueue);
        return (
            <div>
                {this.props.operationQueue ?
                    <Row className="  px-0 ml-1">
                        <div className="col-12 m-0 px-0 border blue" >
                            <Row className="m-0 p-0 align-items-center">
                                <div className=" col-6 m-0 p-0 ">
                                    <div className="row m-0 p-0">
                                        <span className="col-2 p-0 text-align mt-2">{t("surgeons.orderBy")}</span>
                                        <input type="radio" name="location" value="location" className="p-0" style={{ marginTop: "12px" }} checked={this.state.selectedOption === 'location'}
                                            onChange={this.handleOptionChange}></input>
                                        <span className="font-pr px-2 mt-2">{t("surgeons.loc")}</span>
                                        <input type="radio" name="Anesthesiologists" value="Anesthesiologists" style={{ marginTop: "12px" }}
                                            className=" p-0" checked={this.state.selectedOption === 'Anesthesiologists'}
                                            onChange={this.handleOptionChange}></input>
                                        <span className="font-pr px-2 mt-2">{t("surgeons.anesthetic")}</span>


                                        <input type="radio" name="Surgeons" value="Surgeons" style={{ marginTop: "12px" }} checked={this.state.selectedOption === 'Surgeons'} className="p-0"
                                            onChange={this.handleOptionChange}></input>
                                        <span className="font-pr px-2 mt-2">{t("surgeons.Surgeons")}</span>

                                        <input type="radio" name="Departments" value="Departments" style={{ marginTop: "12px" }} checked={this.state.selectedOption === 'Departments'} className="p-0"
                                            onChange={this.handleOptionChange}></input>
                                        <span className="font-pr px-2 mt-2">{t("surgeons.Departments")}</span>

                                    </div>
                                </div>
                                <div className="col-4"></div>
                                 {this.props.reschedulingState?
                                <div className="btn-group btn-group-toggle font-pr" data-toggle="buttons">
                                    <label className="basicColorBlue" onClick={this.props.OriginalReschedule}>
                                        <i className="pe-7s-back basicColorRed"></i>
                                         Revert
                                      </label>
                                    {/* {this.props.reschedulingState ?
                                    {"btn btn-secondary " + this.isActive("O")}
                                        <label className={"btn btn-secondary " + this.isActive("S")} onClick={this.props.NewRescheduling}>
                                            <input type="radio" name="options" id="option2" autoComplete="off" checked /> Scheduling
                                      </label> : <span />}
                                    {this.props.actualState ?
                                        <label className={"btn btn-secondary " + this.isActive("A")} onClick={this.props.Actual}>
                                            <input type="radio" name="options" id="option3" autoComplete="off" checked /> Actual
                                      </label> : <span />} */}
                                </div>
                                : <span />}

                                {
                                    //this.props.operationQueue.conflictList && this.props.operationQueue.conflictList.length != 0 && this.props.state == "O" ?
                                    //<div className="col-2">
                                    //    <select className="custom-select custom-select-sm font-pr mb-3" id="selectbox" onChange={this.sameConflict}>
                                    //        {

                                    //            this.props.operationQueue.conflictList && this.props.state == "O" ? this.props.operationQueue.conflictList.map((item, index) => {
                                    //                return <option key={index} value={index} onClick={(evt) => { console.log(evt); }}>{"Conflict " + index}</option>;
                                    //            }) : ""}
                                    //    </select>
                                    //    </div> : ""
                                }
                            </Row>
                        </div>
                    </Row> : ""}
                <Row className="m-0 px-2 col-12">
                    <div className="col-12 m-0 px-0"> {this.getListByhandleOptionChange()}</div>
                </Row>
            </div>
        );
    }
}
export default withI18n()(RenderSurgery);

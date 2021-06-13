import React, { Component } from 'react';
import { withI18n } from 'react-i18next';
import { Row } from "react-bootstrap";
import '../css/RendeSurgery.css';

export class RenderSurgeryByDepartments extends Component {
    displayName = "RenderSurgeryByDepartments"
    constructor(props) {
        super(props);
        this.state = {
            selectedOption: 'Surgeons',
            indexEvent: 0,
            resourceList: []
        };
        this.createResourseDepartments = this.createResourseDepartments.bind(this);
    }
    createResourseDepartments() {
        this.props.departments.departmentsList.forEach((r) => {
            this.props.changeResource(r.id, r.description)
            this.state.resourceList.push(
                {
                    resourceId: r.id,
                    resourceTitle:r.description
                }
            )
        });
    }

    
    getListByhandleOptionChange = () => {
        var operationData = this.props.operationQueue;
        if (!operationData.operationsList)
            return;
        switch (this.state.selectedOption) {
            
            case "Surgeons":
                {
                    this.props.ResetList();
                    this.createResourseDepartments();
                    var i = 0;
                    for (; i < operationData.operationsList.length; i++) {
                        this.creaeteEventSurgion(operationData.operationsList[i])
                        if (operationData.conflictList && this.props.state === "O")
                            for (var j = 0; j < operationData.conflictList.length; j++) {
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


    

    creaeteEventSurgion = (operationSurgery) => {
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
                                {/* {this.props.reschedulingState || this.props.actualState ? */}
                                <div className="btn-group btn-group-toggle font-pr " data-toggle="buttons">
                                    <label className={"btn btn-secondary " + this.isActive("O")} onClick={this.props.OriginalReschedule}>
                                        <input type="radio" name="options" id="option1" autoComplete="off" checked /> Origion
                                      </label>
                                    {this.props.reschedulingState ?
                                        <label className={"btn btn-secondary " + this.isActive("S")} onClick={this.props.NewRescheduling}>
                                            <input type="radio" name="options" id="option2" autoComplete="off" checked /> Scheduling
                                      </label> : <span />}
                                    {this.props.actualState ?
                                        <label className={"btn btn-secondary " + this.isActive("A")} onClick={this.props.Actual}>
                                            <input type="radio" name="options" id="option3" autoComplete="off" checked /> Actual
                                      </label> : <span />}
                                </div>
                                {/* : <span />} */}

                                
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
export default withI18n()(RenderSurgeryByDepartments);

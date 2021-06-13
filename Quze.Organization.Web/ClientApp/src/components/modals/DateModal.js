import React, { Component } from 'react'
import InputField from './Input'
import { getTime } from '../../helpers/TimeService';
import $ from 'jquery';

export class DateModal extends Component {
    constructor(props) {
        super(props)

        this.state = {
            isPickedSq: false,
            date: new Date().toISOString().substring(0, 10),
            startTime: "", //getTime(new Date())
            endTime: "" //getTime(new Date())
        }

        this.saveServiceQueue = this.saveServiceQueue.bind(this);
        this.handelChange = this.handelChange.bind(this);
        this.onCancel = this.onCancel.bind(this);
    }

    handelChange(e) {
        this.setState({ [e.target.id]: e.target.value })
    }

    saveServiceQueue() {
        if (this.state.date == "" || this.state.startTime == "" || this.state.endTime == "") {
            alert("Missing details!")
        }
        else if (this.state.startTime == this.state.endTime) {
            alert("Change the start time or the end time, please")
        }
        else {
            let date = this.state.date
            let startTime = this.state.startTime
            let endTime = this.state.endTime
            let isPickedSq = true

            this.setState({ isPickedSq: true, startTime: "", endTime: "", date: new Date().toISOString().substring(0, 10) });

            this.props.createNewShift(date, startTime, endTime, isPickedSq)

        }
    }

    onCancel() {
        this.setState({ startTime: "", endTime: "", date: new Date().toISOString().substring(0, 10) });
    }

    render() {
        var t = this.props.t;
        return (
            <div>
                {/* { this.state.isPickedSq == false ?  */}
                <div className="modal d-blocke" id="serviceQueueModal" tabIndex="-1" role="dialog" data-backdrop="false" aria-hidden="true" /* onClose={this.onCloseSqModal} */ >
                    <div className="modal-dialog direction" role="document">
                        <div className="modal-content modal ">

                            <div className="modal-header border-1 py-0">
                                {/* <h4 className="modal-title">Add shift</h4> */}
                                <button type="button" className="close" data-dismiss="modal" aria-label="Cancel" onClick={this.onCancel} >&times;</button>
                            </div>

                            <div className="modal-body" style={{ textAlign: "start" }}>
                                <div className="row p-1">
                                    <InputField
                                        // label={t("surgeryModal.fellowCode")}
                                        label={"Day: "}
                                        span="*"
                                        style={{ color: "red", margin: "5px" }}
                                        type="date"
                                        id="date"
                                        // .valueAsDate  = new Date()
                                        value={this.state.date}
                                        // value={""}
                                        className="timeInput"
                                        autoComplete="off"
                                        onChange={this.handelChange}
                                    />
                                </div>


                                <div className="row p-1">
                                    <InputField
                                        // label={t("surgeryModal.fellowCode")} 
                                        label={"From: "}
                                        span="*"
                                        style={{ color: "red", margin: "5px" }}
                                        type="time"
                                        id="startTime"
                                        // .valueAsDate  = new Date()
                                        value={this.state.startTime}
                                        // value={""}
                                        className="timeInput"
                                        autoComplete="off"
                                        onChange={this.handelChange}
                                    />

                                    <InputField
                                        // label={t("surgeryModal.fellowCode")}
                                        label={"Until: "}
                                        span="*"
                                        style={{ color: "red", margin: "5px" }}
                                        type="time"
                                        id="endTime"
                                        // .valueAsDate  = new Date()
                                        value={this.state.endTime}
                                        // value={""}
                                        className="timeInput"
                                        autoComplete="off"
                                        onChange={this.handelChange}
                                    />
                                </div>
                            </div>
                            <div className="row modal-footer justify-content-end m-0 p-0 mt-2">
                                <button className="btn btn-outline-primary font-pr m-2"
                                    onClick={this.saveServiceQueue} >{t("model.save")}</button>
                            </div>
                        </div>
                    </div>
                </div>
                {/* :
                    ""
                } */}
            </div>
        )
    }
}

export default DateModal

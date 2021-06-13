import React, { Component } from 'react'
import { withI18n } from 'react-i18next';
import serviceQueues from '../../stores/serviceQueues'
import { SelectDateOfServuceQ } from './SelectDateOfServuceQ.js'
import { AddOperationQueue } from './AddOperationQueue.js'
import '../../css/NavCalander.css'
import { textChangeRangeIsUnchanged } from 'typescript';

export class NavCalander extends Component {

    constructor(props) {
        var serviceQueuesST = serviceQueues.fromJS();
        super(props);
        this.state = {
            shiftDateTime: "",
            pickedSq: false,
            serviceQueuesStore: serviceQueuesST
        };
        this.setShiftDateTime = this.setShiftDateTime.bind(this);
    }

    componentWillMount() {

    }

    //set start of shift
    setShiftDateTime(time, pickedSq) {
        this.setState({ shiftDateTime: time, pickedSq: pickedSq });
    }

    render() {
        var t = this.props.t;
        return (
            <div className="col mt-2">
                <div className="m-0 row">
                    <span className="basicColorBlue p-1 mt-2 col-2">{this.props.current_datetime} </span>
                    <div className="col-2"></div>
                    {this.props.operationQueue && this.props.state !== "" && this.props.operationQueue.operationsList.length > 0 ?
                        <button className="font-pr col-7  text-right rescheduling" onClick={this.props.reschedual} title={t("surgeons.Reinlay")}>
                            {t("surgeons.Reinlay")}
                            <i className="pe-7s-right-arrow fs-20 font-weight-bold basicColorRed"></i>
                        </button> : <span />}
                </div>
                    <div className="row m-2">
                        <SelectDateOfServuceQ
                            startDateForDatePicker={this.props.startDateForDatePicker}
                            getServiceQByDate={this.props.getServiceQByDate}
                            dateToDatePicker={this.props.dateToDatePicker}
                            serviceQListByDate={this.props.serviceQListByDate}
                            serviceQueues={this.props.serviceQueues}
                            getServiceQById={this.props.getServiceQById}
                            t={t}
                        />
                    </div>

                    <div className="row m-2 mt-4">
                        <AddOperationQueue
                            operationQueue={this.props.operationQueue}
                            state={this.props.state}
                            Upload={this.props.Upload}
                            UploadActual={this.props.UploadActual}
                            load={this.props.load}
                            t={t}
                            serviceProviders={this.props.serviceProviders}
                            serviceTypes={this.props.serviceTypes}
                            fellows={this.props.fellows}
                            rooms={this.props.rooms}
                            departments={this.props.departments}
                            AddSurgery={this.props.AddSurgery}
                            shiftDateTime={this.state.shiftDateTime}
                            setShiftDateTime={this.setShiftDateTime}
                            getServiceQById={this.props.getServiceQById}
                            serviceQueues={this.props.serviceQueues}
                            dateToDatePicker={this.props.dateToDatePicker}
                            serviceQueuesStore={this.state.serviceQueuesStore}
                            state={this.props.state}
                        ></AddOperationQueue>
                    </div>

            </div>
        )
    }
}

export default withI18n()(NavCalander);
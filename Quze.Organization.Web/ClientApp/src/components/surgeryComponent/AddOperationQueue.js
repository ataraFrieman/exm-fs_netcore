import React, { Component } from 'react'
import { withI18n } from 'react-i18next';
import '../../css/NavCalander.css'
import SurgeryModal from '../modals/SurgeryModal';
import UploadSurgeryModal from '../modals/UploudSurgeryModal';
import { CreateServiceQ } from './CreateServiceQ.js'

export class AddOperationQueue extends Component {

    constructor(props) {

        super(props);
        this.state = {

        };
    }

    render() {
        var t = this.props.t;
        return (
            <div>
                <CreateServiceQ className="row inputStyle"
                    operationQueue={this.props.operationQueue}
                    setShiftDateTime={this.props.setShiftDateTime}
                    getServiceQById={this.props.getServiceQById}
                    serviceQueues={this.props.serviceQueues}
                    dateToDatePicker={this.props.dateToDatePicker}
                    serviceQueuesStore={this.props.serviceQueuesStore}
                    t={t}
                ></CreateServiceQ>

                <div className="m-0 mt-2" data-toggle="tooltip" title={t("surgeons.emergency")}>
                    <button className="font-pr inputStyle" data-toggle="modal" data-target="#operationModal" onClick={this.openModal}>
                        <i className="pe-7s-plus fs-20 font-weight-bold basicColorRed" ></i> {t("surgeons.emergency")}
                    </button>
                </div>
                <div className="mt-2" data-toggle="tooltip" title={t("surgeons.upload")}>
                    <button className="font-pr inputStyle" data-toggle="modal" data-target="#UploadSurgeryModal" onClick={this.openModal}>
                        <i className="pe-7s-upload fs-20 font-weight-bold basicColorRed" ></i> {t("surgeons.upload")}
                    </button>
                </div>

                {this.props.operationQueue ?
                    <SurgeryModal
                        t={this.props.t}
                        serviceProviders={this.props.serviceProviders}
                        serviceTypes={this.props.serviceTypes}
                        fellows={this.props.fellows}
                        rooms={this.props.rooms}
                        departments={this.props.departments}
                        AddSurgery={this.props.AddSurgery}
                        load={this.props.load}
                        serviceQueueId={this.props.operationQueue ? this.props.operationQueue.serviceQueue.id : -1}
                        operationQueue={this.props.operationQueue ? this.props.operationQueue : null}
                        dateTime={this.props.shiftDateTime} //{this.state.pickedSq ? this.state.shiftDateTime }
                        state={this.props.state}
                    /> : ""}
                {this.props.operationQueue ?
                    <UploadSurgeryModal
                        t={this.props.t}
                        Upload={this.props.Upload}
                        UploadActual={this.props.UploadActual}
                        load={this.props.load}
                        state={this.props.state}
                        operationQueue={this.props.operationQueue ? this.props.operationQueue : null}
                        serviceQueueId={this.props.operationQueue ? this.props.operationQueue.serviceQueue.id : -1}
                    /> : ""}

            </div>
        )
    }
}

export default withI18n()(AddOperationQueue);
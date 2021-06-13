import React, { Component } from 'react'
import { withI18n } from 'react-i18next';
import * as http from '../../helpers/Http';
import EquipmentStore from '../../stores/Equipment';
import * as $ from 'jquery';

export class IsReadyEqpModal extends Component {
    constructor(props) {
        var equipments = EquipmentStore.fromJS();
        super(props);
        this.state = {
            equipments: this.props.equipments.EquipmentsList,
            equipmentsStatuses: this.fillStatuseOfAnyEquipment(),//dictonary with any equipmentId true or false
            equipmentStore: equipments,
            massege: ""
        }
        this.isExistEquipmentInAllocationOfEquipmentList = this.isExistEquipmentInAllocationOfEquipmentList.bind(this);
        this.findEquipment = this.findEquipment.bind(this);
        this.fillStatuseOfAnyEquipment = this.fillStatuseOfAnyEquipment.bind(this);
        this.saveStateOfReadyEqp = this.saveStateOfReadyEqp.bind(this);
        this.changeStatuseEquipment = this.changeStatuseEquipment.bind(this);
        this.getColorByUnableEquipments = this.getColorByUnableEquipments.bind(this);
    }

    //find eqipment in equipmentAppointmentRequest list
    findEquipment(equipmentId) {
        if (this.props.appointment.operation.equipmentAppointmentRequest && this.props.appointment.operation.equipmentAppointmentRequest.length > 0)
            return this.props.appointment.operation.equipmentAppointmentRequest.find((eq) => eq.eqpId === equipmentId)
        return null;
    }

    //Checks if the equipment has been taken
    isExistEquipmentInAllocationOfEquipmentList(eqpId) {
        var equipment = this.findEquipment(eqpId);
        if (equipment)//this euipment was taken
            return true;
        return false;//was'nt taken
    }

    //fill to any equipment if taken or not in dictonary to every eqpId true taken or false not taken
    fillStatuseOfAnyEquipment() {
        var dict = {}
        this.props.equipments.EquipmentsList.forEach(eqp => {
            dict[eqp.id] = this.isExistEquipmentInAllocationOfEquipmentList(eqp.id)
        });
        return dict;
    }
    //when checked or unchecked equipment the status chang to true or false
    changeStatuseEquipment(e) {
        this.state.equipmentsStatuses[parseInt(e.target.value)] = !this.state.equipmentsStatuses[parseInt(e.target.value)];
        this.setState({ equipmentsStatuses: this.state.equipmentsStatuses });
    }

    //if appointment need some equipment and can't get it return color red
    getColorByUnableEquipments(eqpId) {
        if (this.props.appointment.operation.notEnabledEquipmentsAppointmentRequest && this.props.appointment.operation.notEnabledEquipmentsAppointmentRequest.length > 0) {
            var equipmentId = this.props.appointment.operation.notEnabledEquipmentsAppointmentRequest.find((eq) => eq === eqpId);
            if (equipmentId)
                return "red"
        }
    }
    //save the statuses if enabled
    saveStateOfReadyEqp() {
        var dataForTakingEquipment =
        {
            equipmentsStatuses: this.state.equipmentsStatuses,
            appointment: this.props.appointment
        }
        this.props.load();
        $("#IsRedeyEqpModal").modal('hide');
        http.post('api/ReadyOperation/EquipmentRequest', dataForTakingEquipment)
            .then(res => {
                this.props.appointment.operation.equipmentAppointmentRequest = res;
                this.props.load()
            }).catch(err => {
                alert("Error: ", err);
            })
    }

    render() {
        var t = this.props.t;
        return (
            <div className="modal fade" id="IsRedeyEqpModal" tabIndex="-1" role="dialog" data-backdrop="false" >
                {/* fixed-top overflow-auto */}
                <div className="modal-dialog inner-modal " role="document">
                    <div className="modal-content ">

                        <div className="modal-header p-0 m-0 row justify-content-end">

                            <button type="button" className="close m-0 p-0 col-1" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>

                        <div className="modal-body font-pr">
                            <div>

                                {this.state.equipments ?
                                    this.state.equipments.map((eq => {
                                        return <div className='row m-0 p-0' key={eq.id}>
                                            <p className="mb-0" key={eq.id} style={{ color: this.getColorByUnableEquipments(eq.id) }} >
                                                <input type="checkbox" name={eq.eqpType} value={eq.id}
                                                    checked={this.state.equipmentsStatuses[eq.id]}
                                                    onChange={this.changeStatuseEquipment}
                                                ></input>
                                                {eq.eqpType}</p>
                                        </div>
                                    }
                                    )) : ""}

                            </div>
                        </div>

                        <div className="row modal-footer justify-content-end m-0 p-0 mt-2">
                            <button className="btn btn-outline-primary font-pr m-2"
                                onClick={this.saveStateOfReadyEqp}>
                                save
                        </button>
                        </div>
                    </div>
                </div >
            </div >

        )
    }
}

export default withI18n()(IsReadyEqpModal);

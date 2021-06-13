import React, { Component } from 'react'
import { NavModels } from '../NavModels.js'
import { Statuses } from '../Statuses.js'
import { ReportedDelay } from '../ReportedDelay.js'
import { EditSurgeryDetails } from '../EditSurgeryDetails.js';
import $ from 'jquery';


export class EditSurgeryModal extends Component {
    constructor(props) {
        super(props)
        this.state = {
            navHeadlines: ["Edit status", "Edit surgery details"],
            editMode: "Edit status"
        }
        this.threeModesOfEditing = this.threeModesOfEditing.bind(this);
        this.getModalBodyContent = this.getModalBodyContent.bind(this);
        this.onCloseModal = this.onCloseModal.bind(this);
    }

    threeModesOfEditing(e) {
        this.setState({ editMode: e.target.value });
    }
   

    getModalBodyContent() {
        switch (this.state.editMode) {
            case this.state.navHeadlines[0]:
                {
                    return <Statuses appointment={this.props.appointment}
                        saveStatus={this.props.saveStatus}>
                    </Statuses>
                }
            case this.state.navHeadlines[1]:
                {
                    
                    return <EditSurgeryDetails
                        appointment={this.props.appointment}
                        serviceProviders={this.props.serviceProviders}
                        serviceTypes={this.props.serviceTypes}
                        fellows={this.props.fellows.FellowsList}
                        rooms={this.props.rooms}
                        departments={this.props.departments}
                        t={this.props.t}
                        operationQueue={this.props.operationQueue}
                        state={this.props.state}
                        AddSurgery={this.props.AddSurgery}
                        loading={this.props.loading}
                        onCloseModal={this.onCloseModal}
                        onCloseBigModal={this.props.onCloseModal}
                        rescheduleSetState={this.props.rescheduleSetState}
                        equipments={this.props.equipments}
                    ></EditSurgeryDetails>
                }
            case this.state.navHeadlines[2]:
                {
                    return <ReportedDelay
                        operationStore={this.props.operationStore}
                        updateDelayApointment={this.props.updateDelayApointment}
                        appointment={this.props.appointment}
                    ></ReportedDelay>
                }
        }
    }

    onCloseModal()
    {
        $("#EditSurgeryModal").modal('hide');
    }
    render() {
        return (
            <div className="modal fade" id="EditSurgeryModal" tabIndex="-1" role="dialog" data-backdrop="false" >
                {/* fixed-top overflow-auto */}
                <div className="modal-dialog inner-modal " role="document">
                    <div className="modal-content ">

                        <div className="modal-header p-0 m-0 row">
                            <NavModels col-11 navHeadlines={this.state.navHeadlines}
                                threeModesOfEditing={this.threeModesOfEditing}>
                            </NavModels>

                            <button type="button" className="close m-0 p-0 col-1" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>

                        <div className="modal-body font-pr">
                            {this.state.editMode ? this.getModalBodyContent() : ""}
                        </div>
                    </div>
                </div>
            </div >)
    }
}

export default EditSurgeryModal;

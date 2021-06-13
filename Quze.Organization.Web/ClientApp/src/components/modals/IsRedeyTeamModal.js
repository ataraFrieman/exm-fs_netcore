import React, { Component } from 'react'
import * as http from '../../helpers/Http';
import * as $ from 'jquery';

export class IsRedeyTeamModal extends Component {
    constructor(props) {
        super(props)
        this.state = {
            surgeon: this.props.appointment.operation.teamReady ?
                this.props.appointment.operation.teamReady.surgeon : false,
            anesthesiologist: this.props.appointment.operation.teamReady ?
                this.props.appointment.operation.teamReady.anesthetic : false,
            nurse: this.props.appointment.operation.teamReady ?
                this.props.appointment.operation.teamReady.nurse : false,
            roomCleaned: this.props.appointment.operation.teamReady ?
                this.props.appointment.operation.teamReady.clean : false,
        }
        this.checkedState = this.checkedState.bind(this);
        this.saveStateOfRedeyTeam = this.saveStateOfRedeyTeam.bind(this);
        this.updateOrAddTeamReady = this.updateOrAddTeamReady.bind(this);
    }

    //cheked every person in team if he is reday or not
    checkedState(e) {
        switch (e.target.value) {
            case "Surgeon":
                {
                    this.setState({ surgeon: !this.state.surgeon });
                    break;
                }
            case "Anesthesiologist":
                {
                    this.setState({ anesthesiologist: !this.state.anesthesiologist });
                    break;
                }
            case "Nurse":
                {
                    this.setState({ nurse: !this.state.nurse });
                    break;
                }
            case "Room cleaned":
                {
                    this.setState({ roomCleaned: !this.state.roomCleaned });
                    break;
                }
        }

    }

    //save if team ready or not
    saveStateOfRedeyTeam() {
        var isTeamReady, teamReady;
        if (this.state.surgeon && this.state.anesthesiologist &&
            this.state.nurse || !this.props.appointment.operation.nurse && this.state.roomCleaned)//if all team ready
        {
            this.props.changeTeamState("Enabled");//can start operation
            this.props.appointment.operation.isTeamReady = "Enabled"
        }
        else {
            this.props.changeTeamState("Error");
            this.props.appointment.operation.isTeamReady = "Error"
        }
        teamReady = {
            Id: this.props.appointment.operation.teamReady ? this.props.appointment.operation.teamReady.id : 0,
            Surgeon: this.state.surgeon,
            Anesthetic: this.state.anesthesiologist,
            Nurse: this.state.nurse,
            Clean: this.state.roomCleaned
        }

        isTeamReady = {
            TeamReady: teamReady,
            State: this.props.appointment.operation.isTeamReady,
            OperationId: this.props.appointment.operation.id,
        }
        $("#IsRedeyTeamModal").modal('hide');
        this.updateOrAddTeamReady(isTeamReady)
    }

    updateOrAddTeamReady(isTeamReady) {
        this.props.load();
        http.post('api/Operations/UpdateTeamReady', isTeamReady)
            .then(res => {
                if (res) {
                    this.props.appointment.operation.teamReady = res;
                    this.props.load();
                }
            })
            .catch(err => {
                alert("Error: ", err)
            })
    }

    render() {
        return (
            <div className="modal fade" id="IsRedeyTeamModal" tabIndex="-1" role="dialog" data-backdrop="false" >
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
                                <div className='row m-0 p-0'>

                                    <p className="mb-0">
                                        <input type="checkbox" name="Surgeon" value="Surgeon"
                                            checked={this.state.surgeon}
                                            onChange={this.checkedState}
                                            key={0}></input>
                                        Surgeon- {this.props.appointment.operation.surgeon.fullName}</p>
                                </div>
                                <div className='row m-0 p-0'>

                                    <p className="mb-0">
                                        <input type="checkbox" name="Anesthesiologist" value="Anesthesiologist"
                                            checked={this.state.anesthesiologist}
                                            onChange={this.checkedState}
                                            key={1}></input>
                                        Anesthesiologist- {this.props.appointment.operation.anesthesiologist.fullName}</p>
                                </div>
                                {this.props.appointment.operation.nurse ?
                                    <div className='row m-0 p-0'>
                                        <p className="mb-0">
                                            <input type="checkbox" name="Nurse" value="Nurse"
                                                checked={this.state.nurse}
                                                onChange={this.checkedState}
                                                key={2}></input>
                                            Nurse- {this.props.appointment.operation.nurse.fullName}</p>
                                    </div> : ""}
                                <div className='row m-0 p-0'>

                                    <p className="mb-0">
                                        <input type="checkbox" name="Room cleaned" value="Room cleaned"
                                            checked={this.state.roomCleaned}
                                            onChange={this.checkedState}
                                            key={3}></input>
                                        Room cleaned</p>
                                </div>

                            </div>
                        </div>

                        <div className="row modal-footer justify-content-end m-0 p-0 mt-2">
                            <button className="btn btn-outline-primary font-pr m-2"
                                onClick={this.saveStateOfRedeyTeam}>
                                save
                        </button>
                        </div>
                    </div>
                </div >
            </div >)
    }
}

export default IsRedeyTeamModal;

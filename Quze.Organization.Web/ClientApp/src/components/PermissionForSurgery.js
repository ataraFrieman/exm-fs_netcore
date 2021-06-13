import React, { Component } from 'react';
import IsRedeyTeamModal from './modals/IsRedeyTeamModal';
import IsReadyEqpModal from './modals/IsReadyEqpModal';
import MinimalKit from './MinimalKit'

export class PermissionForSurgery extends Component {
    constructor(props) {
        super(props);
        this.state = {
            statusePmk: this.props.statusePmk,
            statuseTeam: this.props.statuseTeam,
            statuseEqp: this.props.statuseEqp
        };
        this.getColorOfStatuse = this.getColorOfStatuse.bind(this);
        this.retunColorState = this.retunColorState.bind(this);
        this.changeTeamState = this.changeTeamState.bind(this);
        this.changeEqpState = this.changeEqpState.bind(this);
        this.changeMkStautsAndDetails = this.changeMkStautsAndDetails.bind(this);

    }

    //get the color by state
    retunColorState(statuse) {
        switch (statuse) {
            case "Enabled"://ready can start the surgery
                return "#93d993"
            case "Warning"://not yet reday but can start surgery
                return "orange"
            case "Error"://not yet reday can't start surgery 
                return "#ed1c40 "
            case null://not yet reday can't start surgery 
                return "#ed1c40 "
        }
    }

    //three statuses for every group that need to br redey to surgery
    //get the color of level redey
    getColorOfStatuse(state) {
        var color = ""
        switch (state) {
            case "PMK":
                {
                    color = this.retunColorState(this.state.statusePmk);
                    var style = {
                        background: color,
                        width: "80px"
                    };
                    return style;
                }
            case "Team":
                {
                    color = this.retunColorState(this.state.statuseTeam);
                    var style = {
                        background: color,
                        width: "80px"
                    };
                    return style;
                }
            case "Eqp":
                {
                    color = this.retunColorState(this.props.statuseEqp);
                    var style = {
                        background: color,
                        width: "80px"
                    };
                    return style;
                }
        }
    }


    //change team state to enable or warning or unenable
    changeTeamState(status) {
        this.setState({ statuseTeam: status })
        // this.props.statuseTeam=status
    }

    changeEqpState(status) {
        this.setState({ statuseEqp: status })
    }

    changeMkStautsAndDetails(mkStatus, updateMK) {
        console.log("status of MK: ", mkStatus);
        // this.props.updateMinimalKit(status, updateMK)
        this.setState({ statusePmk: mkStatus });
    }

    render() {
        var t = this.props.t;
        return (
            <div className="">
                {/* <i id="miniMalkit" className="fs-16 pe-7s-note2  basicColorRed" data-target="#MinimalKitModal" data-toggle="modal" style={{ cursor: "pointer" }} onClick={this.openMinimalKitModal} /> */}

                <span data-toggle="tooltip" title={t("Users.minimalkit")}>
                    <button className="row justify-content-center fs-16 font-weight-bold"
                        // data-toggle="modal" data-target="#MinimalKitModal"
                        style={this.getColorOfStatuse("PMK")}>
                        {t("PermissionForSurgery.pmk")}
                    </button>
                </span>
                <span data-toggle="tooltip" title={t("PermissionForSurgery.team")}>
                    <button className="row justify-content-center fs-16 font-weight-bold"
                        data-toggle="modal" data-target="#IsRedeyTeamModal"
                        onClick={this.openModal}
                        style={this.getColorOfStatuse("Team")}>
                        {t("PermissionForSurgery.team")}
                    </button>
                </span>
                <span data-toggle="tooltip" title={t("PermissionForSurgery.equipments")}>
                    <button className="row justify-content-center fs-16 font-weight-bold"
                        data-toggle="modal" data-target="#IsRedeyEqpModal"
                        onClick={this.openModal}
                        style={this.getColorOfStatuse("Eqp")}>
                        {t("PermissionForSurgery.eqp")}
                    </button>
                </span>
                {/* WE BE IN THE FUTURE:
                {
                    (this.props.appointment.appointmentTasks && this.props.appointment.appointmentTasks.length ||
                        this.props.appointment.appointmentDocs && this.props.appointment.appointmentDocs.length ||
                        this.props.appointment.appointmentTests && this.props.appointment.appointmentTests.length) ?
                        <MinimalKit
                            appointment={this.props.appointment}
                            changeMkStautsAndDetails={this.changeMkStautsAndDetails}
                            t={this.props.t}
                        />
                        :
                        console.log("there is no MK to show :(")
                }
                 */}

                <IsRedeyTeamModal
                    appointment={this.props.appointment}
                    changeTeamState={this.changeTeamState}
                    load={this.props.load}
                >
                </IsRedeyTeamModal>
                {this.props.equipments ?
                    <IsReadyEqpModal
                        appointment={this.props.appointment}
                        t={this.props.t}
                        equipments={this.props.equipments}
                        updateAppointment={this.props.updateAppointment}
                        onCloseModal={this.props.onCloseModal}
                        changeEqpState={this.changeEqpState}
                        load={this.props.load}
                    ></IsReadyEqpModal> : ""}


            </div >
        );
    }
}
export default PermissionForSurgery;

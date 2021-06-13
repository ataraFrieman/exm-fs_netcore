import React, { Component } from 'react';
import { withI18n } from "react-i18next";
import operation from '../stores/Operation';


export class Statuses extends Component {

    displayName = "Statuses"

    constructor(props) {
        super(props);
        var operationStore = operation.fromJS();
        this.state = {
            statusSelected:this.props.appointment.operation.status?(this.props.appointment.operation.status).toString():"",
            operationStore: operationStore
        }
        this.ChangeStatus = this.ChangeStatus.bind(this);
        this.saveOperationStatuse = this.saveOperationStatuse.bind(this);
        this.isOperationExeclution = this.isOperationExeclution.bind(this);
    }

    ChangeStatus(status) {
        this.setState({ statusSelected: status.target.value })
    }

    saveOperationStatuse() {
        if (this.state.statusSelected) {
            this.props.appointment.operation.status = parseInt(this.state.statusSelected);
            // var sucssefulChangStatus = this.state.operationStore.changStatuseOperation(this.props.appointment);
            this.props.saveStatus(this.props.appointment);
        }
        else
            alert("Cannot save because no status is selected");
    }

    isOperationExeclution() {
        //if operation execlution cant change statuse to patient enters
        if (this.props.appointment.operation.status === 3 || this.props.appointment.operation.status === 3)
            return true;
        return false;
    }

    render() {
        return <div className="container fs-14">
            <div className="row">
                <div className="col-12">
                    <form>
                        <div className="row">

                            <input type="radio" value="option1" className="col-1"
                                checked={this.state.statusSelected === "2"} value={2}
                                onChange={this.ChangeStatus} disabled={this.isOperationExeclution()} />
                            <label>
                                The patient enters the operating room
                            </label>
                        </div>
                        <div className="row">
                            <input type="radio" value="option2" className="col-1"
                                checked={this.state.statusSelected === "3"} value={3}
                                onChange={this.ChangeStatus} />
                            <label>
                                The surgery in execution
                        </label>
                        </div>
                        <div className="row">

                            <input type="radio" value="option3" className="col-1"
                                checked={this.state.statusSelected === "4"} value={4}
                                onChange={this.ChangeStatus} />
                            <label>
                                The surgery will end in about 30 minutes
                        </label>
                        </div>
                        <div className="row">
                            <input type="radio" value="option3" className="col-1"
                                checked={this.state.statusSelected === "5"} value={5}
                                onChange={this.ChangeStatus} />
                            <label>
                                The room is clean now
                        </label>
                        </div>

                    </form>
                    <div className="row modal-footer justify-content-end m-0 p-0 mt-2">
                        <button className="btn btn-outline-primary font-pr m-2"
                         onClick={this.saveOperationStatuse}>
                            save
                        </button>
                    </div>
                </div>
            </div>
        </div>;
    }
}
export default Statuses;
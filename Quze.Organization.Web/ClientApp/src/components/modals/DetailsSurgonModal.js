import React, { Component } from 'react'
import * as $ from "jquery";
import Http from '../../helpers/Http'
import XLSX from 'xlsx';
import * as http from '../../helpers/Http';
import { getOrganizationId } from '../../helpers/AccountService';
import Modal from 'react-responsive-modal';

export class DetailsSurgonModal extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isShowDetailsModal: true
        };
    };


    onCloseModal = () => {
        this.setState({ isShowDetailsModal: false });
    }


    render() {
        return (
            <div open={this.state.isShowDetailsModal} onClose={this.props.onCloseModal} className="modal d-block" tabIndex="-1" role="dialog" data-backdrop="false">
                <div className="modal-dialog direction" role="document">
                    <div className="modal-content" style={{ background: "whitesmoke" }}>
                        <div className="modal-header border-1">
                            <h5 className="modal-title text-muted" >Surgery Details: </h5>
                            <button type="button" className="close m-0 p-1 font-pr" data-dismiss="modal" onClick={this.props.onCloseModal}>
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body font-pr">
                            <div className='row m-0 p-0'>
                                <p>Fellow name: {this.props.resultDetailsModal.fellow.fullName}</p>
                            </div>
                            <div className='row m-0 p-0'>
                                <p>Anesthetic: {this.props.resultDetailsModal.operation.anesthesiologistId}</p>
                            </div>
                            <div className='row m-0 p-0'>
                                <p>Surgeon: {this.props.resultDetailsModal.operation.surgeonId}</p>
                            </div>
                            <div className='row m-0 p-0'>
                                <p>Nurse: {this.props.resultDetailsModal.operation.nurseId}</p>
                            </div>
                            <div className='row m-0 p-0 '>
                                <p>The time of surgery: {new Date(this.props.resultDetailsModal.beginTime).getHours()}
                                    :{new Date(this.props.resultDetailsModal.beginTime).getMinutes()}
                                    - {new Date(this.props.resultDetailsModal.endTime).getHours()}:
                            {new Date(this.props.resultDetailsModal.endTime).getMinutes()} </p>
                            </div>

                        </div>
                        <div className="modal-footer" style={{ justifyContent: "space-between" }}>
                            <button type="button" className="btn btn-primary">surgery is over</button>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

export default DetailsSurgonModal


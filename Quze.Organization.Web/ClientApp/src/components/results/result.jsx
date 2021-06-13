import React, { Component } from 'react';
import Modal from 'react-responsive-modal';
import DatePicker from 'react-datepicker';
import { Alert } from 'react-bootstrap';
import moment from 'moment';
import 'react-datepicker/dist/react-datepicker.css';
import { getUserId } from "../../helpers/User";
import * as http from '../../helpers/Http';
import "react-datepicker/dist/react-datepicker.css";
import AutoCompleteSearch from '../task/AutoCompleteSearch';
import { AddAppointment } from '../modals/AddAppointmentModal';
import '../../css/result.css'
import { observer } from 'mobx-react';
import { withI18n } from 'react-i18next';
class Result extends Component {
    constructor(props) {
        super(props);
        var fellow = null;
        if (props.fellow)
            fellow = JSON.parse(props.fellow);
        this.state = {
            showModal: false,
            open: false,
            date: moment(this.props.nearestAppointmnent.beginTime),
            selectedDate: '',
            fellow: fellow,
            disableRequestQueue: false,
            startReustQueue: false,
            isLoad: false,
            selctedSlot: null,
            appointmentEntity: null
        };
        this.onSetAppointmentRequest = this.onSetAppointmentRequest.bind(this);
        this.setFellow = this.setFellow.bind(this);
    }

    handleDateChange = (date) => {
        this.setState({ date: date });
    }

    isThereQueueToday = (date) => {
        date = date.format('MM/DD/YYYY'); //get date in format
        var exist = this.props.slots.find(x => moment(x.beginTime).format('MM/DD/YYYY') === date);
        return exist;
    };

    appointmentsForToday = (date) => {
        var t = this.props.t;
        date = date.toString();
        date = moment(date).format('MM/DD/YYYY'); //get date in format
        const appointmentsInDate = this.props.slots.filter(appointment => moment(appointment.beginTime).format('MM/DD/YYYY').toString() === date);



        if (typeof appointmentsInDate !== 'undefined' && appointmentsInDate.length > 0) {
            var appointmenesTime = [];
            appointmentsInDate.map((appointment, index) => {
                appointmenesTime.push(<div className="card" dir="rtl" key={index}>
                    <div className="row m-0 p-0" key={appointment.id}>
                        <span className="col-8">{moment(appointment.beginTime.toString()).format("HH:mm") + '-' + moment(appointment.endTime.toString()).format("HH:mm")}</span>
                        <button type='button'
                            onClick={() => { this.onSetAppointmentRequest(appointment); }}
                            className='col-4 btn btn-danger rounded-0 btn-sm text-align'> {t("result.choose")}</button>
                    </div>
                </div>);
            });
        }
        return appointmenesTime;
    };

    onOpenModal = () => {
        this.setState({ open: true });
    };

    onCloseModal = () => {
        this.setState({ open: false });
    };

    setFellow(fellow) {
        this.setState({ fellow: fellow, disableRequestQueue: false });
    }

    //choose date
    OnAppointmentInvite = event => {
        this.setState({ isLoad: true });
        http.post('Appointments/SetAppointment'
            , JSON.stringify({
                entity: {
                    serviceProviderId: this.props.serviceProviderId,
                    branchId: this.props.branchId,
                    serviceTypeId: this.props.serviceType.id,
                    beginTime: event.beginTime,
                    fellowId: getUserId().toString()
                }
            }), null)
            .then(res => res.json())
            .then(res => console.log(res));
    };

    //nearest queue
    onSetAppointmentRequest() {
        var nearestAppointmnent = this.props.nearestAppointmnent;
        this.setState({ isLoading: true, appointmentRequested: true, open: false });
        //check what happend if not logedin? push to "/login" + the data that he select
        var fellow = this.state.fellow;
        //|| !fellow.phoneNumber
        if (!fellow || !fellow.id) {
            this.setState({ disableRequestQueue: true });
            return;
        }
        var beginTime = this.state.selctedSlot ? this.state.selctedSlot.beginTime : nearestAppointmnent.beginTime;
        var serviceTypeId = this.props.serviceType ? this.props.serviceType.id : 1;
        this.setAppintmentServerRequest(fellow.id, fellow.phoneNumber, serviceTypeId, beginTime);
    };

    setAppintmentServerRequest(fellowIdentityNumber, phoneNumber, serviceTypeId, beginTime) {
        var data = {
            entity: {
                serviceProviderId: this.props.serviceProviderId,
                branchId: this.props.branchId,
                serviceTypeId: this.props.serviceTypeId ? this.props.serviceTypeId : serviceTypeId,
                beginTime: beginTime,
                fellowIdentityNumber: this.props.fellowIdentityNumber ? this.props.fellowIdentityNumber : fellowIdentityNumber,
                phoneNumber: this.props.phonNumber ? this.props.phoneNumber : phoneNumber
            }
        };
        var thisObj = this;
        this.setState({ isLoading: false, isLoad: true });
        http.post('Appointments/SetAppointment', data, null)
            .then(res => {

                thisObj.setState({
                    isLoading: false,
                    inviteQResultData: res,
                    appointmentRequested: false
                });
                if (res.entity) {
                    this.setState({ appointmentEntity: res.entity, isLoad: false });
                    if (this.props.setAppointment)
                        this.props.setAppointment(this.state.appointmentEntity);
                    if (this.props.queues)
                        this.props.queues.addAppointement(res.entity.serviceQueueId, res.entity)
                }
            });
    }

    render() {
        var t = this.props.t;
        const { open } = this.state;
        return (
            <div className={'card resultCard rounded-0 bc-gray ' + (this.props.resultClass || "col")} >
                <div className='card-body text-right p-1'>
                    {this.props.hideAfterSchedual ? <button className="btn btn-light btn-sm fs-12 p-1" onClick={this.props.hideResult}>X</button> : ""}
                    <h6 className="card-text m-0 fc-gray pr-3 pt-2  pb-0">
                        <img src={require('../../pictures/icons/doctor.png')} className=" h-20  " />
                        <span className="pl-1 pr-1"> {this.props.serviceProviderName} </span>
                        <span className="font-pr">{this.props.serviceType ? " - " + this.props.serviceType.description : ""}</span>
                    </h6>
                    <p className="card-text text-right m-0 fc-gray  pr-3 pl-3">
                        <img src={require('../../pictures/icons/point.svg')} className=" icon0" />
                        <span className="pl-1 pr-1">  {" " + this.props.branch + ' '}</span>
                        <br />
                        <img src={require('../../pictures/icons/time.png')} className="icon0" />
                        <span className="pl-1 pr-1">
                            {
                                this.state.selctedSlot ? new Date(this.state.selctedSlot.beginTime).toLocaleTimeString() : this.props.nearestAppointmnent && this.props.nearestAppointmnent.beginTime ?
                                    new Date(this.props.nearestAppointmnent.beginTime).toLocaleTimeString()
                                    : ""
                            }
                        </span>
                    </p>
                    {this.state.appointmentRequested ?
                        <AddAppointment t={this.props.t} fellow={this.state.fellow} setFellow={this.setFellow} serviceTypeId={this.props.serviceTypeId} onSetAppointmentRequest={this.onSetAppointmentRequest} /> : ""}
                    {!this.state.appointmentRequested ? <div>
                        <div className="row m-0 p-0 justify-content-center">
                            <button disabled={this.state.disableRequestQueue}
                                className='col-10 font-pr btn btn-danger  rounded-0 p-1 fs-14 '
                                onClick={() => {
                                    this.onSetAppointmentRequest(this.props.nearestAppointmnent);
                                }}
                            >
                                {t("result.Schedualthisappointment")}
                            </button>
                        </div>
                        <div className="row m-0 p-0 justify-content-center">
                            <button type='button' onClick={this.onOpenModal} className='col-8 btn btn-link m-0 p-0 fc-blue border-0 rounded-0 fs-14' >
                                <u>{t("result.Chooseadifferenttime")}</u></button>
                        </div>
                    </div> : ""}
                    <Modal open={open} onClose={this.onCloseModal} classNames={{ modal: "p-1 " }} showCloseIcon={true}>
                        <div className='row m-0 p-0 pt-5'>
                            <div className='col-7 p-0' >
                                {t("result.datepicker")}
                            </div>
                            <p className='col-5 p-0 fs-12'>{t("result.Chooseadate")}</p>
                        </div>
                        <div className='col  p-0 text-right'>
                            <h6> <u> {t("result.Queuesforthedate")} {this.state.date.toDate().getDate() + '/' + (this.state.date.toDate().getMonth() + 1)}</u>  </h6>
                            {this.appointmentsForToday(this.state.date.toDate())}
                        </div>
                    </Modal>

                </div>
                {this.state.isLoad ? <p className="text-right">{t("result.schedualing")}</p> : ""}
            </div>
        );
    }

}

export default withI18n()(observer(Result));

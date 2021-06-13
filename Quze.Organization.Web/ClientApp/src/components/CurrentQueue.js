import React, { Component } from "react";
import '../css/general.css';
import * as http from '../helpers/Http';
import { Appointments } from './Appointments'
import { SQProgress } from './time/SQProgress';

export class CurrentQueue extends Component {
    constructor(props) {
        super(props);
        this.state = { };
        this.advanceQ = this.advanceQ.bind(this);
        this.advanced = false;

    }

    componentDidMount() {
        if (this.props.serviceQ.currentAppointementId > 0) {
            if (document.getElementById(this.props.serviceQ.currentAppointementId))
            document.getElementById(this.props.serviceQ.currentAppointementId).focus();
        }
    }

    componentDidUpdate() {
        if (this.props.serviceQ.currentAppointementId > 0 && this.advanced) {
            document.getElementById(this.props.serviceQ.currentAppointementId).focus();
            this.advanced = false;
        }
    }

    advanceQ() {
        if (this.props.serviceQ.actualBeginTime) {
            http.post('api/qManagament', { ServiceQueueId: this.props.serviceQ.id }, null)
                .then(data => {
                    this.props.advanceQ(data);
                });
            this.advanced = true;
        }
        else {
            http.post('api/qManagament/startQueue',{ ServiceQueueId: this.props.serviceQ.id }, null)
                .then(data => {
                    this.props.advanceQ(data);
                });

        }
    }

    // componentWillMount() {
    // }
    render() {
        
        var t=this.props.t;
        let header = (
            <div >
                <p className="cardTitle">{this.props.serviceQ.serviceProvider?this.props.serviceQ.serviceProvider.fullName:""}</p>
                <p className="m-0">{this.props.serviceQ.serviceStation ? this.props.serviceQ.serviceStation.description : ""}</p>
            </div>
            );
        return (
            <div className="" >
                {header}
                
                <div className="">

                    <div className="cardMargin">
                        <div className="col-12 cardStyleInside ">
                            <Appointments selectedQ={this.props.serviceQ} />
                        </div>
                    </div>

                    <div className='downbutton'>
                    <div className="buttonCard">
                            <button className=" btn btn-outline-dark col-7 p-0 rounded-0 basicColorRed borderRed"
                            onClick={(event) => { this.advanceQ() }} disabled={this.props.serviceQ.passed}>
                            {this.props.serviceQ.actualBeginTime ? t("CurrentQueue.preQueue") : t("CurrentQueue.startQueue")}
                        </button>
                    </div>

                    <SQProgress start={this.props.serviceQ.beginTime} end={this.props.serviceQ.endTime} />

                    </div>
                </div>
            </div>
        );
    }
}

export default CurrentQueue;


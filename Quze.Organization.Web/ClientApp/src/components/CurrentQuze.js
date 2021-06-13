import React, { Component } from "react";
import { CurrentQueue } from './CurrentQueue';
import * as http from '../helpers/Http';
import * as $ from 'jquery';
import '../css/CurrentQuze.css';
import { observer } from "mobx-react";
import { withI18n } from "react-i18next";


export class CurrentQueues extends Component {
    constructor(props) {
        super(props);
        this.state = {
            serviceQuze: []
        };
        this.getCurrentQueue = this.getCurrentQueue.bind(this);
        this.getModalsQuze = this.getModalsQuze.bind(this);
        this.advanceQ = this.advanceQ.bind(this);
        this.HandleDoubleClick = this.HandleDoubleClick.bind(this);
    }

    
    HandleDoubleClick(e, id) {

        if (($("#" + id).data('bs.modal') || {})._isShown)
            return;
        this.state.serviceQuze.map(item => {
            if (item.id  !==  id)
                $("#" + item.id).modal('hide');

        })
        $("#" + id).modal('show');

    }
    
    advanceQ(data) {
        var serviceQuze = this.state.serviceQuze;
        var index = serviceQuze.findIndex(s => s.id == data.id);
        serviceQuze[index] = data;
        this.setState({ serviceQuze: serviceQuze });
    }
    
    componentWillMount() {
    // componentDidMount() {
        var thisObj = this;
        
        http.get('api/Organization/GetCurrentQueue?branchId=' + this.props.branchId)
            .then(function (response) {
                console.log(response);
                if (!response || !response.length)
                    return;
                thisObj.setState({
                    serviceQuze: response
                });
            });
    }

    getModalsQuze() {
        var t=this.props.t;
        var modalsQuze = [];
        this.state.serviceQuze.map((item, index) => {
            modalsQuze.push(
                
            <div key={index} className="modal fade" id={item.id} tabIndex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true" data-backdrop="false">
                                    <div className="modal-dialog" role="document">
                                <div className="modal-content">
                                    <div className="modal-header">
                                    
                                        <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                        <h5 className="modal-title" id="exampleModalLongTitle">{item.id}</h5>

                                    </div>
                                    <div className="modal-body">
                                    </div>
                                    <div className="modal-footer">
                                    {/* Close */}
                                        <button type="button" className="btn btn-secondary" data-dismiss="modal">{t("CurrentQueue.close")}</button>
                                        {/* Save changes */}
                                        <button type="button" className="btn btn-primary">{t("CurrentQueue.saveChanges")}</button>
                                    </div>
                                </div>
                            </div>
                    </div>
            );
        });
        return modalsQuze;

    }

    getCurrentQueue() {
        var thisObj = this;
        var CurrentQueue = [];
        this.state.serviceQuze.map((item) => {
            var SQShortAppointments = item;
            //console.log(item)
            //var startIndex = SQShortAppointments.appointments.indexOf(a => a.id == item.currentAppointment);
            //startIndex = startIndex <= 0 ? 0 : startIndex-1;
            //var endIndex = 2 + (startIndex == 0 ? 1 : startIndex);
            //endIndex = SQShortAppointments.appointments.length < endIndex ? SQShortAppointments.appointments.length : endIndex
            //SQShortAppointments.appointments = SQShortAppointments.appointments.splice(startIndex, endIndex);
            CurrentQueue.push(
                <div key={item.id} id={"q" + item.id} className={"card-Style"} onDoubleClick={e => thisObj.HandleDoubleClick(e, item.id)}>
                    {/* {console.log("q" + item.id)} */}
                    <CurrentQueue t={this.props.t} key={item.id} serviceQ={SQShortAppointments} advanceQ={this.advanceQ} /> 
                </div>
            );
        });
        return CurrentQueue;

    }

    render() {
        
        var t = this.props.t;
        let modalsQuze = this.getModalsQuze();
        let closedQueues = this.props.QueuesList ? this.props.QueuesList.slice(0,5) : [];

        return (
            
            (!closedQueues.length)  ? 

            <div className='card-container'>
                 {t("CurrentQ.noqueues")}
              </div> :

            <div className='card-container'>
                    {closedQueues.map(item => {
                        return <div key={item.id} id={"q" + item.id} className="card-Style m-2" onDoubleClick={e => this.HandleDoubleClick(e, item.id)}>
                            <CurrentQueue t={this.props.t}key={item.id} serviceQ={item} advanceQ={this.advanceQ} />
                        </div>;})}
                {modalsQuze}
               
                </div>

        );
    }
}

export default withI18n()(observer(CurrentQueues));


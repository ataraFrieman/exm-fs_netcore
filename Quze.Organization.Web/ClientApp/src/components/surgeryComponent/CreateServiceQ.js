import React, { Component } from 'react'
import { withI18n } from 'react-i18next';
import $ from 'jquery';
import * as http from '../../helpers/Http';
import { DateModal } from '../modals/DateModal.js'
import { PropagateLoader } from 'react-spinners';
import '../../css/CreateServiceQ.css'
import '../../css/NavCalander.css'

export class CreateServiceQ extends Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            serviceQStore: this.props.serviceQueuesStore
        };
        this.createNewShift = this.createNewShift.bind(this);
    }

    createNewShift(date, start, end, IsPickedSq) {
        $('#serviceQueueModal').modal('hide');
        this.props.setShiftDateTime(date, IsPickedSq)
        let data = {
            date: date,
            startTime: start,
            endTime: end
        }
        this.setState({ loading: true })
        http.post('api/Operations/CreateNewShift', data)
            .then(res => {
                this.setState({ loading: false })
                let serviceQ = [...this.props.serviceQueues, res.entity]
                let serviceQueueId = res.entity.id
                this.props.getServiceQById(serviceQueueId);
                this.props.dateToDatePicker.push(new Date(res.entity.beginTime));
            })

    }

    render() {
        var t = this.props.t;
        return (
            <div >
                <div>
                    <button className="inputStyle" data-toggle="modal" data-target="#serviceQueueModal" onClick={this.openModal}>
                        <i className="pe-7s-timer fs-20 font-weight-bold basicColorRed"></i>
                        Add shift
                    </button>

                </div>
                {this.props.operationQueue ?
                    <DateModal t={t} createNewShift={this.createNewShift} />
                    : ""}
              
            </div>

        )
    }
}

export default withI18n()(CreateServiceQ);
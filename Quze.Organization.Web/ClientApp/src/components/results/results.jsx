import React, { Component } from 'react';
import Result from './result';
import * as http from '../../helpers/Http';
import moment from 'moment';
import { observer } from 'mobx-react';
import { withI18n } from 'react-i18next';


var token = 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImphY29iIiwidHlwIjoiQXBpVXNlciIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiNDBlY2ZjYzktNmIzZS00MDFjLTlkYzctMzI5NGE3Nzk5OWVkIiwiZXhwIjoxODAwNTI0NjMyLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdCJ9.UHBHxFBD3eB4zESXdiquguVXmYyf8ZVDgtoSPkWtlnw';


class Results extends Component {

    constructor(props) {
        super(props);


        this.state = {
            data: '',
            isLoading: 'false',
            BeginTimeObj: this.props.filtersArray.filter(res => res.id === 'dateRangeSelect')
        };
        this.loadData = this.loadData.bind(this);
        this.hideResults = this.hideResults.bind(this);
    }
    
    componentDidMount() {
        this.loadData();
    }

    loadData(duration) {
        var filtersList = this.props.filtersArray;
        var serviceType = filtersList.filter(e => e.id === "serviceTypeSelect")[0];
        var serviceTypeId = serviceType.valueId ? serviceType.valueId : 1;

        var serviceProvider = filtersList.filter(e => e.id === "serviceProviderSelect")[0];
        var serviceProviderId = serviceProvider.valueId ? serviceProvider.valueId : null;

        var dateRangeSelect = filtersList.filter(e => e.id === "dateRangeSelect")[0];
        var beginTime = dateRangeSelect.valueId;
        beginTime = beginTime ? beginTime : new Date();
        var endTime = dateRangeSelect.value;
        endTime = endTime ? endTime : new Date(9999, 1, 1);

        var branch = filtersList.filter(e => e.id === "branchSelect")[0];
        var branchId = branch.valueId;

        var data = {
            serviceQueue: this.props.serviceQueue ? this.props.serviceQueue : null,
            serviceProviderId: serviceProviderId,
            serviceTypeId: serviceTypeId,
            beginTime: moment(beginTime).format("YYYY-MM-DD"),
            endTime: moment(endTime).format("YYYY-MM-DD"),
            cityId: 3650, //what is that id?       
            arrivalTime: 120, //default 2 minutes from now       
            duration: duration,
            hideResults: false
        };
        this.setState({ isLoading: true });
        var url = this.props.serviceQueue && this.props.serviceQueue.id ? 'GetAvailableSlotsToSQ' : 'GetAvailableSlotsBySt';
        http.post('Scheduals/' + url, data)
            .then(res => {
                this.setState({ isLoading: false, data: res })
            });
    }
    hideResults() {
        this.setState({ hideResults: true });
        if (this.props.hideResultFunc)
            this.props.hideResultFunc();
    }
    
    render() {
      var t=this.props.t;
        if (this.state.hideResults)
            return <span></span>;
        //  this.state.data = res; /////////////////////////////////////////// that the place to swicht between server and local array 
        var { isLoading, data } = this.state;
        let serviceKindSelectObj = this.props.filtersArray.filter(res => res.id === 'serviceTypeSelect');
        let BeginTimeObj = this.props.filtersArray.filter(res => res.id === 'dateRangeSelect');
        var thisObj = this;
        if (isLoading)
            return <p className="text-right">{t("MQ.Loadresults")}</p>;
        return (
            <div className='' name='resultsContiner' dir='rtl'>
                <div className='row m-0 p-0'>
                    <p className="text-right pr-4">{(data.entities ? data.entities.length : "0")
                        + t("MQ.outOf") +" "
                        +(data.entities ? data.entities.length : "0")+" "
                        + t("MQ.results")}
                    </p>
                </div>
                <div className="row p-0 m-0">

                    {data.entities !== null ? data.entities.map((result, index) => {
                        var nearestAppointmnent = null;
                        if (result.slots && result.slots.length)
                            if (!thisObj.props.startTime)
                                nearestAppointmnent = result.slots[0]
                            else {
                                nearestAppointmnent = result.slots.filter(s => new Date(s.beginTime) >= thisObj.props.startTime)[0];
                            }
                        return <Result key={index}
                            fellow={this.props.fellow}
                            filtersArray={this.props.filtersArray}
                            profixUrl={this.props.profixUrl}
                            serviceProviderId={result.serviceProviderId}
                            serviceProviderName={result.serviceProviderName}
                            branchId={result.branchId}
                            branch={result.branchName}
                            nearestAppointmnent={nearestAppointmnent ? nearestAppointmnent : (result.slots && result.slots.length ? result.slots[0] : {})}
                            serviceTypeId={result.serviceTypeId}
                            serviceType={result.serviceType}
                            slots={result.slots}
                            history={this.props.history}
                            t={this.props.t}
                            hideAfterSchedual={this.props.hideAfterSchedual}
                            hideResult={this.hideResults}
                            queues={this.props.queues}
                            resultClass={this.props.resultClass || ""}
                            setAppointment={this.props.setAppointment}
                            my_appointment={this.props.my_appointment}

                        />
                    }) : ''}

                </div>
            </div>
        );
    }
}

export default withI18n()(observer(Results));
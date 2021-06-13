import React from 'react'
import * as  BigCalendar from 'react-big-calendar'
//import events from '../events'
//import ExampleControlSlot from '../ExampleControlSlot'
import "../../node_modules/react-big-calendar/lib/css/react-big-calendar.css";
import moment from 'moment';
import { addMinutes, getTime } from '../helpers/TimeService';
import { getStartWeek } from '../helpers/TimeService';
import { getEndWeek } from '../helpers/TimeService';
import { } from '../helpers/TimeService';
import { getDay } from '../helpers/TimeService';
import { getTimeCurrentWeek } from '../helpers/TimeService';
import * as http from '../helpers/Http';
import { getTimaTableModalData } from '../helpers/ModalData';
import * as $ from 'jquery';
import { Modal } from './Modal';
//import branchesStore from '../stores/branches';
import { FormControl } from "react-bootstrap";



const propTypes = {}
const localizer = BigCalendar.momentLocalizer(moment);
//var branches = branchesStore.fromJS();


export class SchedulePlanning extends React.Component {
    constructor(...args) {
        super(...args)
        this.state = {
            events: [],
            timeTables: this.props.serviceProvider.timeTables ? this.props.serviceProvider.timeTables : [],
            start: getStartWeek(new Date()),
            end: getEndWeek(new Date()),
            //updateServiceQueue: this.props.updateServiceQueue,
            view: BigCalendar.Views.WEEK,
            culture: 'en',

            //TTLitem: this.getNewTTL(),
            //TTLmodalData: this.getTTLModalData(this.getNewTTL()),
            TTitem: this.getNewTT(),
            TTmodalData: this.getTTModalData(this.getNewTT()),

            //item: this.getItem(new Date(), new Date()),
            //modalData: this.getModalData(new Date(), new Date(), this.getItem(new Date(), new Date()))
            branch: null,
            dates: null,
            filterSelect: null
        }
        this.renderNav = this.renderNav.bind(this);
        this.createEvenets = this.createEvenets.bind(this);
        this.updateServiceQueue = this.updateServiceQueue.bind(this);
        this.onSelectEvent = this.onSelectEvent.bind(this);
        //this.newTT = this.newTT.bind(this);
        this.TTsaveEvent = this.TTsaveEvent.bind(this);
        this.updateEvent = this.updateEvent.bind(this);
        //this.getItem = this.getItem.bind(this);
        //this.getModalData = this.getModalData.bind(this);
        this.getTTModalData = this.getTTModalData.bind(this);
        //this.getTTLModalData = this.getTTLModalData.bind(this);
        this.handleSelect = this.handleSelect.bind(this);
        this.onBranchChange = this.onBranchChange.bind(this);
        this.onDatesChange = this.onDatesChange.bind(this);
        this.getNewTT = this.getNewTT.bind(this);
        //this.getNewTTL = this.getNewTTL.bind(this);



    }
    componentWillMount() {
        var sq = this.updateServiceQueue(this.props.branch, this.props.serviceProviderId, new Date(), new Date());
        var branches = [];
        this.props.serviceProvider.timeTables.map(item => {
            if (!branches.find(b => b.id == item.branch.id))
                branches.push(item.branch);
        });
        this.setState({
            branch: this.props.serviceProvider.timeTables && this.props.serviceProvider.timeTables.length ?
                this.props.serviceProvider.timeTables[0].branch.id : null,
            dates: this.props.serviceProvider.timeTables && this.props.serviceProvider.timeTables.length ?
                { validFromDate: this.props.serviceProvider.timeTables[2].validFromDate, validUntilDate: this.props.serviceProvider.timeTables[2].validUntilDate } :
                { validFromDate: null, validUntilDate: null },

        });
        this.setState({ event: this.createEvenets() });
    }

    updateServiceQueue(branch, serviceProviderId, date, endDate) {
        var thisObj = this;
        thisObj.createEvenets(this.props.serviceProvider.timeTables[0]);
        //var thisObj = this;
        //console.log("api/Organization/SchdulePlanningData?serviceProviderId=" + serviceProviderId + "&branchId=" + branch + "&dt=" + date.toJSON() + "&timeTable=" + this.state.timeTable);
        //http.get("api/Organization/SchdulePlanningData?serviceProviderId=" + serviceProviderId + "&branchId=" + branch + "&dt=" + date.toJSON() + "&endDate=" + endDate.toJSON() + "&timeTable=" + this.state.timeTable)
        //    .then(function (response) {
        //        if (!response || !response.length)
        //            return;
        //        if (response != "No Content") {
        //            var sq = thisObj.state.timeTables.concat(response);
        //            thisObj.setState({
        //                timeTables: sq
        //            });
        //            thisObj.createEvenets(sq);
        //            return response;
        //        }
        //    });
    }

    getNewTT() {
        var newTT = { id: 0, validFromDate: null, validUntilDate: null, branchId: 2 }
        return newTT;

    }
    onDatesChange(dates) {
        if (dates && dates != 0) {
            var validFromDate = (dates.slice(0, 19));
            var validUntilDate = (dates.slice(20, 40));

            this.setState({ dates: { validFromDate: validFromDate, validUntilDate: validUntilDate } });
            var ttId = this.state.timeTables.find(tt => tt.branch.id == this.state.branch
                && tt.validFromDate == validFromDate
                && tt.validUntilDate == validUntilDate)
            if (ttId)
                this.setState({ event: this.createEvenets(ttId) });
        }
        else {
            var modalData = this.getTTModalData(this.getNewTT());
            this.setState({
                TTitem: this.getNewTT(),
                TTmodalData: modalData
            }, function () {
                $('#newTT0').appendTo('body').modal('show');
            });

        }

    }

    onBranchChange(branchId) {
        if (branchId && branchId != 0) {
            this.setState({ branch: branchId });
            var ttId = this.state.timeTables.find(tt => tt.branch.id == branchId
                && tt.validFromDate == this.state.dates.validFromDate
                && tt.validUntilDate == this.state.dates.validUntilDate)
            if (ttId)
                this.setState({ event: this.createEvenets(ttId) });
        }
        else {
            var modalData = this.getTTModalData(this.getNewTT());
            this.setState({
                TTitem: this.getNewTT(),
                TTmodalData: modalData
            }, function () {
                $('#newTT0').appendTo('body').modal('show');
            });

        }

    }

    createEvenets(TT) {
        var events = [];
        if (this.state.timeTables && this.state.timeTables.length) {
            var sq = TT ? TT : this.state.timeTables[0];
            if (sq.timeTableLines && sq.timeTableLines.length) {
                sq.timeTableLines.map(ttl => {
                    var start = getTimeCurrentWeek(ttl.weekDay, ttl.beginTime);
                    var end = getTimeCurrentWeek(ttl.weekDay, ttl.endTime);
                    events.push({
                        start: start, end: end, title: '\n' + moment(sq.validFromDate).format('DD/MM/YYYY') + ' - ' + moment(sq.validUntilDate).format('DD/MM/YYYY') + '\n' + sq.branch.name, allDay: false, color: "#1b2e4a",
                        ttId: sq.id, ttlId: ttl.id, branch: [sq.branch.id, sq.branch.name], serviceType: sq.serviceType ? [sq.serviceType.id, sq.serviceType.description] : ["", ""]
                    });
                })
            }
        }

        this.setState({
            events: events
        });
    }

    onSelectEvent(event) {
        console.log(event);
        //var timeTable = this.state.timeTables.find(tt => tt.id == event.ttId);
        //var modalData = this.getModalData(null, null, timeTable);
        //this.setState({
        //    item: timeTable,
        //    modalData: modalData
        //}, function () {
        //    $('#newTT0').appendTo('body').modal('show');
        //});
    }

    //getItem(start, end) {
    //    var newTT = this.newTT(start, end);
    //    return newTT;
    //}

    //getModalData(start, end, item) {
    //    var modalData = getTimaTableModalData(item ? item : this.getItem(start, end), branches.branchesList);
    //    return modalData;
    //}

    getTTModalData(TT) {
        var modalData = getTimaTableModalData(TT);
        return modalData;
    }

    handleSelect = ({ start, end }) => {
        //var modalData = this.getModalData(start, end);
        //this.setState({
        //    item: this.getItem(start, end),
        //    modalData: modalData
        //}, function () {
        //    $('#newTT0').appendTo('body').modal('show');
        //});


    }
    //saveEvent(e, newTT) {
    //    console.log("saveEvent");
    //    console.log(newTT);
    //    $('#newTT0').modal('hide');
    //    var thisObj = this;
    //    if (newTT.id == 0)
    //        http.post('api/Organization/CreateTimeTableLine', newTT, null).then(data => {
    //            console.log(data);
    //            newTT = data;
    //            newTT.branch = branches.branchesList.find(b => b.id == newTT.branchId)
    //            var TT = thisObj.state.timeTables;
    //            TT.push(newTT);
    //            thisObj.setState({ timeTables: TT });
    //            thisObj.createEvenets(TT);
    //        });
    //}
    TTsaveEvent(e, newTT) {
        console.log("saveEvent");
        console.log(newTT);
        $('#newTT0').modal('hide');
        var thisObj = this;

        http.post('api/TimeTable/', newTT, null).then(data => {
            console.log(data);
            newTT = data;
            var TT = thisObj.state.timeTables;
            TT.push(newTT);
            thisObj.setState({ timeTables: TT });
            thisObj.createEvenets(TT);
        });
    }

    updateEvent(newTT) {
        console.log("updateEvent");
        console.log(newTT);
        var thisObj = this;
        thisObj.setState({
            item: newTT,
            modalData: this.getModalData(null, null, newTT)
        });
    }

    //newTT(start, end) {
    //    var newTimeTable = { id: 0, serviceProviderId: this.props.serviceProviderId, validFromDate: start, validUntilDate: start, branchId: 0, timeTableLines: [{ id: 0, weekDay: start.getDay() + 1, beginTime: getTime(start), endTime: getTime(end) }] };
    //    return newTimeTable;
    //}

    renderNav() {
        var branches = [];
        var dates = [];
        this.state.timeTables.map(tt => {
            if (!branches.find(b => b.id == tt.branch.id))
                branches.push(tt.branch);
            var date = dates.find(d => d.validFromDate == tt.validFromDate && d.validUntilDate == tt.validUntilDate)
            if (!date)
                dates.push({ validFromDate: tt.validFromDate, validUntilDate: tt.validUntilDate });
        });
        console.log("branches");
        console.log(branches);
        console.log("dates");
        console.log(dates);
        return (
            <div class="border-bottom p-2 m-2">
                <row>
                    <nav className="col-2 navbar p-0 justify-content-start">
                        <span className="icon p-0 navbar-toggler pe-7s-filter" data-toggle="collapse" data-target="#navbarToggleExternalContent" aria-controls="navbarToggleExternalContent" aria-expanded="false" aria-label="Toggle navigation">
                        </span>
                    </nav>

                </row>


                <div class="pos-f-t">

                    <div class="collapse" id="navbarToggleExternalContent">
                        <div className="bg-light p-4 row">
                            <div className="row m-2 direction">
                                <span className="">
                                    <span className="">סניף</span>
                                    <FormControl value={this.state.branch} className="" componentClass="select" placeholder="select" onChange={e => this.onBranchChange(e.target.value)}>
                                        < option value={333} >
                                            333
                                        </option>
                                        {branches.map((b, i) =>
                                            < option value={b.id} >
                                                {b.name}
                                            </option>)}
                                        < option value={0} >
                                            חדש
                                        </option>)
                                    </FormControl>
                                </span>
                                <span className="">
                                    <span className="">טווח תאריכים</span>
                                    {console.log([this.state.dates.validFromDate, this.state.dates.validUntilDate])}
                                    <FormControl value={[this.state.dates.validFromDate, this.state.dates.validUntilDate]} className="" componentClass="select" placeholder="select" onChange={e => this.onDatesChange(e.target.value)}>
                                        < option value={789}>
                                            789
                                        </option>
                                        {dates.map((tt, i) => {
                                            return (< option value={[tt.validFromDate, tt.validUntilDate]} >
                                                {/*[tt.validFromDate, tt.validUntilDate]*/}
                                                {moment(tt.validUntilDate).format('DD/MM/YYYY') + ' - ' + moment(tt.validFromDate).format('DD/MM/YYYY')}
                                            </option>)
                                        })}
                                        < option value={0}>
                                            חדש
                                        </option>)
                                    </FormControl>
                                </span>
                            </div>
                        </div>
                    </div>

                </div>
            </div>);

    }

    render() {
        let formats = {
            dayFormat: (date, culture, localizer) =>
                getDay(date),

            dayRangeHeaderFormat: ({ start, end }, culture, localizer) =>
                ''
        };
        console.log(this.state.modalId);
        //<Modal item={this.state.TTitem} formInputsData={this.state.TTmodalData} id={"newTT0"} saveEvent={this.TTsaveEvent} updateEvent={this.updateEvent} modalId={"newTT0"} showButton={false} ></Modal>

        return (
            <div className="direction">
                {this.renderNav()}
                <div className="BigCalendarContent " dir="ltr">
                    <BigCalendar
                        selectable
                        localizer={localizer}
                        events={this.state.events}
                        defaultView={this.state.view}
                        scrollToTime={new Date(1970, 1, 1, 6)}
                        defaultDate={new Date()}
                        onSelectEvent={event => { this.onSelectEvent(event) }}
                        showMultiDayTimes={true}
                        onSelectSlot={this.handleSelect}
                        drilldownView="agenda"
                        length={7}
                        step={30}
                        culture={this.state.culture}
                        views={{ week: true }}
                        toolbar={false}
                        formats={formats}
                        eventPropGetter={event => ({
                            style: {
                                backgroundColor: event.color,
                                width: "150px",
                                "max-width": "180px",
                                "left": "0%!important"

                            },
                        })}
                    />
                </div>
            </div>

        )
    }
}

SchedulePlanning.propTypes = propTypes

export default SchedulePlanning
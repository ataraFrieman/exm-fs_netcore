import React from 'react'
import * as  BigCalendar from 'react-big-calendar'
import "../../node_modules/react-big-calendar/lib/css/react-big-calendar.css";
import moment from 'moment';
import * as http from '../helpers/Http';
import { observer } from 'mobx-react';
import { getStartWeek } from '../helpers/TimeService';
import { getEndWeek } from '../helpers/TimeService';
import { Appointment } from './Appointment';
const propTypes = {}


const localizer = BigCalendar.momentLocalizer(moment);
export class RealTimeCalendar extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            events: [],
            serviceQueue: this.props.serviceQueue ? this.props.serviceQueue : [],
            start: getStartWeek(new Date()),
            end: getEndWeek(new Date()),
            //updateServiceQueue: this.props.updateServiceQueue,
            view: BigCalendar.Views.DAY,
            timeTable: this.props.timeTable,
            culture: 'en'

        };
        this.updateServiceQueue = this.updateServiceQueue.bind(this);
        this.onSelectEvent = this.onSelectEvent.bind(this);
        this.getEventsArray = this.getEventsArray.bind(this);
        this.getEventStyle = this.getEventStyle.bind(this);
    }

    updateServiceQueue(branch, serviceProviderId, date, endDate) {
        var thisObj = this;
        console.log("api/Organization/GetServiceQueue?serviceProviderId=" + serviceProviderId + "&branchId=" + branch + "&dt=" + date.toJSON());
        http.get("api/Organization/GetServiceQueue?serviceProviderId=" + serviceProviderId + "&branchId=" + branch + "&dt=" + date.toJSON() + "&endDate=" + endDate.toJSON() + "&timeTable=" + this.state.timeTable)
            .then(function (response) {
                if (!response || !response.length)
                    return;
                if (response != "No Content") {
                    var sq = thisObj.state.serviceQueue.concat(response);
                    thisObj.setState({
                        serviceQueue: sq
                    });
                    thisObj.createEvenets(sq);
                    return response;
                }
            });
    }

    onSelectEvent(event) {
        return;
    }

    handleSelect = ({ start, end }) => {
        return;
    }

    getEventsArray() {
        var events = [];
        if (this.state.serviceQueue && this.state.serviceQueue.appointments)
            events = this.state.serviceQueue.appointments.map(a => {
                var start = a.actualBeginTime ? new Date(a.actualBeginTime) : new Date(a.beginTime);               console.log(start);
                 var end=a.actualEndTime?new Date(a.actualEndTime):new Date(a.endTime);
                var title = a.fellow ? a.fellow.fullName : "?";
                title = title;
                return { start: start, end: end, title: title };
            });
        return events;
    }

    getEventStyle(event, start, end, isSelected) {
        var color = "#fff";
        var index, appointment, time = new Date(start).getTime();
        var appointments = this.state.serviceQueue.appointments;
        if (appointments && appointments.length) {
            appointment = appointments.filter(a => new Date(a.beginTime).getTime() == time || new Date(a.actualBeginTime).getTime()==time);

            if (appointment && appointment.length) {
                appointment = appointment[0];
                console.log(appointment.fellow);

                index = appointments.indexOf(appointment);
                color = appointment.actualBeginTime && appointment.actualDuration ? "lightGray" : (appointment.actualBeginTime ? "lightGreen" : "LightYellow");
            }
        };
        var style = {
            backgroundColor: color,
            color: "black",
            height: "20px",
            width: "250px" ,
            left: 0,
            fontSize: "13px",
            borderRadius: 0,
            borderColor:"gray"

        };
        return {style: style};
    }

    render() {
        var ColoredDateCellWrapper = ({ children, value }) => {
            var disableStyle = {
                backgroundColor: "#f3f3f3",
                "pointerEvents": "none!important",
                "opacity": "0.5"
            };
            var regularStyle = { backgroundColor: "#fff" };
            return React.cloneElement(React.Children.only(children), {
                style: new Date(value) < new Date() ? disableStyle : regularStyle,
            })
        };
        var events = this.getEventsArray();
        return (
            <div className="BigCalendarContent calendar-font w-100" dir="ltr">
                <BigCalendar
                    selectable
                    min={new Date(this.state.serviceQueue.beginTime)}
                    max={new Date(this.state.serviceQueue.endTime)}
                    views={['day', 'agenda']}
                    localizer={localizer}
                    events={events}
                    view={ this.props.viewType || "day"}
                    scrollToTime={new Date()}
                    defaultDate={this.state.serviceQueue.beginTime ? new Date(this.state.serviceQueue.beginTime) : new Date()}
                    onSelectEvent={event => { this.onSelectEvent(event) }}
                    showMultiDayTimes={true}
                    onSelectSlot={this.handleSelect}
                    onRangeChange={this.rangeChange}

                    drilldownView="agenda"
                    length={7}
                    step={this.props.stepSize ? this.props.stepSize : 30}
                    toolbar={false}
                    culture={this.state.culture}
                    components={{
                        timeSlotWrapper: ColoredDateCellWrapper,
                    }}
                    eventPropGetter={(this.getEventStyle)}
                />
            </div>

        )
    }
}

RealTimeCalendar.propTypes = propTypes

export default observer(RealTimeCalendar);
import React from 'react';
import { Calendar, momentLocalizer, Views } from 'react-big-calendar';
import "../../node_modules/react-big-calendar/lib/css/react-big-calendar.css";
import moment from 'moment';
import { addMinutes } from '../helpers/TimeService';
import { getStartWeek } from '../helpers/TimeService';
import { getEndWeek } from '../helpers/TimeService';
import { getNextSun } from '../helpers/TimeService';
import { getPrevSa } from '../helpers/TimeService';
import { getDay } from '../helpers/TimeService';
import * as http from '../helpers/Http';
import { observer } from 'mobx-react';

const propTypes = {};


const localizer = momentLocalizer (moment);
export class Selectable extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            events: [],
            serviceQueue: this.props.serviceQueue ? this.props.serviceQueue : [],
            start: getStartWeek(new Date()),
            end: getEndWeek(new Date()),
            view:Views.DAY,
            timeTable: this.props.timeTable,
            culture: 'en'

        }

        this.getEventsArray = this.getEventsArray.bind(this);
        this.getEventStyle = this.getEventStyle.bind(this);
    }

    handleSelect = ({ start, end }) => {
        if (new Date(start) < new Date())
        return;
        this.props.setDateQueue(start, end);
        this.props.setShowQueue(true);
    }

    getEventsArray() {
        var events = [];

        if (this.state.serviceQueue && this.state.serviceQueue.appointments)
            events = this.state.serviceQueue.appointments.map(a => {return { start: new Date(a.beginTime), end: new Date(a.endTime), title: a.fellow ? a.fellow.fullName : "?" }; });
        console.log(events);
        return events;
    }

    getEventStyle(event, start, end, isSelected) {
        var color = "#fff";
        var appointments = this.state.serviceQueue.appointments;
        if (appointments && appointments.length) {
            var appointment = this.state.serviceQueue.appointments.filter(a => a.fellow.fullName == event.title&&new Date(a.beginTime).getTime()==new Date(event.start).getTime());
            if (appointment && appointment.length) {
                appointment = appointment[0];
                console.log(appointment.fellow.fullName);
                color = appointment.actualBeginTime && appointment.actualDuration ? "lightGray" : (appointment.actualBeginTime ? "lightGreen" : "LightYellow");
            }
        };
        var backgroundColor = '#' + event.hexColor;
        var style = {
            backgroundColor: color,
            color: "black",
            height: "20px",
            width: "250px",
            left: 0,
            fontSize: "13px",
            borderRadius: 0,
            borderColor: "gray"
        };
        return {
            style: style
        };
    }

    render() {
  
        var ColoredDateCellWrapper = ({ children, value }) => {
            var disableStyle = {
                backgroundColor: "#f3f3f3",
                pointerEvents: "none!important",
                opacity: 0.5
            };
            var regularStyle = { backgroundColor: "#fff" };
            return React.cloneElement(React.Children.only(children), {
                style: new Date(value) < new Date() ? disableStyle : regularStyle,
            })
        };
        var events = this.getEventsArray();
        return (
            <div className="BigCalendarContent calendar-font w-100" dir="ltr">
                <Calendar
                    selectable
                    min={new Date(this.state.serviceQueue.beginTime)}
                    max={new Date(this.state.serviceQueue.endTime)}
                    views={['day', 'agenda']}
                    localizer={localizer}
                    events={events}
                    view={this.props.viewType || "day"}
                    scrollToTime={new Date()}
                    defaultDate={this.state.serviceQueue.beginTime ? new Date(this.state.serviceQueue.beginTime) : new Date()}
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

Selectable.propTypes = propTypes

export default observer(Selectable);



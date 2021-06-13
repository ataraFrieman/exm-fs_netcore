//DatesRange
import React, { Component } from 'react';

//import 'rc-time-picker/assets/index.css';

//import React from 'react';
import ReactDom from 'react-dom';
import moment from 'moment';
//import TimePicker from 'rc-time-picker';

import InputRange from 'react-input-range';
import "react-input-range/lib/css/index.css";

var minTime = new Date();
minTime.setHours(0, 0);

var maxTime = new Date();
maxTime.setHours(23, 59);

class DatesRange extends Component {
    constructor(props) {
        super(props);
        this.state = {
            value: { min:0, max: 23 },
            minHour: moment(minTime),
            maxHour: moment(maxTime)
        };
    }


    disabledHoursArray = () => {
        var minimumHour = this.state.minHour.toDate().getHours();
        // console.log(minimumHour);
        var disabaledHours = [];
        for (var i = minimumHour - 1; i >= 0; i--)
            disabaledHours.push(i);
        return disabaledHours;
    }
    
    render() {
        var valuesArray = [new Date().setHours(this.state.value.min, 0), new Date().setHours(this.state.value.max,0)]
        return <div dir="ltr" className="h-100">
         <div className=" h-85">
                <InputRange
                    maxValue={24}
                    minValue={0}
                    formatLabel={value => value == 0 ? `12am` : value <= 12 ? `${value}am` : `${value - 12}pm`}
                    value={this.state.value}
                    onChange={val => this.setState({ value: val })} />
            </div>
            <div className="row m-0 p-0 justify-content-center align-items-center">
                <button type="button" className="btn mkSaveBtn col-10 "
                    onClick={() => {
                this.props.onSelect(valuesArray);
                        this.props.closeModal("searchModaltimeRangeSelect");
            }}>בחר</button>
        </div>
        </div>;
    }
}

export default DatesRange;
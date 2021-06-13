import React from 'react';

import DatePicker from 'react-datepicker';
import moment from 'moment';
import 'react-datepicker/dist/react-datepicker.css';

import "react-date-range/dist/styles.css"; 
import 'react-date-range/dist/theme/default.css';
import { DateRangePicker, DateRange } from 'react-date-range';

// CSS Modules, react-datepicker-cssmodules.css
// import 'react-datepicker/dist/react-datepicker-cssmodules.css';

class DatesRange extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            startDate: new moment(),
            endDate: new moment(),
            selectionRange : {
                startDate: new Date(),
                endDate: new Date(),
                key: 'selection'
            }
        };
        this.handleStartDateChange = this.handleStartDateChange.bind(this);
        this.handleEndDateChange = this.handleEndDateChange.bind(this);
        this.handleClick = this.handleClick.bind(this);
        this.handleSelect = this.handleSelect.bind(this);
    }

    handleStartDateChange(date) {
        this.setState({
            startDate: date
        });
    }

    handleEndDateChange(date) {
        this.setState({
            endDate: date
        });

    }

    handleClick = () => {
        const dates = [this.state.selectionRange.startDate, this.state.selectionRange.endDate];
        this.props.onSelect(dates);
    }




    handleSelect(ranges) {
        console.log(ranges);
        let selectionRange = this.state.selectionRange;
        selectionRange.startDate = new Date(ranges.selection.startDate);
        selectionRange.endDate = new Date(ranges.selection.endDate);
        this.setState({ selectionRange: selectionRange });
       
    }
    render() {
        let selectionRange = this.state.selectionRange;
        return (
          
            <div dir="ltr">
                <DateRange
                    minDate={new Date()}
                onChange={this.handleSelect}
                ranges={[selectionRange]}
                className={'PreviewArea'}
                />
                <div className="row m-0 p-0 justify-content-center align-items-center">
                    <button type="button" className="btn mkSaveBtn col-10 "
                        onClick={() => {
                            this.handleClick();
                            this.props.closeModal("searchModaldateRangeSelect");
                        }}>בחר</button>
                </div>
                </div>
        ) }
}

export default DatesRange;


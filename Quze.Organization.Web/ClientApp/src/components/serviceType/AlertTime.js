import React from 'react';
import * as http from '../../helpers/Http';
import * as $ from 'jquery';
import "../../css/general.css"
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { RadioGroup, Radio } from 'react-radio-group';
import { withI18n } from 'react-i18next';


class AlertTime extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            selectedValue: "before",
            before: true,
            selectedHourBefore: '1',
            selectedHourAfter: '0',
            hoursBefore: 0,
            daysBefore: 0,
            hoursAfter: 0,
            daysAfter: 0
        }
        this.specifiedHours = 0;
        this.handleChange = this.handleChange.bind(this);
    }

    componentDidMount = () => {
        var hour = this.props.initialHour;

        if (this.props.initialHour != null)
            if (hour < 0) {
                this.setState({ selectedValue: "after" })
                if (hour == 0)
                    this.setState({ selectedHourAfter: "0" })
                else if (hour == -24)
                    this.setState({ selectedHourAfter: "-24" })
                else if (hour == -168)
                    this.setState({ selectedHourAfter: "-168" })
                else if (hour % 24 != 0)
                    this.setState({
                        selectedHourAfter: "-200",
                        hoursAfter: -hour,
                    })
                else this.setState({
                    selectedHour: "-300",
                    daysAfter: -hour / 24,
                })
            } else {
                this.setState({ selectedValue: "before" })
                if (hour == 1)
                    this.setState({ selectedHourBefore: "1" })
                else if (hour == 24)
                    this.setState({ selectedHourBefore: "24" })
                else if (hour == 168)
                    this.setState({ selectedHourBefore: "168" })
                else if (hour % 24 != 0)
                    this.setState({
                        selectedHourBefore: "200",
                        hoursBefore: hour,
                    })
                else this.setState({
                    selectedHourBefore: "300",
                    daysBefore: hour / 24,
                })
            }
    }

    handleChange = (e) => {
        this.setState({ selectedValue: e });
    }

    setHourBefore = (e) => {
        console.log(e);
        this.setState({ selectedHourBefore: e });
    }
    setHourAfter = (e) => {
        console.log(e)
        this.setState({ selectedHourAfter: e });
    }

    hoursBefore = (e) => {
        this.setState({
            hoursBefore: e.target.value,
        })
        this.setHourBefore('200');
    }
    daysBefore = (e) => {
        this.setState({
            daysBefore: e.target.value,
        })
        this.setHourBefore('300');
    }
    hoursAfter = (e) => {
        this.setState({
            hoursAfter: e.target.value,
        })
        this.setHourAfter('-200');
    }
    daysAfter = (e) => {
        this.setState({
            daysAfter: e.target.value,
        })
        this.setHourAfter('-300');
    }

    getTime = () => {

        if (this.state.selectedValue == 'before') {
            if (this.state.selectedHourBefore == '200')
                return this.state.hoursBefore;
            else if (this.state.selectedHourBefore == '300')
                return this.state.daysBefore * 24;
            else return this.state.selectedHourBefore;
        }
        else {
            if (this.state.selectedHourAfter == '-200')
                return this.state.hoursAfter * (-1);
            else if (this.state.selectedHourAfter == '-300')
                return this.state.daysAfter * 24 * (-1);
            else return this.state.selectedHourAfter;
        }

    }

    render() {
        var t = this.props.t;

        var isNew = this.props.initialHour == null ?t("ATime.new"):t("ATime.old");

        var options = this.state.selectedValue == "before" ?
            <RadioGroup name={isNew + t("ATime.before") + this.props.number} selectedValue={this.state.selectedHourBefore} onChange={this.setHourBefore}>
                <div className="p-2"><Radio value="1" />{t("ATime.HourBefore")} <br /></div>
                <div className="p-2"><Radio value="24" />{t("ATime.DayBefore")} <br /></div>
                <div className="p-2"><Radio value="168" />{t("ATime.WeekBefore")} <br /></div>
                <form className="form-inline">
                {/* Hours before: */}
                    <Radio value="200" /><input id="200" className="form-control textInput modal-input col-2" type="number" min="0" onChange={this.hoursBefore} onClick={this.hoursBefore} value={this.state.hoursBefore} />{t("ATime.Hbefore")} <br />
                </form>
                <form className="form-inline">
                    <Radio value="300" /> <input id="300" className="form-control textInput modal-input col-2" type="number" min="0" onChange={this.daysBefore} onClick={this.daysBefore} value={this.state.daysBefore} />{t("ATime.Dbefore")}<br />
                </form>
            </RadioGroup> :
            <RadioGroup name={isNew + "after" + this.props.number} selectedValue={this.state.selectedHourAfter} onChange={this.setHourAfter}>
                <div className="p-2"> <Radio value="0" />{t("ATime.EndTreatment")}<br /></div>
                <div className="p-2"><Radio value="-24" />{t("ATime.DayAfter")} <br /></div>
                <div className="p-2"><Radio value="-168" />{t("ATime.WeekAfter")} <br /></div>
                <form className="form-inline">
                    <Radio value="-200" /><input id="-200" className="form-control textInput modal-input col-2" type="number" min="0" onChange={this.hoursAfter} onClick={this.hoursAfter} value={this.state.hoursAfter} /> {t("ATime.HoursAfetr")}<br />
                </form>
                <form className="form-inline">
                    <Radio value="-300" /> <input id="-300" className="form-control textInput modal-input col-2" type="number" min="0" onChange={this.daysAfter} value={this.state.daysAfter} />{t("ATime.DaysAfter")} <br />
                </form>
            </RadioGroup>;


        return (<span>

            <br />
            <span className="center-elemnt">
                <div className="btn-group btn-group-toggle" data-toggle="buttons">
                    <label className="btn btn-secondary active" onClick={() => { this.handleChange("before") }} >
                        <input type="radio" name="options" id="before" value="before" />{t("ATime.BeforeTheTreatment")} 
                    </label>
                    <label className="btn btn-secondary centerElemnt" onClick={() => { this.handleChange("after") }}>
                        <input type="radio" name="options" id="after" autoComplete="off" onSelect={this.handleChange} />{t("ATime.AfterTheTreatment")} 
                     </label>

                </div>
            </span>

            <br />
            <span className="center">
                {options}
            </span>

            <br />
        </span>
        );
    }

}

export default AlertTime;
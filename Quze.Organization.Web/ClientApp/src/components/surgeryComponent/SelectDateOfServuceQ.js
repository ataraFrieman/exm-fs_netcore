import React, { Component } from 'react'
import { withI18n } from 'react-i18next';
import { getDateYYYMMDD } from '../../helpers/TimeService'
import '../../css/NavCalander.css'
import DatePicker from "react-datepicker";

export class SelectDateOfServuceQ extends Component {

    constructor(props) {

        super(props);
        var dups = {};
        this.state = {
            serviceQListByDate:this.props.serviceQListByDate
        };
        this.changeDateSurgery=this.changeDateSurgery.bind(this);
    }

    changeDateSurgery(e)
    {
        this.props.getServiceQByDate(getDateYYYMMDD(new Date(e)));
        this.setState({serviceQListByDate:this.props.serviceQListByDate})
    }

    componentDidMount() {

    }
    render() {
        var t = this.props.t;
        var serviceQsDates = this.props.dateToDatePicker.map((item) =>{return getDateYYYMMDD(item)}  )
         serviceQsDates = serviceQsDates.filter((item, index) => serviceQsDates.indexOf(item) === index)
        return (
            <div>
                <div className="row m-1">
                    <div className="row p-1">
                        <input type="search" id="searchDate" className="searchStyle mr-2 ml-2 mt-2"
                         placeholder={t("ServiceProvidersList.Search")} list="serviceQueues"
                         onChange={(e)=>this.props.getServiceQByDate(e.target.value)} />
                        <datalist id="serviceQueues" className="text-left">
                            {
                                this.props.dateToDatePicker ?
                                    serviceQsDates.map((item, i) => {
                                        return <option className="text-left" key={i} >{item}</option>
                                    })
                                    :
                                    "No results"
                            }
                        </datalist>
                    </div>
                    <button className="mt-2 ml-2 todayButtonStyle" onClick={() => this.props.getServiceQByDate(getDateYYYMMDD(new Date()))}>{t("surgeons.today")}</button>
                </div>
                {this.props.dateToDatePicker ?
                <DatePicker className="mt-2"
                    selected={new Date(this.props.startDateForDatePicker)}
                    onChange={(e) => {this.changeDateSurgery(e) }}
                    includeDates={this.props.dateToDatePicker}
                    inline
                />:""}


                {this.state.serviceQListByDate && this.state.serviceQListByDate.length > 1 ?
                    <div>
                        Shifts list
                        <select className="custom-select custom-select-sm font-pr" id="MySelect" placeholder="hellow" onChange={(e) => { this.props.getServiceQById(parseInt(e.target[e.target.selectedIndex].id)) }} >
                            {
                                this.state.serviceQListByDate.map((item, index) => {
                                    return <option key={item.id} id={item.id} value={item.Id} onChange={(e) => { this.props.getServiceQById(parseInt(e.target[e.target.selectedIndex].id)) }}>{item.beginTime}</option>
                                })

                            }
                        </select>
                    </div>
                    : ""}
            </div>
        )
    }
}

export default withI18n()(SelectDateOfServuceQ);
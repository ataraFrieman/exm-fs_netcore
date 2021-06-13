import React, { Component } from 'react';
import history from '../helpers/History';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

export class FilterMenu extends Component {
    //displayName = Home.name

    constructor(props) {
        super(props);
        console.log(props);
        this.state = {
            startDate: new Date(),
            endDate: new Date(),
            serviceTypesList: props.serviceTypeList ? props.serviceTypeList : [],
            selectedServiceType:null
        };
        this.handleChangeStart = this.handleChangeStart.bind(this);
        this.handleChangeEnd = this.handleChangeEnd.bind(this);
        this.handleChangeST = this.handleChangeST.bind(this);
    }
    handleChangeStart(d) {
        this.setState({
            startDate: d
        });
    }
    handleChangeEnd(d) {
        this.setState({
            endDate: d
        });
    }
    handleChangeST(value) {
        this.setState({selectedServiceType: value});
    }
    render() {
        return <div className="row">
                <div className="col-2">
                    מתאריך
                 <DatePicker
                        selected={this.state.startDate}
                    onChange={this.handleChangeStart}
                /></div>
            <div className="col-2">
                עד תאריך
                 <DatePicker
                    selected={this.state.endDate}
                    onChange={this.handleChangeEnd}
                />
            </div>
            <div className="col-2">
                סוג שרות
                <select className="custom-select pl-0 fs-12"
                    onChange={this.handleChangeST}>

                    {this.state.serviceTypesList && this.state.serviceTypesList.length?
                         this.state.serviceTypesList.map((item, index) => {
                            return <option key={item.id} value={item}  >{item.description}</option>;
                    }):""}
                </select>
            </div>
        </div>;
    }
}

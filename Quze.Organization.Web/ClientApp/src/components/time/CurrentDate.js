import React, { Component } from "react";
import { getDateYYYMMDD } from '../../helpers/TimeService'
import '../../css/navMenu.css'

export class CurrentDate extends Component {
    constructor(props) {
        super(props);
        this.state = {
            curDate: getDateYYYMMDD(new Date())
        };
    }

    componentWillMount() {

    }

    componentDidMount() {
        setInterval(() => {
            var d = new Date();
            this.setState({
                curDate: getDateYYYMMDD(d)
            });

        }, 1000);
    }

    render() {
        return (
                    <span className="fullTime">{this.state.curDate}</span>
        );
    }
}

export default CurrentDate;


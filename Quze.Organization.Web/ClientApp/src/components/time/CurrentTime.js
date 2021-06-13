import React, { Component } from "react";
import { getTime } from '../../helpers/TimeService'


export class CurrentTime extends Component {
    constructor(props) {
        super(props);
        this.state = {
            curTime: getTime(new Date()) 
        };
    }

    componentWillMount() {

    }

    componentDidMount() {
        //setInterval(() => {
        //    var d = new Date();
        //    this.setState({
        //        curTime: getTime(d) 
        //    });

        //}, 1000);
    }

    render() {
        return (
            <span className={this.props.className}>{this.state.curTime}</span>
        );
    }
}

export default CurrentTime;


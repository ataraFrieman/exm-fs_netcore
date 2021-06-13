import React, { Component } from "react";


export class SQProgress extends Component {
    constructor(props) {
        super(props);
        this.state = {
            start: new Date(props.start),
            end: new Date(props.end),
            secondsDuration: null,
            startSeconds:null,
            passedPrecent: 0
        };
    }

    componentWillMount() {
        var secondsDuration = (((new Date(this.props.end).getHours()) * 60 * 60 + new Date(this.props.end).getMinutes() * 60 + new Date(this.props.end).getSeconds()) -
            ((new Date(this.props.start).getHours() * 60 * 60) + new Date(this.props.start).getMinutes() * 60 + new Date(this.props.start).getSeconds()));
        var startSeconds = ((new Date(this.props.start).getHours() * 60 * 60) + new Date(this.props.start).getMinutes() * 60) + new Date(this.props.start).getSeconds();
        this.setState({
            secondsDuration: secondsDuration,
            startSeconds: startSeconds
        });

        console.log('this.props.end' + this.props.end);
        console.log('this.props.start' + this.props.start);
        console.log('secondsDuration' + secondsDuration);
    }

    componentDidMount() {
        var thisObj = this;
        setInterval(() => {
            var pass = ((new Date().getHours()) * 60 * 60 + new Date().getMinutes() * 60 + new Date().getSeconds()) -
                thisObj.state.startSeconds;
            var passedPrecent = pass / (thisObj.state.secondsDuration / 100);
            passedPrecent = Math.round(passedPrecent);
            thisObj.setState({
                passedPrecent: passedPrecent
            });

        }, 100);
    }

    render() {
        return (
            <div className="progress">
                <div className="progress-bar progress-bar-success" role="progressbar" aria-valuemin="0" aria-valuemax="100" style={{ width: this.state.passedPrecent + '%' }}  >
                </div>
            </div>
        );
    }
}

export default SQProgress;


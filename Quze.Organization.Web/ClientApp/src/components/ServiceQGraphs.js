import React, { Component } from "react";
import { Row, Col } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Card } from "./Card";
import ChartistGraph from "react-chartist";
import * as http from "../helpers/Http";
import { min } from "moment";
var optionsBar = {
    height: "350px",
    axisY: {
        labelInterpolationFnc: function (value) {
            var minutes = Math.round((value % 1) * 10), m = 0;
            if (minutes > 0)
                m = (minutes / 10) * 60;
            return " " + Math.round(value - 0.5) + ":" + (m ? m : "00");
        }
    }
};
var responsiveBar = [
    [
        "screen and (max-width: 640px)",
        {
            seriesBarDistance: 10,
            axisY: {
                labelInterpolationFnc: function (value) {
                    var minutes = value % 100;
                    if (minutes > 0)
                        minutes = minutes / 100 * 60
                    console.log(minutes)
                    return value[0];
                },
                offset: 100
            }
        }
    ]
];
var legendBar = {
    names: ["שעת התור", "תחילת התור בפועל"],
    types: ["Blue", "Red"]
};
var legendPie = {
    names: ["טופלו", "לא הופיעו"],
    types: ["Blue", "Red"]
};
export class ServiceQGraphs extends Component {

    constructor(props) {
        super(props);
        this.state = {
            selectedQ: props.selectedQ ? props.selectedQ: null,
            QueuesList: props.queuesList ? props.queuesList : []
        };
        
    }
    componentWillMount() {
        if (this.state.selectedQ && this.state.QueuesList && this.state.QueuesList.length) {
            this.setPieData();
            this.setBarData();
            return;
        }
        else {
            var thisObj = this;
            http.get('api/qManagament?serviceProviderId=5')
                .then(function (response) {

                    if (!response || !response.length)
                        return;
                    response.sort(function (a, b) {
                        return new Date(b.actualBeginTime) - new Date(a.actualBeginTime);
                    });

                    thisObj.setState({
                        selectedQ: response[0],
                        QueuesList: response,
                        dataPie: {}
                    });
                    thisObj.setPieData()
                    thisObj.setBarData()
                })
        }

    }
    setPieData() {
        if (!this.state.selectedQ || !this.state.selectedQ.appointments)
            return;
        var served = this.state.selectedQ.appointments.filter(e => e.served == true).length;
        var noShow = this.state.selectedQ.appointments.filter(e => !e.actualBeginTime).length;
        served = served && served > 0 ? served / this.state.selectedQ.appointments.length * 100 : 0;
        noShow = noShow && noShow > 0 ? noShow / this.state.selectedQ.appointments.length * 100 : 0;
        var dataPie = {
            labels: [Math.round(served) + "%", Math.round(noShow) + "%"],
            series: [served, noShow]
        };
        this.setState({ dataPie: dataPie });
    }
    setBarData() {
        if (!this.state.selectedQ || !this.state.selectedQ.appointments)
            return;
        var dataBar = {
            labels: [],
            series: [
                [],
                []
            ]
        };
        for (var i = 0; i < this.state.selectedQ.appointments.length; i++) {
            var item = this.state.selectedQ.appointments[i];
            if (item.fellow) {
                dataBar.labels.push(item.fellow.firstName)
                dataBar.series[0].push(new Date(item.beginTime).getHours() + ((new Date(item.beginTime).getMinutes() / 100)))
                dataBar.series[1].push(new Date(item.actualBeginTime).getHours() + ((new Date(item.actualBeginTime).getMinutes() / 100)))
            }

        }
        var min = Math.min.apply(null, dataBar.series[0]),
            max = Math.max.apply(null, dataBar.series[0]);
        optionsBar.low = min;
        optionsBar.high = max;
        this.setState({ dataBar: dataBar })

    }
    createLegend(json) {
        var legend = [];
        for (var i = 0; i < json["names"].length; i++) {
            var type = "basicColor" + json["types"][i];
            legend.push(<FontAwesomeIcon icon="circle" className={type} key={i} />);
            legend.push(" ");
            legend.push(json["names"][i]);
        }
        return legend;
    }
    render() {
        // var t=this.props.t;
        return <div className="row">
            <div className="col-2 pl-0">
                {this.state.selectedQ?this.state.selectedQ.id:""}
                {/* {t("SQ.showingResults")} */}
                {this.state.QueuesList ?
                    <select className="custom-select pl-0 fs-12" 
                        onChange={e => {
                            var newq= this.state.QueuesList[parseInt(e.target.value)];
                            console.log(newq)
                            var thisObj = this;
                            this.setState({ selectedQ: newq }, function () {
                                thisObj.setPieData();
                                thisObj.setBarData();});
                            console.log(thisObj.state.selectedQ)
                       
                        
                    }}>

                        {this.state.QueuesList.map((item,index) => {
                            return <option key={item.id} value={index}  >
                                {/* {t("MT.num")} */}
                            {item.id} 
                                {/* {t("SQ.outofdate")} */}
                                {new Date(item.beginTime).toLocaleDateString()}
                            </option>;
                        })}

                    </select> : ""}
            </div>

            <div className="col-4">
                {this.state.selectedQ ? <Card
                    statsIcon="fa fa-clock-o"
                    title=""
                    // {t("SQ.Lineattendancegraph")}
                    content={
                        <div
                            id="chartPreferences"
                            className="ct-chart ct-perfect-fourth"
                        >
                            {this.state.dataPie ? <ChartistGraph data={this.state.dataPie} type="Pie" /> : "nd"}
                        </div>
                    }
                    legend={
                        <div className="legend">{this.createLegend(legendPie)}</div>
                    }
                /> : ""}
            </div>
            <div className="col-6">
                <Card
                    id="chartActivity"
                    title=""
                    // {t("SQ.ExpectArrivalvsActualArrival")}

                    content={
                        <div className="ct-chart">
                            {this.state.dataBar ? <ChartistGraph
                                data={this.state.dataBar}
                                type="Bar"
                                options={optionsBar}
                            /> : ""}
                        </div>
                    }
                    legend={
                        <div className="legend">{this.createLegend(legendBar)}</div>
                    }
                />
            </div>
        </div>


    }
};
export default ServiceQGraphs;

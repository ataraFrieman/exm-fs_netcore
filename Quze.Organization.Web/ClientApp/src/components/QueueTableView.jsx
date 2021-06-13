import React from 'react'
import BigCalendar from 'react-big-calendar'
import * as http from '../helpers/Http';
import { Row, Col, Table} from "react-bootstrap";
import Card from "./Card";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { getTime, getDate } from '../helpers/TimeService';

export class QueueTableView extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            
        }
    }

    componentWillMount() {
       
    }

    render() {
        var t=this.props.t;
        var Q = this.props.Q, isCurrent = this.props.isCurrent, isPassed = this.props.isPassed, spName = this.props.spName, appointments = this.props.appointments;
        return <div >
            <Row className="m-0">
                <div className="col-12 text-right" >
                    <Card
                        title={spName}
                        category={Q.branch ? Q.branch.name : ""}
                        subCategory={new Date(Q.beginTime).toLocaleTimeString() + " - " + new Date(Q.endTime).toLocaleTimeString()}
                        ctTableFullWidth
                        ctTableResponsive
                        content={
                            <Table className="text-center" striped hover>
                                <thead>
                                    <tr>
                                        <th>{t("MT.num")}</th>
                                        <th>{t("MT.follow")}</th>
                                        <th>{t("MT.typeService")}</th>
                                        <th>{t("MT.startTime")}</th>
                                        <th>{t("MT.Actualstarttime")} </th>
                                        <th>{t("MT.duration")}</th>
                                        <th>{t("MT.Expectnonarrival")}</th>
                                        <th>{t("MT.treated")}</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {appointments.map(item =>
                                        <tr key={item.id}>
                                            <td>{item.id}</td>
                                            <td>{item.fellow ? item.fellow.firstName + ' ' + item.fellow.lastName : ''}</td>
                                            <td>{item.serviceType ? item.serviceType.description : ''}</td>
                                            <td>{getTime(item.beginTime)}</td>
                                            <td>{isPassed || isCurrent ? new Date(item.actualBeginTime).toLocaleTimeString() : "-"}</td>
                                            <td>{this.props.secondsToHms(item.duration)}</td>
                                            <td><FontAwesomeIcon className={item.noShow > 0 ? (item.noShow < 5 ? "fc-yellow" : item.noShow < 8 ? "text-warning" : "text-danger") : "text-muted"} icon="circle" /> </td>
                                            <td>{item.served ? <i className='pe-7s-check text-success fs-16 font-weight-bold' /> : ""} </td>
                                        </tr>
                                    )}
                                </tbody>
                            </Table>
                        }
                    />
                </div>
            </Row>
        </div>;
        
    }
}

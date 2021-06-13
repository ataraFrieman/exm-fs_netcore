import React, { Component } from 'react';
import 'react-datepicker/dist/react-datepicker.css';
import HeaderTitlePage from './HeaderTitlePage';
import Search from './search/Search';
import Results from './results/results';
import { observer } from 'mobx-react';
import { withI18n } from "react-i18next";
import { withRouter } from 'react-router-dom';
import "./../css/Schedual.css";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const localUrl = 'http://localhost:3002/';

var schedualFiltersList = {};

class Schedual extends Component {

    constructor(props) {
        super(props);
        console.log(props);
       
        this.state = {
            profixUrl: localUrl,
            isShowAlert: false,
            my_appointment:null
        };
        this.setAppointment = this.setAppointment.bind(this);

        schedualFiltersList = this.props.filtersList;
        this.childRef = React.createRef();
    }

    setAppointment(appointment) {
        this.setState({ my_appointment: appointment });
        this.setState({ showResults: false });
        this.setState({ isShowAlert: true });
    }

    render() {
        var t = this.props.t;
        var fellow=this.props.history.location&&this.props.history.location.state?this.props.history.location.state.fellow:null;
        return (
            <div className='' >
                <HeaderTitlePage title={fellow ? t("shcedual.makeanewqueue") + JSON.parse(fellow).firstName + " " + JSON.parse(fellow).lastName : ""} />
                <div className='row m-0 p-0 '>
                    <div className='p-0 w-100'>
                        <Search profixUrl={this.state.profixUrl}
                            filtersArray={this.props.filtersList}
                            serviceTypes={this.props.serviceTypes}
                            serviceTypesList={this.props.serviceTypesList}
                            serviceProviders={this.props.serviceProviders}
                            branches={this.props.branches}
                            t={this.props.t}
                        />
                    </div>
                </div>
                <div className='row m-0'>
                    <div className='col-sm p-0'>

                        <button className="m-2 font-pr basicColorBlue" onClick={e => {
                            this.setState({ showResults: true });
                            if (this.childRef.current && this.childRef.current!==null)
                                this.childRef.current.loadData();
                        }}>
                          {t("schedul.search")}
                          <i className="pe-7s-search fs-20 icon"></i>
                        </button>
                        {this.state.showResults ?
                            <Results ref={this.childRef}
                                resultClass={this.props.resultClass}
                                fellow={this.props.history.location && this.props.history.location.state ? this.props.history.location.state.fellow : null}
                                profixUrl={this.state.profixUrl}
                                filtersArray={this.props.filtersList}
                                setAppointment={this.setAppointment}
                                my_appointment={this.state.my_appointment}
                            /> : <span />}

                        {this.state.isShowAlert ?
                            <div >
                                <div className="cover"></div>
                                <div className="shadow alert">
                                    <a class="close text-danger" data-dismiss="alert" aria-label="close" onClick={e => {//x
                                        this.setState({ isShowAlert: false });
                                    }}>&times;</a>
                                    <img className="img" src={require('../pictures/appointment.JPG')}/>  <br /><br />
                                    <h5> {t("alertAppointment.succesfuly")}</h5>
                                    <FontAwesomeIcon icon="calendar" /> {t("alertAppointment.date")} {new Date(this.state.my_appointment.beginTime).toLocaleDateString()} &nbsp;&nbsp;&nbsp;&nbsp;
                                    <img src={require('../pictures/icons/doctor.png')} className=" h-20  " />{t("alertAppointment.service")}  {this.state.my_appointment.serviceProvider.fullName} &nbsp;&nbsp;&nbsp;&nbsp;
                                    <FontAwesomeIcon icon="clock" /> {t("alertAppointment.duration")}  {this.state.my_appointment.duration / 60} דקות  &nbsp;&nbsp;&nbsp;&nbsp;
                                    <br/><br />                                  
                                </div>       
                            </div>
                            : ""
                        }
                    </div>
                </div>
            </div>
        );
    }
}
export default withRouter(withI18n()(observer(Schedual)));



import React, { Component } from 'react';
import { Table, Col, Row, Card, Button, Modal } from 'react-bootstrap';
import Select from 'react-select';
import AutoCompleteSearch from '../task/AutoCompleteSearch';
import * as http from '../../helpers/Http';
export class AddAppointment extends Component {
    constructor(props) {
        super(props);
        this.state = {
            serviceTypesList: [],
            fellow: null
        };
        this.selectFelloeCallback = this.selectFelloeCallback.bind(this);
    }
    loadSubCategory() {
        var STId = this.props.serviceTypeId;
        if (!STId)
            return;
        http.get("api/ServiceTypes/GetSTChildren?serviceTypeId=" + STId)
            .then(res => {
                res = res.entities.map(st => ({ value: st.id, label: st.description }));
                this.setState({ serviceTypesList: res });
            });
    }
    componentDidMount() {
        this.loadSubCategory();
    }
    selectFelloeCallback(selectedFellow) {
        this.setState({ fellow: selectedFellow });
        this.props.setFellow(selectedFellow);
    }
    render() {
        var t=this.props.t;
        return <div open={this.props.open}
            onClose={this.onCloseModal}
            className="p-3">
            <div className='row p-0 text-right font-pr'>
                <span className="col-4 fs-12 p-0 pt-2">{t("AddApoint.chooseUser")}</span>

                <AutoCompleteSearch t={this.props.t} classes="col-6 p-0 m-0" searchType="fellowSearch" placeholder={t("AddApoint.Lookforacolleague")}
                    redirectAfterSelect={false} callback={this.selectFelloeCallback} />

            </div>
            {this.state.serviceTypesList.length ?
                <div className=' p-0 text-right row font-pr'>
                    <h6 className="col-4 fs-12 p-0 pt-2">{t("AddApoint.Selectasubcategory")}</h6>
                    <Select className="col-6 fs-14 p-0 pt-2 test1"

                        options={this.state.serviceTypesList}
                        placeholder={this.state.serviceTypesList[0].label}
                        arrowrenderer="true" ></Select>
                </div>
                : ""}
            <div className="row m-0 p-0 justify-content-center mt-2">
                <button className='col-8 btn btn-outline-danger rounded-0 p-1 fs-14 '
                    disabled={!this.state.fellow}
                    onClick={() => {
                        this.props.onSetAppointmentRequest();
                    }}>{t("AddApoint.Schedule")} </button>
            </div>
        </div>
    }
}
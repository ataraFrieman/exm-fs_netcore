import React, { Component } from 'react';
import { Grid, Row, Col, Table } from "react-bootstrap";
import Card from "./Card";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { getFellowModalData } from "../helpers/ModalData";
import { Modal } from './Modal';
import * as http from '../helpers/Http';
import * as $ from 'jquery';
import { observer } from 'mobx-react';
import { trace } from 'mobx';
import { withI18n } from 'react-i18next';
import SearchTemplate from './SearchTamplate';

import history from '../helpers/History';
import AutoCompleteSearch from './task/AutoCompleteSearch';
class FellowsList extends Component {

    displayName = "FellowsList";

    constructor(props) {
        super(props);
        this.state = {
            fellows: null,
            filteredFellows: null,
            organizationName: "",
            newFellow: null,
            modalData: null,
            showAlert: false,
            displayUpdateModal: false,
            selectedFellow: null
        };
        this.renderNav = this.renderNav.bind(this);
        this.saveEvent = this.saveEvent.bind(this);
        this.getEmptyFellow = this.getEmptyFellow.bind(this);
        this.filterAll = this.filterAll.bind(this);
        this.deleteFelow = this.deleteFelow.bind(this);
        this.editFellow = this.editFellow.bind(this);
        this.addFellow = this.addFellow.bind(this);
        this.hideModal = this.hideModal.bind(this);
        //this.renderUpdateModal = this.renderUpdateModal.bind(this);
        this.upateFellow = this.upateFellow.bind(this);
        this.setSelectedFellow = this.setSelectedFellow.bind(this);
    }

    componentWillMount() {
        var fellows = this.props.fellows.FellowsList;
        //if (fellows && fellows.length)
        this.setState({
            fellows: fellows,
            filteredFellows: fellows,
            organizationName: this.props.organizationName,
            newFellow: this.getEmptyFellow(),
            modalData: getFellowModalData(this.getEmptyFellow(), this.props.t),
            upateFellow: this.getEmptyFellow(),
            updateModalData: getFellowModalData(this.getEmptyFellow(), this.props.t)
        });
    }

    getEmptyFellow() {
        var emtpyFellow = { id: 0, organizationId: this.props.organizationId, firstName: "", lastName: "", identityNumber: "", phone: "" };
        return emtpyFellow;
    }
    setSelectedFellow(fellow) {
        this.setState({
            selectedFellow: fellow,

        });
    }
    saveEvent(e, newFellow) {
        var isUpdate = newFellow.id != 0;
        if (isUpdate)
            return this.upateFellow(newFellow);
        this.setState({
            modalData: getFellowModalData(this.getEmptyFellow(), this.props.t), displayModal: false
        });

        http.post("api/Organization/CreateFellow", newFellow, null)
            .then(data => {
                newFellow.id = data;
                this.props.fellows.addFellow(newFellow);
                var e = { target: { value: $('#filterAllFellows').val() } };
                this.filterAll(e);
                this.setState({ newFellow: this.getEmptyFellow() });
            });
    }

    upateFellow(newFellow) {
        this.setState({ updateModalData: getFellowModalData(this.getEmptyFellow(), this.props.t) });
        http.post("api/fellows/UpdateFellow", newFellow, null)
            .then(data => {
                var e = { target: { value: $('#filterAllFellows').val() } };
                this.filterAll(e);
                this.setState({ newFellow: this.getEmptyFellow(), displayModal: false });


            });
    }
    openSchedual(fellow) {

        history.push("/schedual", { fellow: JSON.stringify(fellow) });
        window.location.reload();
    }
    //updateEvent(newFellow) {
    //    this.setState({
    //        updateFellow: newFellow,
    //        updateModalData: getFellowModalData(newFellow),
    //    });
    //}

    filterAll(e) {
        this.setState({
            filteredFellows: this.state.fellows
        });
        if (e.target.value != "") {
            var filteredFellows = this.state.fellows.filter(
                b => {
                    return b.firstName && b.firstName.toString() && b.firstName.toString().indexOf(e.target.value) > -1 ||
                        (b.lastName && b.lastName.includes(e.target.value)) ||
                        (b.identityNumber && b.identityNumber.includes(e.target.value));
                }
            );

            this.setState({ filteredFellows: filteredFellows });
        }
    }

    renderNav() {
        var t = this.props.t;
        return (
            <div className="row m-0">
                <div className="row m-0">
                    <nav className="col-12 navbar p-0 justify-content-start">
                        <SearchTemplate t={this.props.t} click={this.addFellow} change={e => this.filterAll(e)} placeholdersearch={t("Follow.search")} titelnUser={t("Follow.addFellow")}></SearchTemplate>

                        {/* <button className="btn btn-light m-0 p-0 bg-transparent border-0" onClick={this.addFellow}>
                            <i id="open" className={"icon fs-24 pe-7s-plus p-0"} />
                        </button> */}
                        {this.state.modalData && this.state.displayModal ?
                            <Modal t={this.props.t} item={this.state.newFellow}
                                saveEvent={this.saveEvent}
                                modalId={this.state.newFellow.id}
                                showButton={false}
                                formInputsData={this.state.modalData}
                                hideModal={this.hideModal}></Modal> : ""}
                    </nav>
                </div>
            </div>)
    }

    addFellow() {
        var data = getFellowModalData(this.getEmptyFellow(), this.props.t);
        this.setState({ modalData: data, newFellow: this.getEmptyFellow(), displayModal: true });
    }

    deleteFelow(fellow) {
        var thisObj = this;
        http.deleteX('api/Fellows', fellow, null)
            .then((res) => {
                this.props.fellows.deleteFellow(fellow.id);
            });
    }

    editFellow(fellow) {
        var data = getFellowModalData(fellow, this.props.t);
        var item = fellow;
        this.setState({ modalData: data, newFellow: item }, () => {
            this.setState({ displayModal: true })
            console.log(this.state);
        });
    }

    hideModal() {
        this.setState({ displayModal: false });
    }

    //renderUpdateModal() {
    //    return <Modal t={this.props.t} item={this.state.newFellow} saveEvent={this.saveEvent}
    //        modalId={this.state.newFellow.id} showButton={false}
    //        formInputsData={this.state.ModalData} ></Modal>;

    //}
    renderFellow=() => {
        var t=this.props.t;
        var fellows = this.props.fellows.FellowsList;
        return fellows.map((item) => {
            var item = item;
            return item.id ? <div className={" rounded-0 border-white btn btn-default col-lg-3 col-md-5 col-sm-10 m-1 bg-white "} key={item.id}>
                <div className=" card-body ">
                <div className="row justify-content-end">
                    <div className="" data-toggle="modal" onClick={(evt) => { this.editFellow(item); }} >
                        <i id="open" className={"icon fs-24 pe-7s-note"} data-toggle="tooltip" title={t("Users.open")} />
                    </div>
                     <i id="delete" className={"icon fs-24 pe-7s-trash"} onClick={(evt) => { this.deleteFelow(item); }} data-toggle="tooltip" title={t("Users.delete")} />
                </div>

                <h6 className={"card-subtitle  text-muted text-center font-pr fs-16 basicColorBlue"}>{item ? item.firstName + " " + item.lastName : ""}</h6>
                <br></br>
                <div className="row">
                    <div className=" col-2 p-0 ">
                        <i className="fs-24 pe-7s-user" />
                    </div>
                    <span className=" col-10 p-0 text-right text-muted fs-12">{item.identityNumber}</span>
                </div>
            </div>
            </div> : " ";
        });
    }

    render() {
        var t = this.props.t;
        var fellows = this.props.fellows.FellowsList;
        if(this.state.selectedFellow)
        fellows=fellows.filter(f=>f.id==this.state.selectedFellow.id);
        if (!fellows || !fellows.length)
            return <span></span>;
        else
            return (
                <div className="col-12">
                    {this.renderNav()}
                    {this.state.showAlert ? <div className="alert alert-success" role="alert">
                        {t("Follow.theFollow")}  {this.state.newFellow ? this.state.newFellow.firstName + " " + this.state.newFellow.lastName : ""} {t("Follow.succes")}</div> : ""}
                    {this.renderFellow()}
                </div>
            );
    }
}
export default withI18n()(observer(FellowsList));
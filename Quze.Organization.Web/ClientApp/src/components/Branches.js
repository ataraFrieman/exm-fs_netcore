import React, { Component } from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import BootstrapTable from 'react-bootstrap-table-next';
import cellEditFactory, { Type } from 'react-bootstrap-table2-editor';
import 'react-bootstrap-table2-paginator/dist/react-bootstrap-table2-paginator.min.css';
import paginationFactory from 'react-bootstrap-table2-paginator';
import '../css/general.css';
import { Branch } from './Branch';
import { createBrowserHistory } from "history";
import * as http from '../helpers/Http';
import * as $ from 'jquery';
import { getBranchModalData } from "../helpers/ModalData";
import { Modal } from './Modal';
import { getUser } from '../helpers/User';
import { observer } from 'mobx-react';
import '../css/users.css'
import { withI18n } from 'react-i18next';
import SearchTemplate from './SearchTamplate';

class Branches extends Component {
    displayName = Branches.name

    constructor(props) {
        super(props)
        var branches = props.branches.branchesList;
        this.state = {
            filteredBranches: branches ? branches.filter(b => b.isDeleted === false) : [],
            loadingStreets: true,
            loadingCities: true,
            streets: null,
            cities: null,
            dropDownCities: [],
            currentCity: 0,
            linkClick: false,
            serviceProviders: [],
            organizationName: "",
            filter: { showArchived: false },
            newBranch: null,
            modalData: null,
            showModal: false
        };
        this.onDelete = this.onDelete.bind(this);
        this.getFilteredStreets = this.getFilteredStreets.bind(this);
        //this.renderBranchesTable = this.renderBranchesTable.bind(this);
        this.getDropDownItems = this.getDropDownItems.bind(this);
        this.onClickBranch = this.onClickBranch.bind(this);
        this.onSave = this.onSave.bind(this);
        this.update = this.update.bind(this);
        this.renderNav = this.renderNav.bind(this);
        this.filterButtonOnClick = this.filterButtonOnClick.bind(this);
        this.filterAll = this.filterAll.bind(this);
        this.filter = this.filter.bind(this);
        this.newBranch = this.newBranch.bind(this);
        this.saveEvent = this.saveEvent.bind(this);
        this.updateEvent = this.updateEvent.bind(this);
        this.openBranchModal = this.openBranchModal.bind(this);
        this.hideModal = this.hideModal.bind(this);
        this.renderBranchescards = this.renderBranchescards.bind(this);
        this.user = getUser();

        http.get('api/Address/GetStreets')
            .then(data => {
                this.setState({ streets: data, loadingStreets: false });
                if (this.state.cities)
                    this.setState({
                        newBranch: this.newBranch(),
                        modalData: getBranchModalData(this.newBranch(), data, this.state.cities, this.saveEvent)
                    });
            });

        var items;
        http.get('api/Address/GetCities')
            .then(data => {
                items = this.getDropDownItems(data);
                this.setState({ cities: data, dropDownCities: items, loadingCities: false });
                if (this.state.streets)
                    this.setState({
                        newBranch: this.newBranch(),
                        modalData: getBranchModalData(this.newBranch(), this.state.streets, data, this.saveEvent)
                    });
            });
        //this.filter();

        //var filteredBranches = this.props.branches.branchesList.filter(b => !b.isDeleted);
        //this.setState({
        //    filteredBranches: filteredBranches,
        //    newBranch: this.newBranch(),
        //    modalData: getBranchModalData(this.newBranch(), this.state.streets, this.state.cities, this.saveEvent)
        //})
        //if (this.props.serviceProviders && this.props.serviceProviders.length)
        //    this.setState({ serviceProviders: this.props.serviceProviders, organizationName: this.props.organizationName });

    }

    componentWillMount() {
        this.setState({
            newBranch: this.newBranch(),
            modalData: getBranchModalData(this.newBranch(), this.state.streets, this.state.cities, this.saveEvent)
        });
    }

    onDelete(id) {
        var deleteItem = this.props.branches.branchesList.filter(b => b.id === id);
        if (deleteItem && deleteItem.length) {
            deleteItem = deleteItem[0];
            deleteItem.isDeleted = 'true';
        }
        console.log(deleteItem);
        http.deleteX('api/Branches',
            deleteItem.id
            , null);
        this.filter();
        var e = { target: { value: $('#filterAll').val() } };
        this.filterAll(e);

    }

    openBranchModal(modalId) {
        this.setState({ showModal: true });
        $(modalId).appendTo("body").modal('show');
    }

    hideModal() {
        this.setState({ showModal: false });
    }

    saveEvent(e, newBranch) {
        console.log(newBranch);
        $('#editBranch0').modal('hide');

        this.setState({
            newBranch: this.newBranch(),
            modalData: getBranchModalData(this.newBranch(), this.state.streets, this.state.cities, this.saveEvent)
        });

        http.post('api/Branches', newBranch, null).then(data => {
            console.log(data);
            newBranch.id = data;
            var branches = this.state.branches;
            branches.push(newBranch);
            this.setState({ branches: branches });
            this.filter();
            var e = { target: { value: $('#filterAll').val() } };
            this.filterAll(e);
        });
    }

    updateEvent(newBranch) {
        this.setState({
            newBranch: newBranch,
            modalData: getBranchModalData(newBranch, this.state.streets, this.state.cities, this.saveEvent)
        });
    }

    onSave(id) {
        var editBranch = this.props.branches.branchesList.filter(b => b.id === id);
    }

    getDropDownItems(array) {
        //Branches.filterStreets(sender, )
        if (array) {
            array.sort(function (a, b) {
                return a.name.localeCompare(b.name);
            });
            var items = [];
            var streets = array;//sender.state.streets;
            for (var i = 0; i < streets.length; i++) {
                items.push({ value: streets[i].id, label: streets[i].name })
            }
            return items;
        }
    }

    getFilteredStreets() {
        if (this.state.currentCity !== 0) {
            var cityId = this.state.currentCity;
            var streets = this.state.streets.filter(s => s.cityId === cityId);
            return this.getDropDownItems(streets);
        }
        else {
            return this.getDropDownItems(this.state.streets);
        }
    }

    onClickBranch(id) {
        var currentBranch = this.state.filteredBranches.filter(b => b.id === id);
        var serviceTypes = [];
        //serviceTypes = this.state.serviceTypes.filter(st => st.br)

        if (currentBranch && currentBranch.length) {
            var input = {
                branch: currentBranch[0], streets: this.state.streets, cities: this.state.cities,
                serviceProviders: this.state.serviceProviders, organizationName: this.state.organizationName,
                serviceTypes: serviceTypes,
                QueuesList: this.props.QueuesList, selectedQ: this.props.selectedQ
            };
            this.props.history.push("/branch", { input: input });
        }
    }

    update(editItem) {
        console.log(editItem);
        http.put('api/Branches', editItem, null);

    }

    filter() {
        var filter = this.state.filter;
        var filteredBranches = this.props.branches.branchesList.branchesList;
        if (!filter.showArchived)
            filteredBranches = this.props.branches.branchesList.filter(b => !b.isDeleted);
        this.setState({ filteredBranches: filteredBranches });
        return filteredBranches;


    }

    filterAll(e) {
        var filteredBranches = this.filter();
        if (e.target.value !== "") {
             filteredBranches = filteredBranches.filter(
                b => {
                    var address = (b.street.name ? b.street.name + " " : "") + (b.houseNumber ? b.houseNumber.toString() + " " : "") +
                        (b.street.city ? b.street.city.name + " " : "");
                    console.log(address);
                    return b.id && b.id.toString() && b.id.toString().indexOf(e.target.value) > -1 ||
                        (b.name && b.name.includes(e.target.value)) ||
                        (address.trim().includes(e.target.value)) ||
                        (b.phonNumber && b.phonNumber.toString() && b.phonNumber.toString().includes(e.target.value)) ||
                        (b.emailAddress && b.emailAddress.includes(e.target.value));
                }
            );
            this.setState({ filteredBranches: filteredBranches });
        }
    }

    filterButtonOnClick(e) {
        var filter = this.state.filter;
        filter.showArchived = $('#showArchivedCheckbox').is(':checked');
        this.setState({ filter: filter });

        this.filter();
    }

    newBranch() {
        var newBranch = { name: "", id: 0, organizationId: this.user.organizationId, streetId: 0, street: { id: 0, cityId: 0, city: "" }, houseNumber: "", phonNumber: "", emailAddress: "", isDeleted: false }
        return newBranch;
    }



    renderBranchescards() {
        var t = this.props.t;
        var branches = this.props.branches.branchesList;
        return branches.map((item) => {
            var b = item;
            return item.id ? <div className={" rounded-0 border-white btn btn-default col-lg-3 col-md-5 col-sm-10 m-1 bg-white "} key={item.id}>
                <div className=" card-body ">

                    <h5 className={"card-subtitle  text-muted text-center fs-16 font-pr basicColorBlue"}>{b ? b.name : ""}</h5>
                    <br></br>
                    <h6 className="card-subtitle mb-2 text-muted">{(b.street && b.street.city) ? b.street.city.name : ""}</h6>
                    <p className="card-text">{b.street ? (b.street.name + " " + b.houseNumber) : ""} </p>
                </div>
            </div> : " ";
        });
    }

    renderNav() {

        var t = this.props.t;
        return (<div className="row m-0 p-0">
                <div className="col-3">
                    <span className="icon p-0 navbar-toggler pe-7s-filter" data-toggle="collapse" data-target="#navbarToggleExternalContentFilter" aria-controls="navbarToggleExternalContentFilter" aria-expanded="false" aria-label="Toggle navigation"></span>
                <SearchTemplate t={this.props.t} click={e => this.openBranchModal("#editBranch0")} change={e => this.filterAll(e)} placeholdersearch={t("Branche.Search")} titelnUser={t("Branche.Newbranch")}></SearchTemplate>
                <div className="collapse item-a" id="navbarToggleExternalContentFilter">
                    <div className="bg-light p-4">                        <span id="showArchivedSpan">{t("Branche.noBranchPlay")}</span>
                        <input id="showArchivedCheckbox" type="checkbox" unChecked />
                        <button id="filterButton" onClick={e => this.filterButtonOnClick(e)}>{t("Branche.Filter")}</button>
                    </div>
                </div>

                <div>
                    {this.state.modalData && this.state.showModal ? <Modal item={this.state.newBranch} saveEvent={this.saveEvent} updateEvent={this.updateEvent} modalId={"editBranch0"} showButton={false} formInputsData={this.state.modalData}></Modal> : ""}
                </div>





                </div>

            </div>);
    }

    render() {
        let contents =
            this.props.branches.branchesList.length && this.state.streets && this.state.streets.length && this.state.cities && this.state.cities.length ? this.renderBranchescards() : "";

        return (
            <div>
                {this.state.streets && this.state.streets.length && this.state.cities && this.state.cities.length ? this.renderNav() : ""}
                {contents}

            </div>
        );
    }
}
export default withI18n()(observer(Branches));








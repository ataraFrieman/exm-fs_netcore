import React, { Component } from 'react';
import history from '../helpers/History';
import * as http from '../helpers/Http';
import { getOrganizationId } from '../helpers/AccountService';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Modal } from './Modal';
import { getUser } from '../helpers/User'
import * as $ from 'jquery';
import { observer } from 'mobx-react';
import { RadioGroup, Radio } from 'react-radio-group';
import { PropagateLoader } from 'react-spinners';
import '../css/users.css'
import { withI18n } from "react-i18next";
import SearchTemplate from './SearchTamplate';


class Users extends Component {

    displayName = "Uesrs"

    constructor(props) {
        super(props);
        this.state = {
            usersList: [],
            filteredUsersList: [],
            filter: { showArchived: false },
            newUser: null,
            modalData: null,
            type: props.type ? props.type : 1,
            isLoading: false,
            dispalyModal: false
        }
        this.updateUser = this.updateUser.bind(this);
        this.insertUser = this.insertUser.bind(this);
        this.newUser = this.newUser.bind(this);
        this.getEditModalData = this.getEditModalData.bind(this);
        this.saveEvent = this.saveEvent.bind(this);
        this.updateEvent = this.updateEvent.bind(this);
        this.showModal = this.showModal.bind(this);
        this.openSP = this.openSP.bind(this);
        this.onDelete = this.onDelete.bind(this);
        this.onDeleteSP = this.onDeleteSP.bind(this);
        this.filter = this.filter.bind(this);
        this.filterButtonOnClick = this.filterButtonOnClick.bind(this);
        this.filterAll = this.filterAll.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.renderUsers = this.renderUsers.bind(this);
        this.hideModal = this.hideModal.bind(this);
        this.user = getUser();
    }

    componentWillMount() {

        this.setState({
            newUser: this.newUser(),
            modalData: this.getEditModalData(this.newUser())
        });
        if (this.props.type == 1 && this.props.serviceProviders && this.props.serviceProviders.length) {
            this.setState({
                type: 1,
                usersList: this.props.serviceProviders,
                filteredUsersList: this.props.serviceProviders.filter(u => !u.isDeleted)
            });
        }
        else
            this.loadUsers();
    }

    handleChange = (e) => {
        this.setState({ isLoading: true });
        this.setState({
            type: e,
            isLoading: false
        });
    }

    loadUsers() {
        
        var organizationId = getOrganizationId();
        if (!organizationId) {
            {
                this.setState({ noOrganizationId: true });
                return;
            }
        }
        var thisObj = this;
        http.get('api/Users/GetUsersToOrganization/' + organizationId)
            .then(function (response) {
                console.log(response);
                if (!response || !response.length)
                    return;
                thisObj.setState({
                    usersList: response,
                    filteredUsersList: response.filter(u => !u.isDeleted)
                });
            })
    }

    filter() {
        var filter = this.state.filter;
        var filteredUsersList = this.state.usersList;
        if (!filter.showArchived)
            filteredUsersList = this.state.usersList.filter(u => !u.isDeleted);
        this.setState({ filteredUsersList: filteredUsersList });
        return filteredUsersList;
    }

    filterButtonOnClick(e) {
        var filter = this.state.filter;
        filter.showArchived = $('#showArchivedCheckbox').is(':checked');
        this.setState({ filter: filter });

        this.filter();
    }

    filterAll(e) {
        var filteredUsersList = this.filter();
        if (e.target.value != "") {
            var filteredUsersList = filteredUsersList.filter(
                u => {
                    return u.identityNumber && u.identityNumber.toString() && u.identityNumber.toString().indexOf(e.target.value) > -1 ||
                        (u.userType.description && u.userType.description.includes(e.target.value)) ||
                        (u.userName && u.userName.includes(e.target.value)) ||
                        (u.titledFullName && u.titledFullName.includes(e.target.value));
                }
            );
            this.setState({ filteredUsersList: filteredUsersList });
        }
    }

    updateUser(item) {
        if (!item)
            return;
        var user = item;

        var thisObj = this;

        console.log(item);

        let request = {
            Entity: item,
        }
        http.put('api/Users/', request)
            .then(function (response) {
                console.log(response);
                if (!response || !response.entity || !response.entity.id)
                    return;
                var oldList = thisObj.state.usersList;
                var index = oldList.findIndex(i => i.id == response.entity.id);
                if (index >= 0)
                    oldList[index] = response.entity;
                console.log(oldList);
                thisObj.setState({ usersList: oldList });
                thisObj.filter();
                thisObj.hideModal();
            });

    }

    insertUser(item) {
        var thisObj = this;
        let request = {
            Entity: item,
        }

        http.post('api/Users/', request, null).then(data => {
            item = data.entity;
            item.userType = { id: 2, description: "משתמש ארגוני" };
            var usersList = thisObj.state.usersList;
            usersList.push(item);
            thisObj.setState({ usersList: usersList });
            thisObj.filter();
            //var e = { target: { value: $('#filterAll').val() } };
            //this.filterAll(e);
        });
        this.setState({
            newUser: this.newUser(),
            modalData: this.getEditModalData(this.newUser())
        });

    }

    showModal(user) {
        this.setState({
            newUser: user,
            modalData: this.getEditModalData(user),
            dispalyModal: true
        });
    }

    hideModal() {
        this.setState({ dispalyModal: false });
    }

    openSP(sp) {
        var input = { serviceProvider: sp }
        history.push("/serviceProvider", input,this.props.t);
        window.location.reload();
    }

    getEditModalData(item) {
        var t=this.props.t
        return [{
            cols: ["col-md-6", "col-md-6"]
            , properties:
                [
                    {
                        label: t("Users.firstName"),
                        type: "text",
                        bsclass: "form-control",
                        placeholder:t("Users.firstName"),
                        name: "firstName",
                        mandatory: "true",
                        value: item ? item.firstName : ""
                    },
                    {
                        label:t("Users.lastName"),
                        type: "text",
                        bsclass: "form-control",
                        placeholder:t("Users.lastName"),
                        name: "lastName",
                        mandatory: "true",
                        value: item ? item.lastName : ""
                    }
                ]
        },
        {
            cols: ["col-md-6", "col-md-6"]
            , properties:
                [
                    {
                        label:t("Users.id"),
                        type: "text",
                        bsclass: "form-control",
                        placeholder:t("Users.id"),
                        name: "identityNumber",
                        type: "identityNumber",
                        disabled: item.id == 0 ? false : true,
                        mandatory: "true",
                        value: item ? item.identityNumber : ""
                    },
                    {
                        label:t("Users.phoneNumber"),
                        type: "text",
                        bsclass: "form-control",
                        placeholder:t("Users.phoneNumber"),
                        type: "phone",
                        name: "userName",
                        mandatory: "true",
                        value: item ? item.userName : ""
                    }
                ]
        }];
    }

    saveEvent(e, item) {
        var thisObj = this;
        if (this.state.type == 1) {
            this.props.serviceProviders.addServiceProvider(item)
            thisObj.hideModal();
            
        }
        else {

            if (item.id > 0)
                this.updateUser(item);
            else {
                this.hideModal();
                this.insertUser(item);
            }
        }
    }

    updateEvent(newUser) {
        console.log(newUser);

        this.setState({
            newUser: newUser,
            modalData: this.getEditModalData(newUser)
        });
    }

    newUser() {
        var newUser = { firstName: "", id: 0, organizationId: this.user.organizationId, userName: "", identityNumber: "", lastName: "", isDeleted: false, userTypeId: 2 }
        return newUser;
    }

    onDeleteSP(SP) {
        //var thisObj = this;
        if (this.state.type == 1) {
            this.props.serviceProviders.deleteServiceProvider(SP);
        }
    }

    onDelete(user) {
        var thisObj = this;
        const data = {
            Entity: user,
        }
        http.deleteX('api/Users', data, null)
            .then(response => {
                console.log(response);
                var oldList = thisObj.state.usersList;
                var index = oldList.findIndex(i => i.id == user.id);
                if (index >= 0)
                    oldList[index].isDeleted = true;
                thisObj.setState({ usersList: oldList });
                thisObj.filter();
            });
    }

    renderUsers() {
        var t=this.props.t;
        return this.state.filteredUsersList.map((item) => {
            var user = item;
            return user.id ? <div className={" rounded-0 border-white btn btn-default col-lg-3 col-md-5 col-sm-10 m-1 bg-white "} key={user.id}>
                <div className=" card-body ">
                    <div className="row justify-content-end">
                        <div className="" data-toggle="modal" onClick={e => this.showModal(JSON.parse(JSON.stringify(user)))} >
                            <i id="open" className={"icon fs-24 pe-7s-note"} data-toggle="tooltip" title={t("Users.open")}/>
                        </div>
                        {/* "delete"  */}
                        {user.isDeleted ? "" : <i id="delete" className={"icon fs-24 pe-7s-trash"} onClick={e => this.onDelete(user)} data-toggle="tooltip" title={t("Users.delete")}/>}
                    </div>
                    <h6 className={"card-subtitle  text-muted text-center font-pr fs-16 " + (user.isDeleted ? "text-secondary" : " basicColorBlue")}>{user ? user.titledFullName : ""}</h6>
                    <h6 className="card-subtitle  text-muted p-0 text-center m-1">{user.userType ? user.userType.description : ""}</h6>
                    <div className="row">
                        <div className=" col-2 p-0 ">
                            <i className="fs-24 pe-7s-user" />
                        </div>
                        <span className=" col-10 p-0 text-right text-muted fs-12">{user.identityNumber}</span>
                    </div>
                    <div className="row">
                        <div className=" col-2 p-0 ">
                            <i className="fs-24 pe-7s-phone" />
                        </div>
                        <span className=" col-10 p-0 text-right text-muted fs-12">{user.userName}</span>
                    </div>
                </div>
            </div> : " ";
        });

    }

    renderNav() {

        var t=this.props.t;
        
        return (
            <div className="grid-user ">

                <div>
                    {/*<span className="icon p-0 navbar-toggler pe-7s-filter" data-toggle="collapse" data-target="#navbarToggleExternalContentFilter" aria-controls="navbarToggleExternalContentFilter" aria-expanded="false" aria-label="Toggle navigation">                        
                     * </span>*/}
                     <SearchTemplate t={this.props.t} click={e =>this.showModal(this.newUser())} change={e => this.filterAll(e)} placeholdersearch={t("Users.SortByName")} titelnUser={t("Users.newUser")}></SearchTemplate>
                    {/* <span className="" data-toggle="modal" onClick={e => this.showModal(this.newUser())}>
                        <i id="open" className={"icon fs-24 pe-7s-plus p-0"} data-toggle="tooltip" title={t("Users.newUser")} />
                    </span> */}

                </div>
                <div>
                    {this.state.dispalyModal ? <Modal t={this.props.t} hideModal={this.hideModal} item={this.state.newUser} saveEvent={this.saveEvent} updateEvent={this.updateEvent} modalId={"editUser"} showButton={false} formInputsData={this.state.modalData}></Modal> : ""}
                    {/* <input type="text" id="filterAll" onChange={e => this.filterAll(e)} placeholder={t("Users.SortByName")} /> */}
                </div>



                {/*<div className="collapse item-a" id="navbarToggleExternalContentFilter">
                    <div className="bg-light p-4">
                        <span id="showArchivedSpan">{t("Users.ShowInactiveUsers")}</span>
                        <input id="showArchivedCheckbox" type="checkbox" unchecked={"true"}></input>
                        <button id="filterButton" onClick={e => this.filterButtonOnClick(e)}>{t("Users.filter")}</button>
                    </div>
                </div>*/}

            </div>)
    }
g=()=>
{
    console.log("=========================================================")
    console.log(this.props.serviceProviders)

}
    render() {
        this.g()
        var t=this.props.t;
        return <div className="">
            {this.renderNav()}
            {this.state.type == 1 ?
                <div>
                    {(!this.props.serviceProviders.serviceProviders || this.state.isLoading) ?
                        (<div className="ProgressStyle ">
                            <PropagateLoader color={'#3c519d'} />
                        </div>) :
                        (<div>
                            {this.props.serviceProviders.serviceProviders.map((sp ,index)=> {
                                return (!sp.isDeleted?< div className={"rounded-0 border-white btn btn-default col-lg-3 col-md-5 col-sm-10 m-1 bg-white "} key={index} >
                                    <div className="card-body">
                                        <div className="row justify-content-end">
                                            <div className="" data-toggle="modal" onClick={e => this.openSP(JSON.parse(JSON.stringify(sp)))} >
                                                <i id="open" className={"icon fs-24 pe-7s-note"} data-toggle="tooltip" title={t("Users.open")} />
                                            </div>
                                            {sp.isDeleted ? "" : <i id="delete" className={"icon fs-24 pe-7s-trash"} onClick={e => this.onDeleteSP(sp)} data-toggle="tooltip" title={t("Users.delete")} />}
                                        </div>
                                        <h6 className={"card-subtitle   text-muted text-center fs-16 font-pr" + (sp.isDeleted ? "text-secondary" : " basicColorBlue")}>{sp ? sp.titledFullName : ""}</h6>
                                        <h6 className="card-subtitle  text-muted p-0 text-center m-1">{!sp.isDeleted?"רופא":""}</h6>
                                    </div>
                                </div>:"");
                            }
                            )
                            }
                        </div>)

                    }
                </div> :
                this.renderUsers()}
        </div>;
    }
}
export default withI18n()(observer(Users));
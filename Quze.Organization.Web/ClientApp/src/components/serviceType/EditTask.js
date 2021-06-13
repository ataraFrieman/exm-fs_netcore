import React from 'react';
import { Tasks } from '../task/Tasks';
import * as http from '../../helpers/Http';
import { counter, config } from '@fortawesome/fontawesome-svg-core';
import EditAlert from './EditAlert';
import AddAlert from './AddAlert';
import * as $ from "jquery";
import "../../css/general.css"
import { Modal } from 'react-bootstrap'
import { Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { withI18n } from 'react-i18next';

class EditTask extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            task: this.props.task,
            selectedTab: props.tab,
            isRequired: true,
            numChildren: 0,
            description: this.props.task.description,
            code: this.props.task.code,
            show: false,
            alerts: [],
        };
        this.counter = 0;
        this.myDivId = 'div1';
        this.myDeleteButtonId = 'btn1';
        this.chaneged = false;
        this.handleShow = this.handleShow.bind(this);
        this.handleClose = this.handleClose.bind(this);
    }

    componentDidMount = () => {
        this.setState({
            alerts: this.props.task.alertRules,
        })
    }

    setChange = () => {
        this.chaneged = true;
    }

    reloadAlerts = () => {
        http.get('api/AlertRules/GetAlertByTaskId?taskId=' + this.state.task.id)
            .then(res => {
                console.log(res);
                this.setState({ alerts: res.entities })
            })
    }

    handleSubmit = (event) => {
        var t = this.props.t;
        event.preventDefault();
        var alerts = [];
        this.counter = 0;
        //Code 0:The user didn't fill all the required fields. Code 1, the alert was not emited.
        for (var refIndex = 0, arrayIndex = 0; refIndex < this.state.alerts.length; refIndex++) {
            console.log(this.refs["alert" + refIndex])
            if (!this.refs["alert" + refIndex].isDeleted) {
                alerts[arrayIndex] = this.refs["alert" + refIndex].sendData();
                if (alerts[arrayIndex] == 0) {
                    alert(t("AddT.alertForget"));
                    return
                }
                else if (alerts[arrayIndex] != 1) {
                    alerts[arrayIndex].RequiredTaskID = this.state.task.id;
                    arrayIndex++;
                }
            }
        }

        if (alerts.length != 0) {
            let alertRequest = {
                Entities: alerts,
            }
            http.put('api/AlertRules', alertRequest, null)
                .then(() => this.reloadAlerts());
        }

        if (this.chaneged) {
            let data = {
                id: this.state.task.id,
                Description: this.state.description,
                IsRequired: this.state.isRequired,
                ServiceTypeID: this.state.task.serviceTypeID,
            }

            let request = {
                Entity: data,
            }

            http.put('api/RequiredTasks', request, null)
                .then(() => this.props.reloadData());
        }

        var numChildren = this.state.numChildren;
        var t = this.props.t;
        if (this.state.numChildren != 0) {
            this.counter = 0;
            var newAlerts = [];
            for (var refIndex = 0, arrayIndex = 0; refIndex < numChildren; refIndex++) {
                var newAlert = this.refs["newAlert" + refIndex]
                console.log(newAlert);
                if (!newAlert.isDeleted) {
                    newAlerts[arrayIndex] = newAlert.sendData();
                    if (newAlerts[arrayIndex] == false) {
                        alert(t("AddT.alertForget"));
                        return
                    }
                    newAlerts[arrayIndex].RequiredTaskID = this.state.task.id;
                    arrayIndex++;
                }
            }

            if (newAlerts.length > 0) {
                let requestNewAlerts = {
                    Entities: newAlerts,
                }
                http.post('api/AlertRules', requestNewAlerts, null)
                    .then(() => this.reloadAlerts());
            }
        }

        this.setState({ show: false, numChildren: 0, });
    }

    getAlerts = () => {
        http.get()
    }

    handleCancel = () => {
        this.setState({
            description: "",
            numChildren: 0,
            code: "",
        })
    }

    handleClose() {
        this.setState({ show: false, numChildren: 0, });

    }

    handleShow() {
        this.setState({ show: true });
    }

    handleInputChange = (event) => {

        this.setState({
            description: event.target.value
        });
        this.chaneged = true;
    }

    handleCodeChange = (event) => {
        this.setState({
            code: event.target.value
        });
        this.chaneged = true;
    }

    add = () => {
        this.setState({
            numChildren: this.state.numChildren + 1,
        })
    }


    // renderFellow=() => {
    //     var t=this.props.t;
    //     var fellows = this.props.fellows.FellowsList;
    //     return fellows.map((item) => {
    //         var item = item;
    //         return item.id ? <div className={" rounded-0 border-white btn btn-default col-lg-3 col-md-5 col-sm-10 m-1 bg-white "} key={item.id}>
    //             <div className=" card-body ">
                // <div className="row justify-content-end">
                //     <div className="" data-toggle="modal" onClick={(evt) => { this.editFellow(item); }} >
                //         <i id="open" className={"icon fs-24 pe-7s-note"} data-toggle="tooltip" title={t("Users.open")} />
                //     </div>
                //      <i id="delete" className={"icon fs-24 pe-7s-trash"} onClick={(evt) => { this.deleteFelow(item); }} data-toggle="tooltip" title={t("Users.delete")} />
                // </div>

    //             <h6 className={"card-subtitle  text-muted text-center fs-16 basicColorBlue"}>{item ? item.firstName + " " + item.lastName : ""}</h6>
    //             <br></br>
    //             <div className="row">
    //                 <div className=" col-2 p-0 ">
    //                     <i className="fs-24 pe-7s-user" />
    //                 </div>
    //                 <span className=" col-10 p-0 text-right text-muted fs-12">{item.identityNumber}</span>
    //             </div>
    //         </div>
    //         </div> : " ";
    //     });
    // }
    render() {
        var t = this.props.t;
        const children = [];
        var arr = this.state.alerts;
        if (!(arr == undefined || arr.length < 1)) {
            arr.map((l, index) => {
                children.push(<span className="p-2" key={index + 0.5}>
                    <EditAlert key={index}
                        number={index + 0.5}
                        ref={"alert" + index}
                        serviceTypeID={this.state.task.ServiceTypeID}
                        serviceName={this.state.task.serviceName}
                        isNew={true}
                        alert={l}
                        t={this.props.t}
                    /></span>)
            }
            )
        }

        for (var i = 0; i < this.state.numChildren; i += 1) {

            children.push(<span className="p-2" key={i}><AddAlert key={i}
                number={i + this.state.task.alertRules.length}
                ref={"newAlert" + i}
                serviceTypeID={this.props.ServiceTypeID}
                serviceName={this.props.serviceName}
                isNew={true}
                t={this.props.t} /></span>);

        };

        var alertNum = this.props.task.alertRules.length != 0 ? <span className="p-2 text-danger" >
            <FontAwesomeIcon
                icon="bell"
                className="basicColorRed"
            /><small>{this.state.alerts.length}</small>
        </span> : "";

        return (
            <span>
                <div className={" rounded-0 border-white btn btn-default col-lg-3 col-md-5 col-sm-10 m-1 bg-white "}
                    key={this.props.task.id}
                    data-toggle="modal"
                >
                    <div className=" card-body ">

                    <div className="row justify-content-end">
                    <div className="" data-toggle="modal" onClick={(evt) => { this.handleShow(); }} >
                        <i id="open" className={"icon fs-24 pe-7s-note"} data-toggle="tooltip" title={t("Users.open")} />
                    </div>
                     <i id="delete" className={"icon fs-24 pe-7s-trash"} onClick={(e) => { e.stopPropagation(); this.props.delete(this.props.task.id); }} data-toggle="tooltip" title={t("Users.delete")} />
                </div>
                        <br />
                        <h6 className={"card-subtitle  text-muted text-center fs-16 basicColorBlue font-pr "}>
                            {this.props.task ? this.props.task.description : ""}</h6>
                    </div>
                </div>

                <Modal
                    size="lg"
                    show={this.state.show}
                    onHide={this.handleClose}
                    aria-labelledby="contained-modal-title-vcenter"
                >
                    <Modal.Header closeButton>
                        <Modal.Title>{t("EditT.editDirective")}</Modal.Title>
                    </Modal.Header>
                    <Modal.Body className="text-align" style={{ 'maxHeight': 'calc(100vh - 210px)', 'overflowY': 'auto' }}>

                        <span >
                            <label >{t("EditT.SetTheTask")}</label>
                            <input type="text"
                                className="form-control textInput modal-input text-align"
                                value={this.state.description}
                                onChange={e => { this.setState({ description: e.target.value }); this.chaneged = true; }}
                            ></input>
                        </span>
                        <br />
                        <span >
                            <label className="align-hebrew">{t("EditT.YouCanAddCodeToTheTask")}</label>
                            <input type="text"
                                className="form-control textInput modal-input text-align"
                                value={this.state.code != null ? this.state.code : ""}
                                onChange={this.handleCodeChange}
                            >
                            </input>

                        </span>

                        <span className="center-elemnt">
                            <button type="button" className="btn btn-danger m m-3" onClick={this.add}>{t("EditT.addAlert")} </button>
                            <br />
                        </span>
                        {children}
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={this.handleClose}>
                            {t("EditT.close")}
                        </Button>
                        <Button variant="primary" onClick={this.handleSubmit}>
                            {t("EditT.save")}
                        </Button>
                    </Modal.Footer>
                </Modal>
            </span>

        );
    }

}

export default withI18n()(EditTask)
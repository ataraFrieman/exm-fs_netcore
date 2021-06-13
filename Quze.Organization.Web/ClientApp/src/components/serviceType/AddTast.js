import React from 'react';
import { Tasks } from '../task/Tasks';
import * as http from '../../helpers/Http';
import { counter } from '@fortawesome/fontawesome-svg-core';
import { string } from 'prop-types';
import AddAlert from './AddAlert';
import * as $ from "jquery";
import "../../css/general.css"
import { Modal } from 'react-bootstrap'
import { Button } from 'react-bootstrap'
import { withI18n } from 'react-i18next';
// import SearchTemplate from '../SearchTamplate';
import '../../css/SearchTemplate.css';


class AddTast extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            service: props.serviceType,
            name: props.serviceName,
            selectedTab: props.tab,
            isRequired: true,
            numChildren: 0,
            description: "",
            code: "",
            lgShow: false,
        };
        this.counter = 0;
        this.myDivId = 'div1';
        this.myDeleteButtonId = 'btn1';

    }

    handleSubmit = (event) => {
        event.preventDefault();

        var alerts = [];
        var t = this.props.t;
        this.counter = 0;
        for (var refIndex = 0, arrayIndex = 0; refIndex < this.state.numChildren; refIndex++) {

            if (!this.refs["alert" + refIndex].isDeleted) {
                alerts[arrayIndex] = this.refs["alert" + refIndex].sendData();
                if (alerts[arrayIndex] == false) {
                    alert(t("AddTast.alertForget"));
                    return
                }
                arrayIndex++;
            }
        }

        var id = this.props.serviceTypeID;

        let data = {
            Description: this.state.description,
            IsRequired: this.state.isRequired,
            ServiceTypeID: id,
            AlertRules: alerts
        }

        console.log(data);

        let request = {
            Entity: data,
        }

        console.log(request);
        http.post('api/RequiredTasks', request, null)
            .then(res => {
                console.log(res);
            }).then(() => this.props.getTasks())
            .then(this.handleCancel);

        this.setState({ lgShow: false, });
    }

    handleCancel = () => {
        this.setState({
            description: "",
            numChildren: 0,
            code: "",
            lgShow: false,
        })
    }

    handleInputChange = (event) => {
        //console.log(event.target.value);
        const target = event.target;

        this.setState({
            description: event.target.value
        });

    }

    handleCodeChange = (event) => {
        this.setState({
            code: event.target.value
        });

    }

    add = () => {
        this.setState({
            numChildren: this.state.numChildren + 1,
        })
    }
    showLg = () => {
        this.setState({ lgShow: true });
    }

    render() {
        var t = this.props.t;
        const children = [];
        var t = this.props.t;

        for (var i = 0; i < this.state.numChildren; i += 1) {

            children.push(<span className="p-2" key={i}><AddAlert t={this.props.t} key={i}
                number={i}
                ref={"alert" + i}
                serviceTypeID={this.props.ServiceTypeID}
                serviceName={this.props.serviceName}
                isNew={true} /></span>
            );
        };



        return (
            <span >
                <div className="container">
                    <div class="row">
                        <div className="col-md-1 searcheePluse text-primary m-1 p-0" onClick={() => this.showLg()} title={(t("AddT.addNewDirective"))}>
                            <i className="pe-7s-plus fs-20 font-weight-bold basicColorRed"></i>
                        </div>
                    </div>
                </div>
                <Modal
                    open={this.state.lgShow}
                    onHide={this.handleCancel}
                    aria-labelledby="example-modal-sizes-title-lg"
                >
                    <Modal.Header >
                        <Modal.Title id="example-modal-sizes-title-lg">
                            {this.props.serviceName}
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body className='text-align' style={{ 'maxHeight': 'calc(100vh - 210px)', 'overflowY': 'auto' }}>
                        <span >
                            <label >
                                {(t("AddT.SetTheTask"))}
                            </label>
                            <input type="text"
                                className="form-control modal-input"
                                value={this.state.description}
                                onChange={this.handleInputChange}
                            ></input>
                        </span>
                        <br />
                        <span >
                            <label >
                                {(t("AddT.YouCanAddCodeToTheTask"))}
                            </label>
                            <input type="text"
                                className="form-control modal-input"
                                value={this.state.code}
                                onChange={this.handleCodeChange}
                            >
                            </input>

                        </span>
                        <span className="center-elemnt">
                            <button type="button" className="btn btn-danger m m-3" onClick={this.add}>
                                {(t("AddT.AddAlert"))}
                            </button>
                        </span>
                        {children}
                        <br />
                    </Modal.Body>
                    {/* <Modal.Footer> */}
                    <div className="modal-footer text-alignsss">
                        <Button variant="secondary" onClick={() => this.setState({ lgShow: false })}>
                            {(t("AddT.close"))}
                        </Button>
                        <Button variant="primary" onClick={this.handleSubmit}>
                            {(t("AddT.save"))}
                        </Button>
                    </div>
                    {/* </Modal.Footer> */}
                </Modal>
            </span >

        );
    }

}

export default AddTast
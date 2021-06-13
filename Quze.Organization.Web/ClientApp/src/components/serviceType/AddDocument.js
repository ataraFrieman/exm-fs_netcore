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
import SearchTemplate from '../SearchTamplate';


class AddDocument extends React.Component {

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
         var t=this.props.t;
        var alerts = [];

        this.counter = 0;
        for (var refIndex = 0, arrayIndex = 0; refIndex < this.state.numChildren; refIndex++) {
            if (!this.refs["alert" + refIndex].isDeleted) {
                alerts[arrayIndex] = this.refs["alert" + refIndex].sendData();
                if (alerts[arrayIndex] == false) {
                    alert(t("AddT.alertForget"));
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
        http.post('api/RequiredDocuments', request, null)
            .then(res => {
                console.log(res);
            }).then(() => this.props.getDocuments())
            .then(this.handleCancel);

        this.setState({ lgShow: false, });
    }

    handleCancel = () => {
        console.log("cancel");
        this.setState({
            description: "",
            numChildren: 0,
            code: "",
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
    showLg=()=>
    {
        this.setState({ lgShow: true });
    }

    render() {
        var t = this.props.t;

        const children = [];

        for (var i = 0; i < this.state.numChildren; i += 1) {
            children.push(<span className="p-2" key={i}><AddAlert key={i}
                number={i}
                ref={"alert" + i}
                serviceTypeID={this.props.ServiceTypeID}
                serviceName={this.props.serviceName}
                isNew={true}
                t={this.props.t} /></span>);
        };

        let lgClose = () => this.setState({ lgShow: false });

        return (
            <span >
              <div className="container">
                    <div class="row">
                        <div className="col-md-1 searcheePluse text-primary m-1 p-0" onClick={()=>this.showLg()} title={t("AddDoc.newRequirement")}>
                            <i className="pe-7s-plus fs-20 font-weight-bold basicColorRed"></i>
                        </div>
                    </div>
                </div>
                
                <Modal
                    show={this.state.lgShow}
                    onHide={lgClose}
                    aria-labelledby="example-modal-sizes-title-lg"
                    className='text-align'
                >
                    <Modal.Header className='text-align'>
                        <Modal.Title id="example-modal-sizes-title-lg">
                            {this.props.serviceName}
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body className='text-align' style={{ 'maxHeight': 'calc(100vh - 210px)', 'overflowY': 'auto' }}>
                        <span >
                            <label>{t("AddDoc.documentName")}</label>
                            <input type="text"
                                className="form-control  modal-input"
                                value={this.state.description}
                                onChange={this.handleInputChange}
                            ></input>
                        </span>
                        <br/>
                        <span >
                            <label > {t("AddDoc.youCanAddCode")}</label>
                            <input type="text"
                                className="form-control modal-input"
                                value={this.state.code}
                                onChange={this.handleCodeChange}
                            >
                            </input>
                        </span>
                        <span className="center-elemnt">
                        <button type="button" className="btn btn-danger m m-3" onClick={this.add}>{t("EditT.addAlert")}</button>
                        </span>
                        {children}

                        <div id="lines" className="container" ref="addLines">
                            <br />
                        </div>
                    </Modal.Body>
                    <Modal.Footer className='text-align'>
                        <Button variant="secondary" onClick={() => this.setState({ lgShow: false })}>
                        {t("AddDoc.close")}
                        </Button>
                        <Button variant="primary" onClick={this.handleSubmit}>
                        {t("AddDoc.save")}
                        </Button>
                    </Modal.Footer>
                </Modal>
            </span >

        );
    }

}

export default withI18n()((AddDocument));
import React, { Component } from 'react';
import "../../css/site.css"
import "../../css/general.css"
import EditDocument from './EditDocument';
import * as http from '../../helpers/Http';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { withI18n } from 'react-i18next';



export class ViewReqiuredDocuments extends Component {
    constructor(props) {
        super(props);
        this.state = {
            serviceTypeId: 0,
            documentskList: []
        };
    }

    componentDidMount() {
        this.setState({
            serviceTypeId: this.props.serviceTypeID,
            documentskList: this.props.list
        })
    }

    click = (RequiredDocumentId) => {
        const data = {
            EntityId: RequiredDocumentId,
        }

        http.deleteX('api/RequiredDocuments', data, null)
            .then(response => {
                if (!response || !response.length)
                    return;
                this.setState({
                    scrolledList: response,
                    loading: false,
                })
            }).then(() => console.log(this.state));
    }

    render = () => {
        var t = this.props.t;
        var list = this.props.loadingdocumentskList == false ? this.props.list.map(
            (line,index) => <EditDocument
                    key={index}
                    task={line}
                    reloadData={this.props.getDocument}
                    type="doc"
                    delete={this.click}
                    serviceName={this.props.serviceName}
                    t={this.props.t}
                />
            
        ) : <li> {t("vDoc.notLoading")}</li>;


        return <span> {list}</span>
    }
};

export default withI18n()(ViewReqiuredDocuments);

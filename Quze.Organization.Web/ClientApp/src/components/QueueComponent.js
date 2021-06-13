import React, { Component } from 'react';
import { Grid, Row, Col, Table, Tab, Tabs } from "react-bootstrap";
import Card from "./Card";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Selectable } from './Calendar';
// import { ServiceTypesCards } from './ServiceType';
import { ServiceQGraphs } from './ServiceQGraphs';
import * as http from '../helpers/Http';
import history from '../helpers/History';
import { QueuesList } from './QueuesList';
import  ManageQ  from './ManageQ';
import { getOrganizationId } from '../helpers/AccountService';
import { observer } from 'mobx-react';
import { withI18n } from "react-i18next";

 class QueuesComponent extends Component {
    displayName = "QueuesComponent"
    constructor(props) {
        super(props);
        this.state = {
            QueuesList: this.props.QueuesList ? this.props.QueuesList : [],
            activeTabIndex:1, 
            selectedQ: this.props.selectedQ ? this.props.selectedQ : null
        };
        this.updateQueuesList = this.updateQueuesList.bind(this);
        this.showList = this.showList.bind(this);
    }
    handleClickTab(event, index, thisObj) {
        event.stopPropagation();
        event.nativeEvent.stopImmediatePropagation();
        thisObj.setState({ activeTabIndex: index })
    }

    openCurrentQ(item,index) {
        this.setState({ selectedQ: item, activeTabIndex: index })
    }
    updateQueuesList(newList) {
        this.setState({ QueuesList: newList });
    }
     componentWillMount() {
        var thisObj = this;
        var organizationId = getOrganizationId();
        if (!organizationId) {
            {
                this.setState({ noOrganizationId: true });
                return;
            }
         }
         if (!this.state.QueuesList || !this.state.QueuesList.length)
             this.setState({ QueuesList: this.props.Queues.queuesList });
    }

     showList() {
         this.setState({ selectedQ:null})}

     render() {
         var t = this.props.t; 
         return <div className=" p-0">
                

            <div  id="myTabContent" >
                    {!this.state.selectedQ  && this.state.QueuesList.length?
                        <div className="tab-pane fade active show"  >
                            
                         <QueuesList t={this.props.t} queuesList={this.state.QueuesList} openCurrentQ={this.openCurrentQ.bind(this)} Queues={this.props.Queues}/> 
                        </div> : ""}
                    { this.state.selectedQ ?
                        <div className="tab-pane fade active show">
                            
                         <ManageQ  t={this.props.t} selectedQ={this.state.selectedQ} queuesList={this.state.QueuesList} updateQueuesList={this.updateQueuesList} filterStore={this.props.filterStore} filtersList={this.props.filtersList} queues={this.props.Queues} showList={this.showList}/> 
                        </div> : ""}
                    {this.state.activeTabIndex === 3 && this.state.selectedQ ?
                        <div className="tab-pane fade active show" >
                            <ServiceQGraphs t={this.props.t} selectedQ={this.state.selectedQ} queuesList={this.state.QueuesList}  />
                        </div> : ""}
                </div>

            </div>;
    }
}
export default withI18n()(observer(QueuesComponent));
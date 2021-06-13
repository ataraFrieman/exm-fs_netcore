import React, { Component } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import Filtered from './Filtered/Filtered';
import * as $ from "jquery";
import { observer } from 'mobx-react';
import { withI18n } from "react-i18next";

//let btnsToDisplay;

class Filters extends Component {

    constructor(props) {
        super(props);
        this.btnsToDisplay = this.btnsToDisplay.bind(this);
        this.IsParentSelected = this.IsParentSelected.bind(this);
    }

    btnsToDisplay = () => this.props.filtersArray.filter(item =>
        item.value === '' && (item.parentId === '' || this.IsParentSelected(item) === true));

    IsParentSelected = item => {
        let parent = this.props.filtersArray.filter(r => r.id === item.parentId);
        // console.log(parent);
        // console.log(typeof(parent));
        return (parent[0].value !== '');
    }

    IsDisableButton(filterType) {
        let organizationFilter = this.props.filtersArray.filter(i => i.id == "organiztionSelect");
        let isOrganizationFilter = organizationFilter && organizationFilter.length && organizationFilter[0].value;
        let serviceTypeFilter = this.props.filtersArray.filter(i => i.id == "serviceTypeSelect");
        let isServiceTypeFilter = serviceTypeFilter && serviceTypeFilter.length;// && serviceTypeFilter[0].value    
        switch (filterType) {
            case "ארגון":
                return false;
            case "סוג שירות":
                return false;
            case "בין התאריכים":
                // return !(isOrganizationFilter && isServiceTypeFilter);
                return false;
            case "בין השעות":
                // !(isOrganizationFilter && isServiceTypeFilter);
                return false;
            case "שם רופא":
                //return !(isOrganizationFilter );
                return false;
            default:
                return false;
        }
    }
    getItemName = (key) => {

        var t = this.props.t;
        switch (key) {
            case "סניף":
                return t("search.branch");
            case "סוג שירות":
                return t("search.serviceType");
            case "בין התאריכים":
                return t("search.rangeofdates");
            case "בין השעות":
                return t("search.rangeofhours");
            case "שם רופא":
                return t("search.dotorname");
            default:
        }
    }

    getIconNameByKey(key) {
        console.log(key);
        switch (key) {
            case "ארגון":
            case "סניף":
                return "fs-24 pe-7s-way icon";
            case "קטגוריה":
            case "סוג שירות":
                return "fs-24 pe-7s-eyedropper icon";
            case "בין התאריכים":
                return "fs-24 pe-7s-date icon";
            case "בין השעות":
                return "fs-24 pe-7s-timer icon";
            case "שם רופא":
                return "fs-24 pe-7s-id icon";

            default:
        }

        return "user-circle";
    }

    render() {
        var t = this.props.t;
        var thisObj = this;
        return (
            <div className='row m-0 p-0 w-100 justify-content-center' id='filterByButtons'>
                {
                    thisObj.props.filtersArray.map(item => {
                        var filteredObj = this.props.filtersArray.filter(f => f.key == item.key);
                        return (
                            <div key={item.id} >
                                <div className="card m-2 p-0" key={item.id}>
                                    <div className="card-body p-2 text-center" >
                                        <button onClick={() => {
                                            $('.modal').modal('hide');
                                            this.props.onAddingFilter(item.id);
                                        }}
                                            id={item.id}
                                            className="btn btn-outline-secondary border-0 fc-gray"
                                            data-toggle="modal"
                                            data-target={"#searchModal" + item.id}
                                            disabled={this.IsDisableButton(item.key)}
                                        >

                                            <i className={this.getIconNameByKey(item.key)}></i>
                                            <span className="p-2 font-pr basicColorBlue">{this.getItemName(item.key)}</span>
                                        </button>
                                        <Filtered filtersArray={[item]}
                                            onCancelFilter={this.props.onCancelFilter} />

                                    </div>
                                </div>
                                <div className={"row w-100  m-0 " + (item.id != "dateRangeSelect" ? "mxw-150" : "dateRangeSelect")}>
                                    <div className="col-12 m-0 p-0"> {this.props.renderSearchModal(item)}</div>
                                </div>
                            </div>

                        );
                    })
                }

            </div>
        );
    }
}

export default withI18n()(observer(Filters));
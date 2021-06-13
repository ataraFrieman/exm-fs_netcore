import React, { Component } from 'react';
import SelectCategory from './SelectCategory';
import SelectProvider from './SelectDoctor';
import SelectFree from './SelectFree';
import Filters from '../Filters';
import Filtered from '../Filtered/Filtered';
import SelectArea from './SelectArea';
import SelectBranch from './SelectBranch';
import SelecctOrganiztion from './SelectOrganiztion';
import SelectServiceType from './SelectServiceType';
import SelectHours from './SelectHours';
import SelectDatesRange from './SelectDatesRange';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import * as $ from "jquery";
import { observer } from 'mobx-react';
import filterList from '../../stores/schedualFilters';
import { withI18n } from "react-i18next";


var filterBy = filterList.fromJS().filterBy;


class Search extends Component {

    constructor(props) {
        super(props);
        this.state = {
            
            selectedCategory:1,
        };
        this.onSelectFunc = this.onSelectFunc.bind(this);
        this.onAddingFilter = this.onAddingFilter.bind(this);
        this.onCancelFilter = this.onCancelFilter.bind(this);
        this.renderSearchModal = this.renderSearchModal.bind(this);
    }

    onAddingFilter = filterToAdd => {
        filterBy = filterToAdd;
        this.forceUpdate();
    }

    onCancelFilter = filterToDelete => {
        this.props.filtersArray.map(f => {
            if (f.id === filterToDelete) {
                f.value = '';
                f.valueId = '';
            }
            if (f.parentId === filterToDelete) {
                f.value = '';
                f.valueId = '';
            }
            return f;
        });
    };

    onSelectFunc(selectedValue, isCategory) {
        this.props.filtersArray.map(f => {
            if (f.id === filterBy || (isCategory && f.id == "categorySelect")) {
                f.valueId = selectedValue[0];
                f.value = selectedValue[1];
            }
            return f;
        });
        filterBy= '' ;
    };

    closeModal(id) {
        $('#' + id).modal('hide');
    }

    selectType = () => {
        if (filterBy === 'serviceProviderSelect') {
            return <SelectProvider
                serviceProviders={this.props.serviceProviders} closeModal={this.closeModal} filtersArray={this.props.filtersArray}
                onSelect={_valueSelected => this.onSelectFunc(_valueSelected)} />;                
        }
        if (filterBy == 'areaSelect') { return <SelectArea closeModal={this.closeModal} profixUrl={this.props.profixUrl} filtersArray={this.props.filtersArray} onSelect={_valueSelected => this.onSelectFunc(_valueSelected)} />; }




        if (filterBy == 'branchSelect') {
            return <SelectBranch
                branches={this.props.branches}
                selectedOrganiztion={5}
                closeModal={this.closeModal} profixUrl={this.props.profixUrl} filtersArray={this.props.filtersArray} onSelect={_valueSelected => this.onSelectFunc(_valueSelected)} />;
        }





        if (filterBy === 'organiztionSelect') { return <SelecctOrganiztion closeModal={this.closeModal} profixUrl={this.props.profixUrl} filtersArray={this.props.filtersArray} onSelect={_valueSelected => this.onSelectFunc(_valueSelected)} />; }
        if (filterBy === 'categorySelect') { return <SelectCategory closeModal={this.closeModal} profixUrl={this.props.profixUrl} filtersArray={this.props.filtersArray} onSelect={_valueSelected => this.onSelectFunc(_valueSelected)} />; }




//---------------------------------------------------------------------------------
        if (filterBy === 'serviceTypeSelect') {
            return <SelectServiceType
                serviceTypes={this.props.serviceTypes}
                serviceTypesList={this.props.serviceTypesList}
                selectedCategory={this.state.selectedCategory}
                closeModal={this.closeModal} profixUrl={this.props.profixUrl} filtersArray={this.props.filtersArray} onSelect={_valueSelected => this.onSelectFunc(_valueSelected)} />;
        }





        if (filterBy === 'timeRangeSelect') { return <SelectHours closeModal={this.closeModal} profixUrl={this.props.profixUrl} filtersArray={this.props.filtersArray} onSelect={_valueSelected => this.onSelectFunc(_valueSelected)} />; }
        if (filterBy === 'dateRangeSelect') { return <SelectDatesRange closeModal={this.closeModal} profixUrl={this.props.profixUrl} filtersArray={this.props.filtersArray} onSelect={_valueSelected => this.onSelectFunc(_valueSelected)} />; }


        if (filterBy === '') { return <SelectFree profixUrl={this.props.profixUrl} onSelect={_valueSelected => this.onSelectFunc(_valueSelected)} /> }

    };

    filterTitle() {
        var t=this.props.t;
        switch (filterBy) {
            case "organiztionSelect":
                return t("search.organization");
            case "branchSelect":
                return t("search.branch");
            case "serviceTypeSelect":
                return t("search.serviceType");
            case "dateRangeSelect":
                return t("search.rangeofdates") ;
            case "timeRangeSelect":
                return t("search.time");
            default:
                return "";
        }
    }

    renderSearchModal(filterItem) {
        var t=this.props.t;
        return <div className="modal p-0 m-0 fade searchModal" data-backdrop="false" id={"searchModal"+filterItem.id} tabIndex="-1" role="dialog" >
            <div className="modal-dialog h-100  m-0" role="document">
                <div className=" border border-secondary modal-content h-100  rounded-0">
                    <div className="modal-header p-1">
                        <h6 className="modal-title p-1 pr-3 fc-gray font-weight-light">
                           {t("search.choose")}  {this.filterTitle()}</h6>
                        <div>
                            <FontAwesomeIcon icon="arrow-left" size="xs" onClick={e => this.closeModal("searchModal" + filterItem.id)} />
                        </div>
                    </div>
                    <div className="modal-body h-90 p-1">
                        {filterItem.id === filterBy ? this.selectType() : <div>{filterBy}</div>}
                    </div>

                </div>
            </div>
        </div>;
    }

    render() {
        return (
            <div >
                <div className='row m-0 p-0 justify-content-center' name='searchOptions'>
                    <div className=' col-10  p-0  pt-3 pb-3' id='12col'>
                        <SelectFree t={this.props.t} />
                    </div>
                </div>
  
                        <Filters
                            filtersArray={this.props.filtersArray}
                            onCancelFilter={this.onCancelFilter}
                            onAddingFilter={this.onAddingFilter}
                            renderSearchModal={this.renderSearchModal}
                            filterBy={filterBy}
                            t={this.props.t}
                              />
            </div>
        );
    }
}

export default withI18n()(observer( Search));
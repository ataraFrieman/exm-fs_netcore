
import React, { Component } from 'react';
import Select from 'react-select';
import { observer } from 'mobx-react';


var Branches;
var fillOrganizations = res => {
    Branches = res.map(Organization => ({ value: Organization.id, label: Organization.name }));
    return Branches;
};

class SelectBranch extends Component {

    constructor(props) {
        super(props);
        this.state = { isLoading: false };
    }

    componentWillMount() {
        //this.props.branches.loadBranches();
    }
        
    render() {
        var BranchesList = this.props.branches.branchesList;
        var Branches = BranchesList.map(branch => ({ value: branch.id, label: branch.name }));
        return (
            <div>
                {this.props.branches.branchesList.length}
            <div>
                    <Select id='branchSelect' options={Branches} autoFocus='true'
                        className="fc-gray bc-gray rounded-0 filterSelect fs-12"
                    onChange={evt => this.props.onSelect([evt.value, evt.label])}
                    
                    placeholder={'לדוגמא:פתח תקוה-מרכז...'} arrowRenderer />

            </div>
            <div className="row m-0 p-0 justify-content-center">
                    {Branches ? Branches.map(item => {
                        return <div className="card col-12 rounded-0 fc-gray bc-gray fs-12 mb-2" key={item.label} onClick={e => {
                        this.props.onSelect([item.value, item.label]);
                            this.props.closeModal("searchModalbranchSelect")
                    }}>
                        <div className="card-body p-1 text-center font-weight-light">{item.label}</div>
                    </div>;
                }) : "no"}
            </div>
            </div>
        );
    }
}

export default observer(SelectBranch);
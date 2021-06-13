import React, { Component } from 'react';
import Select from 'react-select';
import { observer } from 'mobx-react';
import serviceTypes from '../../stores/serviceTypes';


class SelectServiceType extends Component {

    constructor(props) {
        super(props);

        this.state = {
            ServiceTypes: '',
            isLoading: false
        };
        console.log(props);
        
    };
    componentWillMount() {
        //if (this.props.serviceTypes.serviceTypes || this.props.serviceTypes.serviceTypesList.length)
            this.props.serviceTypes.loadServiceTypes();

    }
    componentDidMount() {
        var selectedCategory;
        this.props.filtersArray.map(f => {
            if (f.id === 'categorySelect') {
                selectedCategory = f.valueId;
                console.log(selectedCategory);
            }
        });

        
    }
    render() {
        var STList = this.props.serviceTypes.serviceTypesList;
        var serviceTypesList = STList.map(ServiceType => ({ value: ServiceType.id, label: ServiceType.description }));
        return (
            <div>
                <div>
                    <Select id='ServiceTypeSelect' options={serviceTypesList} autoFocus='true'
                        className="fc-gray bc-gray rounded-0 filterSelect fs-12"
                        onChange={evt => this.props.onSelect([evt.value, evt.label])}
                        //      placeholder={this.state.isLoading?'טוען נתונים...':'לדוגמא: ' + this.state.ServiceTypes[0].value} arrowRenderer />
                        placeholder={'לדוגמא: רופא שיניים...'} arrowRenderer />
                </div>
                <div className="row m-0 p-0 justify-content-center">
                    {serviceTypesList ? serviceTypesList.map(item => {
                        return <div key={item.values} className="card col-12 mb-2 rounded-0 fc-gray bc-gray" onClick={e => {
                            this.props.onSelect([item.value, item.label]);
                            this.props.closeModal("searchModalserviceTypeSelect")
                        }}>
                            <div className="card-body p-1 text-center font-weight-light fs-14">{item.label}</div>
                        </div>;
                    }) : "no"}
                </div>
            </div>
        );
    }
}

export default observer(SelectServiceType);
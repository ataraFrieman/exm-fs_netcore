import React, { Component } from 'react';
import Select from 'react-select';
import * as http from '../../helpers/Http';
import { observer } from 'mobx-react';

class SelectProvider extends Component {

    constructor(props) {
        super(props);

        this.state = {
            isLoading: false,
            serviceProvidersList:[]
            //providersList: []
        };
        this.initProviders = this.initProviders.bind(this);
    };

    initProviders() {

        var data = this.props.serviceProviders;
        if (!data || !data.length)
            return [];
        var list = [];
        data.forEach(function (provider, index) {
            list.push({
                value: provider.id,
                label: provider.titledFullName
            });
        });
        this.setState({ serviceProvidersList: list })
    }

    componentWillMount() {
        this.initProviders();
    }

    componentDidMount() {

    }

    getProviders(res) {
        const list = res.map(provider => ({
            value: provider.id,
            label: this.getName(provider)
        })
        );
        return list;
    }

    getName(provider) {
        const title = provider.title ? provider.title : '';
        return (title + ' ' + provider.firstName + ' ' + provider.lastName).trim();
    }

    render() {
        return (
            <div> 
                <div>
                    <Select
                        id='selectProviders' options={this.state.serviceProvidersList} autoFocus='true'
                        className="fc-gray bc-gray rounded-0 filterSelect"
                        onChange={evt => {
                            this.props.onSelect([evt.value, evt.label]);// value=provider.id, label=name
                            this.props.closeModal("searchModalserviceProviderSelect");
                        }}
                        placeholder="forExampleMosheMoskowitz" arrowRenderer />
                </div>
                <div className="row m-0 p-0 justify-content-center">
                    {
                        this.state.serviceProvidersList ? this.state.serviceProvidersList.map((item, index) => {
                            return (<div className="card col-12 rounded-0 fc-gray bc-gray"
                                onClick={e => {
                                    this.props.onSelect([item.value, item.label]);// value=provider.id, label=name
                                    this.props.closeModal("searchModalserviceProviderSelect")
                                }} key={index}>
                                <div className="card-body p-2 text-center font-weight-light">{item.label}</div>
                            </div>);
                        })
                            : ""
                    }
                </div>
            </div>

        );
    }
}
export default observer(SelectProvider);
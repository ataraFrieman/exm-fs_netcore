
import React, { Component } from 'react';
import Select from 'react-select';


//const organizations = [
//    { value: '1', label: 'רפואה' },
//    { value: '2', label: 'רכב' },
//    { value: '3', label: 'טיפוח ויופי' }
//];



var organizations;

var token = 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImphY29iIiwidHlwIjoiQXBpVXNlciIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiNDBlY2ZjYzktNmIzZS00MDFjLTlkYzctMzI5NGE3Nzk5OWVkIiwiZXhwIjoxODAwNTI0NjMyLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdCJ9.UHBHxFBD3eB4zESXdiquguVXmYyf8ZVDgtoSPkWtlnw';

var FillOrganizations = res => {
    organizations = res.map(organization => ({ value: organization.id, label: organization.name }));
    console.log(organizations);
    return organizations;
};






class SelectOrganiztion extends Component {

    constructor(props) {
        super(props);
        this.state = {
            organizations: 'no options',
            isLoading: false
        };
    }

    componentDidMount() {
        this.setState({ isLoading: true });
        fetch(this.props.profixUrl + 'Organizations/GetOrganizations', {
            method: 'GET',
            headers: {
                'Accept': 'application/json, text/plain, */*',
                'Content-Type': 'application/json',
                'Access-Control-Allow-Origin': '*',
                'Authorization': token
            }
        })
            .then(res => res.json())
            .then(res => this.setState({ organizations: FillOrganizations(res.entities), isLoading: false }));
    }

    render() {
        return (
            <div>
                <div>
                    <Select id='Selectorganization' options={organizations} autoFocus='true'
                        onChange={evt => {
                            this.props.onSelect([evt.value, evt.label]);
                            this.props.closeModal()
                        }}
                        className="fc-gray bc-gray rounded-0 filterSelect"
                        placeholder={'לדוגמא:מאוחדת...'} arrowRenderer
                    />
                </div>
                <div className="row m-0 p-0 justify-content-center">
                    {organizations ? organizations.map(item => {
                        return <div className="card col-12 rounded-0 fc-gray bc-gray" key={item.value} onClick={e => {
                            this.props.onSelect([item.value, item.label]);
                            this.props.closeModal()}}>
                            <div className="card-body p-2 text-center font-weight-light">{item.label}</div>
                        </div>;
                    }) : "no"}
                </div>
            </div>

        );
    }
}

export default SelectOrganiztion;
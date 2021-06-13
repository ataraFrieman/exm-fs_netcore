import React, { Component } from 'react';
import Select from 'react-select';


var categories1;

var token = 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImphY29iIiwidHlwIjoiQXBpVXNlciIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiNDBlY2ZjYzktNmIzZS00MDFjLTlkYzctMzI5NGE3Nzk5OWVkIiwiZXhwIjoxODAwNTI0NjMyLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdCJ9.UHBHxFBD3eB4zESXdiquguVXmYyf8ZVDgtoSPkWtlnw';

export function  FillCategories( res ) {
    categories1 = res.map(category => ({ value: category.id, label: category.description }));
    console.log(categories1);
    return categories1;
};






class SelectCategory extends Component {

    constructor(props) {
        super(props);
        
        this.state = {
            categories: 'no options',
            isLoading: false
        };
    }

    componentDidMount() {
        this.setState({ isLoading: true });
        fetch(this.props.profixUrl + 'serviceTypes/GetCategories', {
            method: 'GET',
            headers: {
                'Accept': 'application/json, text/plain, */*',
                'Content-Type': 'application/json',
                'Access-Control-Allow-Origin': '*',
                'Authorization': token
            }
        })
            .then(res => res.json())
            .then(res => this.setState({ categories: FillCategories(res.entities), isLoading: false }));
    }

    render() {
        return (
            <div>
                <Select id='SelectCategory' options={categories1} autoFocus='true'
                    onChange={evt => this.props.onSelect([evt.value, evt.label])}
                    placeholder={'לדוגמא: רפואה...'} arrowRenderer />
            </div>

        );
    }
}

export default SelectCategory;
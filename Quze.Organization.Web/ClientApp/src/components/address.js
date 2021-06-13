import React, { Component } from 'react';
import { FormGroup, FormControl, Row } from "react-bootstrap";
import * as $ from 'jquery';




export class Address extends Component {
    displayName = Address.name

    constructor(props) {
        super(props);
        this.state = {
            street: this.props.street ? this.props.street : 0,
            houseNumber: this.props.houseNumber,
            cities: this.props.cities,
            streets: this.props.streets,
            filteredStreets: this.props.streets,
            handleInputChange: this.props.onChange
        }
        this.onCityChange = this.onCityChange.bind(this);
        this.onStreetChange = this.onStreetChange.bind(this);
        this.getOptions = this.getOptions.bind(this);
        this.onHouseNumberChange = this.onHouseNumberChange.bind(this);

        this.onCityChange(this.props.street ? this.props.street.city : 0);
    }

    onCityChange(cityId) {
        if (!this.props.cities || !this.props.streets)
            return;
        var city = this.props.cities.filter(c => c.id == cityId);
        var streets = this.props.streets.filter(s => s.cityId == cityId);
        if (city && city.length) {
            var street = { id: 0, name: "", cityId: cityId, city: city[0] };
            this.setState({
                street: street,
                filteredStreets: streets
            });
            var event1 = { target: { type: "address", name: "street", value: street } };
            var event2 = { target: { type: "address", name: "streetId", value: street.id } };
            this.state.handleInputChange(event1);
            this.state.handleInputChange(event2);
        }
    }


    onStreetChange(streetId) {
        var street = this.props.streets.filter(s => s.id == streetId);
        if (street && street.length) {
            this.setState({
                street: street[0]
            });
            var event1 = { target: { type: "address", name: "street", value: street[0] } };
            var event2 = { target: { type: "address", name: "streetId", value: streetId } };
            this.state.handleInputChange(event1);
            this.state.handleInputChange(event2);
        }
    }

    onHouseNumberChange(newValue) {
        this.setState({
            houseNumber: newValue
        });
        var event = { target: { type: "text", name: "houseNumber", value: newValue } };
        this.state.handleInputChange(event);
    }

    getOptions(options, selected) {
        if (!options)
            return;
        var optionsTags = [];
        optionsTags.push(<option key="-1" hidden value="0" disabled>בחר</option>);
        for (var i = 0; i < options.length; i++) {
            optionsTags.push(<option key={options[i].id} value={options[i].id}>{options[i].name}</option>)
        }
        return optionsTags;
    }
    render() {
        var citisOptionsTags = this.getOptions(this.props.cities, this.props.cityVal);
        var streetsOptionsTags = this.getOptions(this.state.filteredStreets, this.props.streetVal);

        return (
            <div className="row">
                <div className="col-4">
                    {/* <FormControl></FormControl> instead <ControlLabel></ControlLabel> in the new version */}
                    <span>עיר</span>
                    <FormControl id="#selectpicker" name="city" onChange={e => this.onCityChange(e.target.value)} componentClass="select" placeholder="select"
                        value={this.state.street.city.id}>{citisOptionsTags}</FormControl>
                    {$('#selectpicker').val(this.props.cityVal)}
                    
                </div>
                <div className="col-5">
                    {/* <FormControl></FormControl> instead <ControlLabel></ControlLabel> in the new version */}
                    <span>רחוב</span>
                    <FormControl name="street" onChange={e => this.onStreetChange(e.target.value)} componentClass="select" placeholder="select"
                        value={this.state.street.id}>{streetsOptionsTags}</FormControl>
                </div>
                <div className="col-3">
                    {/* <FormControl></FormControl> instead <ControlLabel></ControlLabel> in the new version */}
                    <span>מספר בית</span>
                    <FormControl name="houseNumber" onChange={e => this.onHouseNumberChange(e.target.value)} type="text" placeholder="מספר בית" value={this.state.houseNumber}></FormControl>
                </div>
            </div>

        );
    }


}
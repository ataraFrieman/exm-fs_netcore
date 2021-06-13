import React, { Component } from "react";

export class CheckBoxesList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            checkedList: [],
            handleInputChange: this.props.onChange
        };
        this.serviceTypeCheck = this.serviceTypeCheck.bind(this);
        this.uncheckedChildrens = this.uncheckedChildrens.bind(this);
        this.getChildrens = this.getChildrens.bind(this);

    }
    //
    render() {
        var serviceTypes = this.props.serviceTypes;
        var serviceTypesCheckBox = [];
        if (serviceTypes && serviceTypes.length) {
        serviceTypes.map((item) => {
            serviceTypesCheckBox.push(
                <div class="custom-control custom-checkbox" key={item.id}>
                    <input type="checkbox" class="custom-control-input" id={item.id} value={item.description} onClick={e => this.serviceTypeCheck(e)}
                        checked={this.state.checkedList.find(c => c.serviceType.id == item.id)} />
                    <label class="custom-control-label" for={item.id}>{item.description}</label>
                </div>
            );
            });
        }
        return (serviceTypesCheckBox);
    }

    serviceTypeCheck(e) {
        console.log("e.target.id: " + e.target.id + "e.target.value: " + e.target.value + " e.target.checked: " + e.target.checked);
        var checkedList = this.state.checkedList;
        var serviceTypes = this.props.serviceTypes;
        var serviceType = serviceTypes.find(s => s.id == e.target.id);

        if (serviceType) {

            var currentServiceType = { id: 0, serviceProviderId: this.props.serviceProviderId, serviceTypeId: serviceType.id, serviceType: serviceType };

            if (e.target.checked) {
                checkedList.push(currentServiceType);
                if (currentServiceType.serviceType.parentServiceId > 0) { //check the parents
                    do {
                        serviceType = serviceTypes.find(s => s.id == currentServiceType.serviceType.parentServiceId);
                        currentServiceType = { id: 0, serviceProviderId: this.props.serviceProviderId, serviceTypeId: serviceType.id, serviceType: serviceType };
                        var index = checkedList.find(c => c.serviceType.id == currentServiceType.serviceType.id);
                        if (index)
                            break;
                        else {
                            checkedList.push(currentServiceType);
                        }
                    }
                    while (currentServiceType.serviceType.parentServiceId > 0);
                }
            }
            else {
                var i = 0;
                for (; checkedList[i].serviceType.id != e.target.id && i < checkedList.length; i++);
                checkedList.splice(i, 1);   
                var childrens = [];
                childrens = childrens.concat(this.getChildrens(checkedList, e.target.id));//unchecked childrens
               //checkedList = this.uncheckedChildrens(checkedList, e.target.id);//unchecked childrens
                for (var i = 0; i < childrens.length; i++) {
                    var index = checkedList.indexOf(childrens[i]);
                    checkedList.splice(index, 1);
                }
                
            }

            this.setState({ checkedList: checkedList });
            var event = { target: { type: "checkBoxesList", name: "serviceProvidersServiceTypes", value: checkedList } };
            this.state.handleInputChange(event);
        }
    }

    uncheckedChildrens(checkedList, parentServiceId) {
        //var childrens = checkedList.find(c => c.parentServiceId == parentServiceId);
        for (var i = 0; i < checkedList.length; i++)
            if (checkedList[i].parentServiceId == parentServiceId) {
                var id = checkedList[i].id;
                checkedList.splice(i, 1);
                checkedList = this.uncheckedChildrens(checkedList, id);
                
            }
        return checkedList;
    }

    getChildrens(checkedList, parentServiceId) {
        var childrens = checkedList.filter(c => c.serviceType.parentServiceId == parentServiceId);
        if (childrens && childrens.length)
            for (var i = 0; i < childrens.length; i++) {
                childrens = childrens.concat(this.getChildrens(checkedList, childrens[i].serviceType.id));
                }
        return childrens;
    }

}
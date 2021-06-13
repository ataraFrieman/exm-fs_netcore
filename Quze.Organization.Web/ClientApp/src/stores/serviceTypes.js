import { observable, computed, reaction, action, decorate } from 'mobx';
import * as http from '../helpers/Http';
var isLoading = false;
export default class serviceTypes {
    serviceTypesList = [];


    loadServiceTypes(forceUpdate) {
        var thisObj = this;
        if ((!this.serviceTypesList.length || forceUpdate) && !isLoading) {
            isLoading = true

            http.get('api/ServiceTypes/loadServiceTypeDatailsByOrganizationId')
                .then(res => {
                    isLoading = false;
                    res.entities.forEach(function (st, index) {
                        thisObj.serviceTypesList.push(st);
                    });
                    return thisObj.serviceTypesList;
                });
        }
        else
            thisObj.serviceTypesList = thisObj.serviceTypesList;

    }

    static fromJS(array) {
        return new serviceTypes();
    }
}
decorate(serviceTypes, {
    serviceTypesList: observable,
    loadServiceTypes: action,
});
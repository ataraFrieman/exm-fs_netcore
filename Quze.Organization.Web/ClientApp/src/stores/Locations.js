import { observable, computed, reaction, action, decorate } from 'mobx';
import * as http from '../helpers/Http';
var isLoading = false;
export default class Locations {
    serviceStationList = [];

    getServiceStations(forceUpdate) {
        var thisObj = this;
        if ((!this.serviceStationList.length || forceUpdate) && !isLoading) {
            isLoading = true

            http.get('api/Operations/GetRooms')
                .then(res => {
                    isLoading = false;
                    res.entities.forEach(function (st, index) {
                        thisObj.serviceStationList .push(st);
                    });
                    return thisObj.serviceStationList;
                });
        }
        else
            thisObj.serviceStationList = thisObj.serviceStationList;

    }

    static fromJS(array) {
        return new Locations();
    }
}
decorate(Locations, {
    serviceStationList: observable,
    getServiceStations: action,
});
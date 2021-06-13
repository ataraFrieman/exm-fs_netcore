import { observable, computed, reaction, action, decorate } from 'mobx';
import * as http from '../helpers/Http';
var isLoading = false;
export default class CancelationReasons {
    cancelationReasonsList = [];

    getCancelationReasons(forceUpdate) {
        var thisObj = this;
        if ((!this.cancelationReasonsList.length || forceUpdate) && !isLoading) {
            isLoading = true

            http.get('api/Operations/GetCancelationReasons')
                .then(res => {
                    console.log("Reasons: ", res)
                    isLoading = false;
                    res.entities.forEach( element => {
                        thisObj.cancelationReasonsList.push(element);
                    });
                    return thisObj.cancelationReasonsList;
                });
        }
        else
            thisObj.cancelationReasonsList = thisObj.cancelationReasonsList;
    }

    static fromJS(array) {
        return new CancelationReasons();
    }
}
decorate(CancelationReasons, {
    cancelationReasonsList: observable,
    loadServiceTypes: action,
});
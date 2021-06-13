import { observable, computed, reaction, action, decorate } from 'mobx';
import * as http from '../helpers/Http';
var isLoading = false;
export default class serviceQueues {
    serviceQueuesList = [];

    getServiceQueues(forceUpdate) {
        var thisObj = this;
        if ((!this.serviceQueuesList.length || forceUpdate) && !isLoading) {
            isLoading = true

            http.get('api/Operations/GetQueuesDates')
                .then(res => {
                    console.log("GetQueuesDates: ", res)
                    isLoading = false;
                    res.forEach( element => {
                        let time = element.beginTime.split("T")
                     
                        element.beginTime = time[0] + ", " + time[1]
                        thisObj.serviceQueuesList.push(element);
                    });
                    return thisObj.serviceQueuesList;
                });
        }
        else
            thisObj.serviceQueuesList = thisObj.serviceQueuesList;

    }

    createServiceQ(data)
    {
        http.post('api/Operations/CreateNewShift', data)
            .then(res => {
                return res;

            })
            .catch(error => { console.log("Error:", error) });
    }

    static fromJS(array) {
        return new serviceQueues();
    }
}
decorate(serviceQueues, {
    serviceQueuesList: observable,
    loadServiceTypes: action,
});
import { observable, computed, reaction, action, decorate } from 'mobx';
import * as http from '../helpers/Http';

var isLoading = false;
export default class Fellows {
    FellowsList = [];
    
    getFellows(forceUpdate) {
        var thisObj = this;
        if ((!this.FellowsList.length || forceUpdate) && !isLoading) {
            isLoading = true;
            http.get('api/Fellows/LoadFellowsByOrganizationId')
                .then(res => {
                    isLoading = false;
                    if (res && res.entities && res.entities.length)
                        thisObj.FellowsList = res.entities

                });
        }
        else
            return thisObj.FellowsList;
    }



    toJS() {
        return this.FellowsList.map(f => f);
    }

    deleteFellow(id) {
        this.FellowsList = this.FellowsList.filter((f) => f.id != id);
    }
    updateFellow(updatedFellow) {

        var fellow = this.FellowsList = this.FellowsList.indexOf((f) => f.id == updatedFellow.id);
        this.FellowsList[fellow].firstName = updatedFellow.firstName;
        this.FellowsList[fellow].lastName = updatedFellow.lastName;
        this.FellowsList[fellow].identityNumber = updatedFellow.identityNumber;
    }
    addFellow(fellow) {

        this.FellowsList.push(fellow);
    }

    static fromJS(array) {
        return new Fellows();
    }

}
decorate(Fellows, {
    FellowsList: observable,
    getFellows: action,
    deleteFellow: action,
    addFellow: action

})
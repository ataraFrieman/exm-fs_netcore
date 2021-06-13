import { observable, computed, reaction, action, decorate } from 'mobx';
import * as http from '../helpers/Http';
var isLoading = false;
export default class Departments {
    departmentsList = [];

    getDepartments(forceUpdate) {
        var thisObj = this;
        if ((!this.departmentsList.length || forceUpdate) && !isLoading) {
        isLoading = true
        http.get('api/Operations/GetDepartments')
            .then(res => {
                isLoading = false;
                res.entities.forEach(function (st, index) {
                    thisObj.departmentsList.push(st);
                });
                return thisObj.departmentsList;
            });
    }
        else
            thisObj.departmentsList = thisObj.departmentsList;

    }
    
    static fromJS(array) {
        return new Departments();
    }
}
decorate(Departments, {
    departmentsList: observable,
    getDepartments: action,
});
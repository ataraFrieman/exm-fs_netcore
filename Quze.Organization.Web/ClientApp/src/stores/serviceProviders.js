import { observable, computed, reaction, action, decorate } from 'mobx';
import * as http from '../helpers/Http';

var isLoading = false;
export default class ServiceProviders {
    serviceProviders = [];

    constructor() {
        this.getServiceProviders(null);
    }
    getServiceProviders(forceUpdate) {
        var thisObj = this;
        if ((!this.serviceProviders.length || forceUpdate) ) {
            isLoading = true;
            //this.toJS,
            http.get('ServiceProviders/getAll')
                .then(res => {
                    isLoading = false;
                    if (res && res.entities && res.entities.length)
                        res.entities.forEach(function (value) {
                            thisObj.serviceProviders.push(value);
                        });
                    thisObj.serviceProviders = res.entities;
                    return thisObj.serviceProviders;
                });
            // );
        }
        else
            return thisObj.serviceProviders;

    }

    addServiceProvider(serviceProvider) {
        http.post("ServiceProviders/CreateSPAsync",serviceProvider).
            then(res => {
                if (res && res.entity) {
                    this.serviceProviders.push(res.entity);
                    return(res.entity);
                }
                    
            })
    }
    deleteServiceProvider(serviceProvider) {
        http.deleteX("ServiceProviders", serviceProvider).
            then(res => {
                if (res && res.entity) {
                    this.serviceProviders = this.serviceProviders.filter(s => s.id != serviceProvider.id);;
                    return (res.entity);
                }

            })
    
    }


    toJS() {
        return this.serviceProviders.map(sp => sp);
    }

    static fromJS(array) {
        return new ServiceProviders();
    }

}
decorate(ServiceProviders, {
    serviceProviders: observable,
    getServiceProviders: action,
    addServiceProvider: action,
    deleteServiceProvider: action

})
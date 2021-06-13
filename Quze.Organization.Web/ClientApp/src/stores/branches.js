import { observable, computed, reaction, action, decorate } from 'mobx';
import * as http from '../helpers/Http';

export default class branches {
    branchesList = [];

    constructor() {
        return this.getBranches();
    }

    getBranches(forceUpdate, getAllData) {
        var thisObj = this;

        if (!this.branchesList || !this.branchesList.length || forceUpdate)
            return this.loadBranches();

        return this.branchesList;

    }
    loadBranches(forceUpdate) {
        var thisObj = this;
            http.get('api/Branches/getBranches')
                .then(res => { 
                    thisObj.branchesList = res.entities;
                    return thisObj.branchesList;
                });
    }
    
    static fromJS(array) {
        return new branches();
    }
}
decorate(branches, {
    branchesList: observable,
    loadBranches: action,
});
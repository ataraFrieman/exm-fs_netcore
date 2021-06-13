import { observable, computed, reaction, action, decorate } from 'mobx';
import * as http from '../helpers/Http';

export default class queues {
    queuesList = [];

    constructor() {
        this.loadMore = this.loadMore.bind(this);
        return this.getQueues();

    }

    getQueues(forceUpdate, getAllData) {
        var thisObj = this;
        if (!this.queuesList || !this.queuesList.length || forceUpdate)
            return this.loadQueues();
        return this.queuesList;
    }

    loadQueues(forceUpdate, lastId) {
        var thisObj = this;
        http.get('api/QManagament' + (lastId ? ("?idStart=" + lastId) : ""))
            .then(res => {
                if (res && res.length && res != "No Content")
                    res.forEach(e => { thisObj.queuesList.push(e) });
                return thisObj.queuesList;
            });
    }
    addAppointement(SQId, appointment) {
        if (!SQId || !appointment)
            return;
        var list = this.queuesList;
        var SQItem = list.filter(s => s.id == SQId);
        if (SQItem && SQItem.length) {
            SQItem[0].appointments.push(appointment);
            //this.queuesList.replace(list);
        }
    }
    setQueue(newQ) {
        var index = this.queuesList.indexOf(q => q.id == newQ.id);
        if (index != 1)
            this.queuesList[index] = newQ;
    }
    loadMore() {
        this.loadQueues(false, this.queuesList[this.queuesList.length - 1].id)
    }

    static fromJS(array) {
        return new queues();
    }
}
decorate(queues, {
    queuesList: observable,
    loadQueues: action,
    addAppointement: action,
    setQueue: action,
    loadMore: action
});
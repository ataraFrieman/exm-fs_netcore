import { observable, computed, reaction, action, decorate } from 'mobx';

export default class SchedualFilters {
    filtersList = [
        { id: 'branchSelect', key: 'סניף', value: '', valueId: '', parentId: '' },
        { id: 'serviceTypeSelect', key: 'סוג שירות', valueId: '', value: '', parentId: '' },
        { id: 'dateRangeSelect', key: 'בין התאריכים', value: '', valueId: '', parentId: '' },
        { id: 'timeRangeSelect', key: 'בין השעות', value: '', valueId: '', parentId: '' },
        { id: 'serviceProviderSelect', key: 'שם רופא', value: '', valueId: '', parentId: '' }
    ];
    filterBy = '';

    toJS() {
        return this.filtersList.map(filter => filter.toJS());
    }
    setFilterItem(key, value, id) {
        var list = this.filtersList;
        var item = list.filter((e) => e.id == key);
        
        if (item&&item.length) {
            item = item[0];
            item.value = value;
            item.valueId = id;
        }
        this.filtersList.replace(list);
    }
    static fromJS(array) {
        const filtersStore = new SchedualFilters();
        //filtersStore.filtersList = array.map(item => TodoModel.fromJS(todoStore, item));
        return filtersStore;
    }
}
decorate(SchedualFilters, {
    filtersList: observable,
    filterBy: observable,
    setFilterItem:action

})
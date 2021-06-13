import { observable, computed, reaction, action, decorate } from 'mobx';
import * as http from '../helpers/Http';

var isLoading = false;
export default class Equipment {
    EquipmentsList = [];

    getEquipments(forceUpdate) {
        var thisObj = this;
        if ((!this.EquipmentsList.length || forceUpdate) && !isLoading) {
            isLoading = true;
            http.get('api/Equipment/GetEquipmentsByOrganizationId')
                .then(res => {
                    isLoading = false;
                    if (res && res.entities && res.entities.length)
                        thisObj.EquipmentsList = res.entities;
                    console.log(res.entities);
                });
        }
        else
            return thisObj.EquipmentsList;
    }

    updateEquipments(EquipmentsList, EquipmentsListTaken, uvAbleEquipmentsList) {
        EquipmentsListTaken.forEach(eq => {
            var equipment = this.findEquipment(EquipmentsList, eq.eqpId);
            EquipmentsList[equipment] = eq.equipment;
            if (eq.equipment.numAllow === eq.equipment.maximumAmount)
            EquipmentsList[equipment].isQuantityLeft = false;
            else
            EquipmentsList[equipment].isQuantityLeft = true;
        });
        if (uvAbleEquipmentsList && uvAbleEquipmentsList.length != 0) {
            uvAbleEquipmentsList.forEach(eqId => {
                var equipment = this.findEquipment(EquipmentsList, eqId);
                EquipmentsList[equipment].isQuantityLeft = false;
            })
        }
        this.EquipmentsList = EquipmentsList;
        return EquipmentsList;
    }

    findEquipment(EquipmentsList, eqpId) {
        return EquipmentsList.findIndex((eq) => eq.id === eqpId)
    }
    toJS() {
        return this.EquipmentsList.map(f => f);
    }

    static fromJS(array) {
        return new Equipment();
    }

}
decorate(Equipment, {
    EquipmentsList: observable,
    getEquipments: action,
    updateEquipments: action,
    findEquipment: action
})
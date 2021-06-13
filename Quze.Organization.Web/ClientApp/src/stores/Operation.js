import * as http from '../helpers/Http';
import EquipmentStore from './Equipment';

export default class operation {
    changStatuseOperation(operationQueue,appointment,state,funcRescheduleSetState,funcAddSurgery) {
        var editRequest =
        {
            OperationQueue: operationQueue ? operationQueue : null,
            oldAppointment: appointment,
            State: state,
        }
        http.post("api/StatusOperation/UpdadateStatusApointment", editRequest)
            .then(res => {
                if (state === "S")
                funcRescheduleSetState(res);
            else
                funcAddSurgery(res);
            })
            .catch(error => { console.log("Error:", error) });
    }

    cancelApointment(operationQueue,appointment,state,funcRescheduleSetState,funcAddSurgery) {
        var editRequest =
        {
            OperationQueue: operationQueue ? operationQueue : null,
            oldAppointment: appointment,
            State: state,
        }
        http.post("api/StatusOperation/CancelApointment", editRequest)
            .then(res => {
                if (state === "S")
                funcRescheduleSetState(res);
            else
                funcAddSurgery(res);
            })
            .catch(error => { console.log("Error:", error) });
    }

    deleteApointment(operationQueue,appointment,state,funcRescheduleSetState,funcAddSurgery) {
        var editRequest =
        {
            OperationQueue: operationQueue ? operationQueue : null,
            oldAppointment: appointment,
            State: state,
        }
        http.post("api/Operations/DeleteOperation", editRequest)
            .then(res => {
                if (state === "S")
                funcRescheduleSetState(res);
            else
                funcAddSurgery(res);
            })
            .catch(error => { console.log("Error:", error) });
    }

    updateDelayApointment(appointment) {
        http.post("api/StatusOperation/UpdateDelayApointment", appointment)
            .then(resSucsseful => {
                if (resSucsseful)
                    return true;
                return false;
            })
            .catch(error => { console.log("Error:", error) });
    }

    editOperation(data, operationQueue, equipments, appointment, state, funcLoad, funcAddSurgery, funcRescheduleSetState) {

        var editRequest =
        {
            Entity: data,
            OperationQueue: operationQueue ? operationQueue : null,
            oldAppointment: appointment,
            State: state,
            Equipments: equipments.EquipmentsList
        }
        funcLoad();
        http.post('api/EditOperation/IsItPossibleToEditAppointmentAsync', editRequest)
            .then(res => {

                if (state === "S")
                    funcRescheduleSetState(res);
                else
                    funcAddSurgery(res);
            })
            .catch(err => {
                console.log("Error: ", err)
            });
    }

    reschedual(operationQueue, equipments, funcReschedule) {
        var data = {
            operationQueue: operationQueue,
            BeginTime: operationQueue.serviceQueue.beginTime,
            Equipments: equipments.EquipmentsList
        };
        http.post('api/NewInlay', data)
            .then(res => {
                funcReschedule(res);
            })
            .catch(error => { alert("Error:", error) });
    }
    static fromJS() {
        return new operation();
    }

}

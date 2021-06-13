import * as http from '../helpers/Http';

export default class MinimalKit {

    addDocument(descriptionDoc, isRequired, appointmentId) {
        http.post("api/MinimalKit/AddDocument", { description: descriptionDoc, isRequired: isRequired, appointmentId: appointmentId })
            .then(data => {
                return data;
            })
            .catch(error => { console.log("Error:", error); });
    }

    //     /* 
    //     saveMK(minimalKitData){
        //     post('api/MinimalKit/SaveMinimalKit',minimalKitData)
        //     .then(data => {
        //         console.log("data: ", data)
        //         console.log("MK is saved")
        //     })
        //     .catch(x => { console.log("Error: ", x) })
    //     }
    //     */

    static fromJS() {
        return new MinimalKit();
    }
}


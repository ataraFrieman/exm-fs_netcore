import history from "./History";
import { getUserId } from './User'

export function getOrganizationId() {
    var organizationId = localStorage.getItem("user");
    if (organizationId) {
        organizationId = JSON.parse(organizationId).organizationId;
        return organizationId ? organizationId : false;

    }
    else {
        history.push("/login");
        return;
    }
}


// export function getFellowByUserId() {
//     return new Promise((resolve, reject) => {
//         var userID = getUserId();
//         if (userID)
//             http.get("fellow/" + userID)
//                 .then(res => {
//                     //console.log("res fellow");
//                     //console.log(res); => all the data
//                     resolve(res);
//                 })
//     });
// }
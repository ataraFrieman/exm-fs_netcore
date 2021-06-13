import history from './History';
import { isUserAuthenticated, getUser } from './User'

export function authHeader() {
    if (!isUserAuthenticated) {
        history.push("/login");
        window.location.reload();
    }

    let token = localStorage.getItem("securityToken");
    if (token) {
        return { 'Authorization': 'Bearer ' + token };
    } else {
        return {};
    }
}

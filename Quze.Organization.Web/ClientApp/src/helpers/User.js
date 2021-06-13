
import history from './History'
import { locale, locales } from 'moment';

export function isValidIsraeliID(idNum) {
    let id = String(idNum).trim();
    let intId = parseInt(id);
    if (id.length > 9 || id.length < 5 || !intId || isNaN(intId)) return false;

    // Pad string with zeros up to 9 digits
    id = id.length < 9 ? ("00000000" + id).slice(-9) : id;

    return Array.from(id, Number)
        .reduce((counter, digit, i) => {
            const step = digit * ((i % 2) + 1);
            return counter + (step > 9 ? step - 9 : step);
        }) % 10 === 0;
}

export function isValidPhone(phoneNum) {
    var phoneRe = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/;
    return phoneRe.test(phoneNum);
}

export function getUser() {
    var user = localStorage.getItem("user");
    user = JSON.parse(user);
    return user;
}

export function getUserId() {
    var user = getUser();
    return user.id;
}

export function getUserName() {
    var user = getUser();
    if (user)
        return user.fullName;
}

export function getOrganization() {
    let organization = localStorage.getItem('organization');
    let organizationDetails = JSON.parse(organization);
    if (organizationDetails === null) {
        return
    } else {
        let organizationObj = {
            Name: organizationDetails.name,
            Icon: organizationDetails.iconOrganization,
            userName: getUserName()
        }
        return organizationObj;
    }
}

export function isUserAuthenticated() {
    return (getUser() !== null && getUser() !== undefined);//משתמש קיים
}

//logout:
//set loggedIn = false
export function logout() {
    //clear localStorage
    //let lang = localStorage.getItem('Language')
    localStorage.clear();
    // if(lang == 'en'){
    //     localStorage.setItem('Language', 'en');
    // } else{
    //     localStorage.setItem('Language', 'he');
    // }
    //store.isLoggedIn = false;
    history.push("/login");
    window.location.reload();
}


export function getLanguage() {
    let user = localStorage.getItem("user");
    if (!user)
        return "en";
    else {
        user = JSON.parse(user);
        if (user && user.language)
            return user.language;
        else
            return "en";
    }

}
export function setLanguageStorege(lan) {
    let user = localStorage.getItem("user");
    if (!user)
        return;
    if (user) {
        user = JSON.parse(user);
        user.language = lan;
        localStorage.setItem("user", JSON.stringify(user));
    }
}

//export function getDir (){
//var language = getLanguage().substring(0, 2);
        //if (language === 'he' || language === 'ar')
        //    this.state.dir = "rtl";
        //else
        //    this.state.dir = "ltr";
//}

export function getDir() {
    let dir = localStorage.getItem('i18nextLng');
    return dir == 'en' || dir == "en-US" ? "ltr" : "rtl"
}
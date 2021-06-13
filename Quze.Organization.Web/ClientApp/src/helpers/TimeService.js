import * as moment from 'moment';
import 'moment/locale/pt-br';

export function getTime(d) {
    var date = new Date(d);
    var hours = date.getHours() == 0 ? "00" : date.getHours() < 10 ? "0" + date.getHours() : date.getHours().toString();
    var minutes = date.getMinutes() == 0 ? "00" : date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes().toString();
    return hours + ":" + minutes;
}

export function getDate(d) {
    var date = new Date(d);
    var day = date.getDate().toString();
    var month = (date.getMonth() +1).toString();
    var year = date.getFullYear().toString();
    return day + "/" + month + "/" + year + ", " + getTime(d);
}

export function getDateYYYMMDD(d) {
    var date = new Date(d);
    var day = date.getDate().toString();
    var month = (date.getMonth() + 1).toString();
    var year = date.getFullYear().toString();
    return  month+ "/" + day+ "/" + year;
}

export function addMinutes(d, minutes) {
    var date = new Date(d.setMinutes(d.getMinutes() + minutes));
    return date;
}

export function getStartWeek(d) {
    var dt = new Date(d);
    var day = dt.getDay();
    var diff = dt.getDate() - day;
    var startWeek = new Date(new Date(dt.setDate(diff)).setHours(0, 0, 0, 0));
    return startWeek;


}

export function getPrevSa(d) {
    var dt = new Date(d);
    var day = dt.getDay();
    var diff = dt.getDate() - day - 1;
    var prevSa = new Date(new Date(dt.setDate(diff)).setHours(0, 0, 0, 0));
    return prevSa;


}

export function getNextSun(d) {
    var dt = new Date(d);
    var day = dt.getDay();
    var diff = dt.getDate() - day + 7;
    var startWeek = new Date(new Date(dt.setDate(diff)).setHours(0, 0, 0, 0));
    return startWeek;
}

export function getEndWeek(d) {
    var dt = new Date(d);
    var day = dt.getDay();
    var diff = dt.getDate() - day + 6;
    var endWeek = new Date(new Date(dt.setDate(diff)).setHours(0, 0, 0, 0));
    return endWeek;


}

export function getTimeCurrentWeek(day, time) {
    var dt = new Date();
    dt = getStartWeek(dt);
    var diff = dt.getDate() + day-1;
    dt = new Date(new Date(dt.setDate(diff)).setHours(time.slice(0, 2), time.slice(3, 5), time.slice(6, 8)));
    return dt;
}

export function getDay(d) {
    var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    var day = days[d.getDay()];
    return day
}

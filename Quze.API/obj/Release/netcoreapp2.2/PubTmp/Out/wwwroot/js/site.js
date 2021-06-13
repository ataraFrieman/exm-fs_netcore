// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
//url = 'http://test.quze.ai/dev/api/v1.0/ml/schedule'
var url = 'http://test.quze.ai/dev/api/v1.0/ml/schedule';

//var url = 'http://test.quze.ai:770/api/ml/getall';
// 
let x = -3

let user = {
    fellowId: '5',
    serviceType: '12',
    compositeAppointmentType: '',
    timeSelectDates: ''
};

let isRTL = true;

function onChange() { //onChange(str,value)
    x++;
    console.log(x);

    // if( str === "fellowId") {
    //  user.fellowId = value //"'" + value + "'"
    // }
    // else if(str === "serviceType") {
    //     user.serviceType = value
    // }
    // else if (str === "compositeAppointmentType") {
    //             user.compositeAppointmentType =  value 
    // }
    // else {
    //     user.timeSelectDates = value 
    // }
    // console.log(user)
    //if (user.fellowId != "" && user.serviceType != "" ){ //&& user.compositeAppointmentType != "" user.timeSelectDates && != ""
    //     alert("fellowId: " + user.fellowId + " serviceType: " + user.serviceType)
    // }
    //predictDuration();

    predictDuration();
}

function getData() {
    let today = new Date();
    let month = today.getMonth() + 1
    if (month < 10)
        month = "0" + month;
    let date = today.getFullYear() + '-' + month + '-' + today.getDate();
    let time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
    let scheduledDate = date + " " + time;
    return {
    /* 
        'requestId': '111',
        'fellowIdentiyType': 'T.Z.',
        'serviceProviderId': '123',
        'serviceProviderGender': 'M',
        'serviceProviderMainLang': 'EN',
        'serviceProviderSecondarylLang': 'EN',
        'serviceProviderBirthDate': '1978-01-10',
        'fellowOS': 'android',
        'fellowMobileModel': 'Samsung galaxy 10',
        'fellowLocation': '52.509669, 13.376294',
        'fellowMobility': 'noraml',
        'FellowAccompany': 'no',
        'appointmentLocation': '52.509669, 13.376294',
        'firstCase': '2019-05-11 10:00:00',
        'departmentId': '123',
        'MinimalKit': '',

        'fellowGender': 'F',
        'fellowMainLang': 'HE',
        'fellowSecondarylLang': 'EN',
        'fellowPhoneNumber': '9725012345678',
        'fellowBirthDate': '1990-01-01',
        'visitNumber': '10',
        'organizationId': '123',
   
        'fellowId': user.fellowId,
        'serviceTypeId': user.serviceType,
        'scheduledDate': scheduledDate,
        'appointmentTime': scheduledDate
    */
        "requestId": "111", //- not needed
        "fellowIdentiyType": "T.Z.", // - not needed
        "serviceProviderId": "123", //- not needed
        "serviceProviderGender": "M", //- not needed
        "serviceProviderMainLang": "EN", //- not needed
        "serviceProviderSecondarylLang": "EN",// - not needed
        "serviceProviderBirthDate": "1978-01-10",// - not needed
        "fellowOS": "android", // - not needed
        "fellowMobileModel": "Samsung galaxy 10",//  - not needed
        "fellowLocation": "52.509669, 13.376294",// - not needed
        "fellowMobility": "noraml", // - not needed
        "FellowAccompany": "no", // - not needed
        "appointmentLocation": "52.509669, 13.376294",// - not needed
        "firstCase": "2019-05-11 10:00:00",// - not needed
        "departmentId": "123",// - not needed
        "MinimalKit": "", // - not needed

        "fellowGender": "F", //we get it from DB if it needed
        "fellowMainLang": "HE", //we get it from DB if it needed
        "fellowSecondarylLang": "EN",//we get it from DB if it needed
        "fellowPhoneNumber": "9725012345678", //we get it from DB if it needed
        "fellowBirthDate": "1990-01-01", //we get it from DB if it needed
        "visitNumber": "10", // we get it from DB if it needed
        "organizationId": "123", //we get it from DB if it needed
   
        "fellowId": "123", //user.fellowId, //"fellowId": "123",
        "serviceTypeId": "3",// user.serviceType, //"serviceTypeId": "3",
        "scheduledDate": "2019-07-18 12:57:45", //scheduledDate, //now time
        "appointmentTime": "2019-07-18 12:57:45" //scheduledDate // the time of the appointment
    };
}

function getToken() {
    // return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjI3MSIsInVuaXF1ZV9uYW1lIjoiMDUyNzE3NDIxNCIsInR5cCI6IjMiLCJnaXZlbl9uYW1lIjoiIiwiZmFtaWx5X25hbWUiOiIiLCJlbWFpbCI6IiIsIk9yZ2FuaXphdGlvbklkIjoiNSIsIklkZW50aXR5TnVtYmVyIjoiMjI1NDQ4MjMyIiwiZXhwIjoxNTYzMjY5MTYxLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiQVBJX1VzZXIifQ.LmYtpivGBPU0DY9ldVFET-BaSNeCZO5qBmuljxePRkA";
    return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjM4OSIsInVuaXF1ZV9uYW1lIjoiMDUwODczODM1MSIsInR5cCI6IjMiLCJnaXZlbl9uYW1lIjoiIiwiZmFtaWx5X25hbWUiOiIiLCJlbWFpbCI6IiIsIk9yZ2FuaXphdGlvbklkIjoiIiwiSWRlbnRpdHlOdW1iZXIiOiIzMTExMjkzMzIiLCJleHAiOjE1NzExMzYzODcsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3QiLCJhdWQiOiJBUElfVXNlciJ9.aQ5zihOQS-cel3kvzugTBj_zZ_0nbGTcBGTN5MHLWJE"
}

function predictDuration() {
    var data = JSON.stringify(getData());
    let dataDuration
    //console.log(data);
    let proxyurl = 'https://cors-anywhere.herokuapp.com/';
    
    
        fetch(url,{ //proxyurl + url 
                method: 'POST',
                data: data,
                headers:{
                     'Authorization': 'Bearer ' + getToken(),
                     'Content-Type': 'application/json',
                     'Access-Control-Allow-Origin': '*',
                     'Access-Control-Allow-Credentials': 'true',
                     'Access-Control-Allow-Headers': 'Origin, X-Requested-With, Content-Type, Accept'
                }
        })  
        .then(response => response.json())
        // .then(response => response.text())
        .then(response => {
                //console.log("getTable: ",response);
                dataDuration = JSON.parse(response);
                console.log("Duration: ", dataDuration);
                //let d = returnDuration(result);
        })
        .catch((err) => {
                console.log("Err: ", err)
                //return dataDuration;
        })
    
    /*
    $.post({
        url: url, //proxyurl + url
        headers: {
            'Authorization': 'Bearer ' + getToken(),
            'Content-Type': 'application/ json'
        },
        data: data,
        success: function (result) {
            console.log("predictDuration: ", result);
            let d = returnDuration(result);
            //console.log("d: ",d)
            //returnDuration(result)
        },
        error: function (err) {
            // successmessage = 'Error';
            console.log(err)
        }
    });
    */
    
}



function returnDuration(result) {
    if (result.isError === -1) {
        console.log(result.responseDuration)
        if (isRTL) { //window.qflow.isRTL
            //alert("משך מומלץ: " + result.responseDuration + "  דקות")
            //$(".display-duration").html("<p>משך מומלץ: " + result.responseDuration + "  דקות </p>");
            $(".display-duration").append("<p style=text-align:right>משך מומלץ: " + "<b>" + result.responseDuration + "</b>" + "  דקות </p>");
            //class=VAlignTop
            return result.responseDuration;
        } else {
            //alert("Duration recommendation: " + result.responseDuration + "  minutes")
            $(".display-duration").append("<p style=text-align:left>Duration recommendation: " + "<b>" + result.responseDuration + "</b>" + "  minutes </p>");
            //class=VAlignTop
            return result.responseDuration;
        }
    }
    //$(".display-duration").html(<p>....</p>)
    else {
        console.log(result.errors[0].errorDescription)
        $(".display-duration").html("<p>Error: " + result.errors[0].errorDescription + "</p>");
        return result.errors;
    }
}



/*----------------------------------------------------------------------------------------------------------------------*/ 

function getTableData() {
    let tableData = ''
    //let urlTable = 'http://p1.quze.ai:8080/api/ml/123';

    /*Option 1: */
    // fetch(urlTable, {
    //         method: 'GET', // or 'PUT'
    //         //body: data, // data can be `string` or {object}!
    //         headers:{
    //                   //'Authorization': 'Bearer ' + getToken(),
    //                   'Content-Type': 'application/json',
    //                   'Access-Control-Request-Headers': 'x-requested-with',
    //               },
    //           })
    //           .then(res => res.json())
    //           .then(response => {
    //             console.log('Success:', JSON.stringify(response));
    //             let d = returnDuration(response);
    //             console.log("d: ",d);

    //           })
    //           .catch(error => console.log('Error:', error));

    /*Option 2: */
    // let url = 'http://p1.quze.ai:8080/api/ml/123';

    //not so good, to fix it when i can and allow "Access-Control-Allow-Origin": "*"," in the server!
    const proxyurl = "https://cors-anywhere.herokuapp.com/";

    //Example fetch-to use with then:
    //const url = 'http://p1.quze.ai:8080/api/ml/123'; //"https://example.com"; =>  site that doesn’t send Access-Control-*

    const urlTable = 'http://test.quze.ai:770/api/ml/getall'
    // https://cors-anywhere.herokuapp.com/http://test.quze.ai:770/api/ml/getall
    fetch(proxyurl + urlTable) // https://cors-anywhere.herokuapp.com/https://example.com 
        .then(response => response.text())
        .then(response => {
            //console.log("getTable: ",response);
            tableData = JSON.parse(response);
            /*
            response:
            appointmentDate: "0001-01-01T00:00:00"
            appointmentID: 123
            costumerName: "Zohar"
            duration: 5
            noShow: false
            noShowProb: 0.7
            serviceTypeId: -1
            serviceTypeName: "Dancing"
            */
            console.log("getTable: ", tableData);
            getTable(tableData)
        })
        .catch(() => {
            console.log("Err, no data")
            return tableData;
        })
    //.catch(() => console.log("Can’t access " + url + " response. Blocked by browser?")) 

    /* 
    $.get({
        url: proxyurl + url,
        // useDefaultXhrHeader: false,
        //dataType: 'jsonp',
        // headers: {
        //     //'Authorization': 'Bearer ' + getToken(),
        //     'Content-Type': 'application/ json',
        //    "Access-Control-Allow-Origin": "*",
        //    'Access-Control-Allow-Credentials': 'true',
        //    'Access-Control-Allow-Headers': 'Origin, X-Requested-With, Content-Type, Accept'
        // },
        //data: data,
        success: function(result){
            //res = result;
            console.log("getTable: ",result);
            tableData = result;
            return tableData
            //returnDuration(result)
            //return result
        },
        error: function (err) {
            // successmessage = 'Error';
            console.log(err)
            return tableData
        }
    });
    */
}



function getTable(tableData) { //tableData
    // $('head').insertAdjacentHTML("efterend",
    // `<style>

    // </style>`
    // )
    let table = tableData; //parse all the object to array
    // [
    //     {
    //         appointmentDate: "0001-01-01T00:00:00",
    //         appointmentID: 123,
    //         costumerName: "Zohar",
    //         duration: 5,
    //         noShow: true,
    //         noShowProb: 0.7,
    //         serviceTypeId: -1,
    //         serviceTypeName: "Dancing"
    //     },
    //     {
    //         appointmentDate: "0001-01-01T00:00:00",
    //         appointmentID: 223,
    //         costumerName: "Dan",
    //         duration: 5,
    //         noShow: true,
    //         noShowProb: 0.8,
    //         serviceTypeId: 11,
    //         serviceTypeName: "ETN"
    //     },
    //     {
    //         appointmentDate: "0001-01-01T00:00:00",
    //         appointmentID: 223,
    //         costumerName: "Ben",
    //         duration: 5,
    //         noShow: false,
    //         noShowProb: 0,
    //         serviceTypeId: 11,
    //         serviceTypeName: "Gastro"
    //     },
    //     {
    //         appointmentDate: "0001-01-01T00:00:00",
    //         appointmentID: 223,
    //         costumerName: "Dov",
    //         duration: 5,
    //         noShow: true,
    //         noShowProb: 0.92,
    //         serviceTypeId: 11,
    //         serviceTypeName: "ETN"
    //     }
    // ]


    //console.log("Size:",table.length)
    console.log("Table:", table)
    //var body = document.getElementsByTagName("body")[0];
    var div = document.getElementsByClassName("display-table-details")[0];

    let str = [
        "Case ID",
        "Case Date",
        "Customer Name",
        "Service ID",
        "Service Name",
        "Prob of No-Show",
        "Show/No-Show",
        "Duration",
        "Notification"
    ]

    var tTable = document.createElement('TABLE');
    tTable.cellPadding = "5";
    var tblb = document.createElement('TBODY');
    tTable.appendChild(tblb);
    tblb.insertAdjacentHTML("beforeend",
        `
        <tr>
            <th>${str[0]}</th>
            <th>${str[1]}</th>
            <th>${str[2]}</th>
            <th>${str[3]}</th>
            <th>${str[4]}</th>
            <th>${str[5]}</th>
            <th>${str[6]}</th>
            <th>${str[7]}</th>
            <th>${str[8]}</th>
        </tr>`
    )
    table.forEach((val, index) => {
        //console.log("index :",index); //index = i
        //console.log("value :", val.appointmentDate);  //val = value
        var appointmentDate = val.appointmentDate;
        var timeApp = appointmentDate.split("T");

        var noShow = val.noShow;
        var noShowProb = "";
        var style = "";
        var notification = ""
        if (val.noShow === false) {
            noShow = "Show";
            noShowProb = "-";
        } else {
            noShow = "No-Show";
            if (val.noShowProb > 0.9) {
                noShowProb = "High";
                notification = true;
                style = "color:red;font-weight:bold;"
            } else if (val.noShowProb < 0.9 && val.noShowProb > 0.75) {
                noShowProb = "Medium";
                notification = false;
                style = "color:orange;font-weight:bold;"
            } else { //val.noShowProb <= 0.75
                noShowProb = "Low";
                notification = false;
                style = "color:green;font-weight:bold;"
            }
        }



        tblb.insertAdjacentHTML("beforeend",
            `<tr>
            <td>${val.appointmentID}</td>
            <td>${timeApp}</td>
            <td>${val.costumerName}</td>
            <td>${val.serviceTypeId}</td>
            <td>${val.serviceTypeName}</td>
            <td style=${style}>${noShowProb}</td>
            <td>${noShow}</td>
            <td>${val.duration}</td>
            <td>
                ${notification === false ? ' - ' : '<button onclick=promptMe() type="button">Send</button>'}
            </td>
         </tr>`);
    });


    //     for (var i = 0; i < table.length + 1; i++) {
    //     var tr = document.createElement('TR');
    //     tblb.appendChild(tr);
    //     for (var j = 0; j < 7; j++) {
    //       var td = document.createElement('TD');
    //       if(i == 0)
    // 	  {
    //         td.style.fontWeight = "bold";
    //         var textnode = document.createTextNode(str[j]);
    //         td.appendChild(textnode);
    //         tr.appendChild(td);
    //       }
    //       else {
    //         console.log(table[j])
    //         switch (j) {
    //             case 0:
    //                 //var textnode = document.createTextNode(table[j].appointmentID);
    //                 //td.appendChild(textnode);
    //                 //tr.appendChild(td);
    //                 tr.insertAdjacentHTML("beforeend", "<td>My new paragraph</p>");
    //                 break;
    //             // case 1:
    //             //     var textnode = document.createTextNode(table[j].appointmentDate );
    //             //     td.appendChild(textnode);
    //             //     tr.appendChild(td);
    //             //     break;
    //             // case 2:
    //             //     var textnode = document.createTextNode(table[j].costumerName);
    //             //     td.appendChild(textnode);
    //             //     tr.appendChild(td);
    //             //     break;
    //             // case 3:
    //             //     var textnode = document.createTextNode(table[j].serviceTypeId);
    //             //     td.appendChild(textnode);
    //             //     tr.appendChild(td);
    //             //     break;
    //             // case 4:
    //             //     var textnode = document.createTextNode(table[j].serviceTypeName);
    //             //     td.appendChild(textnode);
    //             //     tr.appendChild(td);
    //             //     break;
    //             // case 5:
    //             //     var textnode = document.createTextNode(table[j].noShowProb);
    //             //     td.appendChild(textnode);
    //             //     tr.appendChild(td);
    //             //     break;
    //             // case 6:
    //             //     var textnode = document.createTextNode(table[j].noShow);
    //             //     td.appendChild(textnode);
    //             //     tr.appendChild(td);
    //         }//switch
    //      }//else
    //   }//for_j
    //}
    //body.appendChild(tTable);
    div.appendChild(tTable);
}

function promptMe() {

    let userDetails = {
        "ServiceType": "Limud Torah",
        "FellowName": "Zohar",
        "PhoneNumber": "0508738351",
        "AppointmentTime": "2019-07-23T07:00:00",
        "AppointmentId": "229"
        //,"AppointmentTime": "2019-06-16T07:00:00"
        //"AppointmentId": "229"
     }	
    
    console.log(JSON.stringify(userDetails))
  /* 
  let user = userDetails; //JSON.parse(userDetails);
  console.log(user);
  let time = user.AppointmentTime.split("T");
  */ 


    /* TO CHECK:
        console.log(userDetails) 
        let user = JSON.parse(userDetails)
        let userAdjective = prompt("The ST is: " + user.ServiceType + 
                                    "\nThe Time is: " + user.AppointmentTime +
                                    "\nThe Appointment id is: " + user.AppointmentId +
                                    "\nPlease enter " + user.FellowName + " number:", user.PhoneNumber);
        if(userAdjective) {
            alert ("The message sent successfully");
            window.open("http://www.google.com?query="+userAdjective)
            
        }
    */

    //const aNumber = Number(window.prompt("Type a number", ""));
    //var userAdjective = prompt("Please enter the patient number:");
    //alert (userAdjective);
    //var myWindow = window.open("http://www.google.com?query="+userAdjective)
    // if (userAdjective) {
    //     alert("The message sent successfully");
    //     window.open("http://www.google.com?query=" + userAdjective)

    // }

    //console.log(myWindow);
    
    const urlIVR = 'http://localhost:51483/api/IVR/qflow' ;
    //const urlIVR = 'http://test.quze.ai:770/api/ivr/qflow';
  	///proxyurl + urlIVR
	fetch(urlIVR, {
  		method: 'POST',
      	body: JSON.stringify(userDetails), // data can be `string` or {object}!
  		headers:{
        	//'Authorization': 'Bearer ' + getToken(),
        	'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
        	'Access-Control-Request-Headers': 'x-requested-with'//,
        	}
        	
		}) 
        //.then(response => response.json())
      	.then(response => response.text())
     	.then(response => {
        	console.log(`response: ${response}`)//JSON.parse(response));
          	alert("response: " + response)
        })
     	.catch(err => {
          	console.log("Err: " + err)
          	alert("Err: " + err)
    	})
  
}

function getServiceIdAndName() {
    //document.getElementById('ctl00_MainPlaceHolder_pnlTimeSelectDate').getElementsByTagName('table')[0].tBodies[0].children[i].getElementsByTagName('td')[0].getElementsByTagName('input')[1].value
    var innerHTML = ""
    var serviceId = ""
    var serviceName = document.getElementById('ctl00_MainPlaceHolder_lblVacantTimeHeader').innerText.trim();
    var tableTr = document.getElementById('ctl00_MainPlaceHolder_pnlTimeSelectDate').getElementsByTagName('table')[0].tBodies[0];
    for (let i = 0; i < tableTr.children.length; i++) {
        innerHTML = tableTr.children[i].getElementsByTagName('td')[0].getElementsByTagName('span')[0].innerHTML;
        //console.log("innerHTML: " + innerHTML + " serviceName: " + serviceName)
        if (innerHTML === serviceName) {
            serviceId = tableTr.children[i].getElementsByTagName('td')[0].getElementsByTagName('input')[1].value;
            console.log("innerHTML: " + innerHTML + " ,serviceName: " + serviceName + " ,service id: " + serviceId)
        }
    }//for
}

// function addClickToDate() {
//     //document.getElementById('ctl00_MainPlaceHolder_pnlTimeSelectDate').getElementsByTagName('table')[0].tBodies[0].children[0].getElementsByTagName('td')[2].getElementsByTagName('a')[0].innerHTML
//     var timeText = ""
//     var tableTime = document.getElementById('ctl00_MainPlaceHolder_pnlTimeSelectDate').getElementsByTagName('table')[0].tBodies[0];
//     for (let i = 0; i < tableTime.children.length; i++) {
//         timeText = tableTime.children[i].getElementsByTagName('td')[2].getElementsByTagName('a')[0];
//         //console.log("innerHTML: " + innerHTML + " serviceName: " + serviceName)
//         if(timeText) {
//             //serviceId = tableTime.children[i].getElementsByTagName('td')[0].getElementsByTagName('input')[1].value;
//             console.log("time: " + timeText.innerHTML)


//             document.getElementById('ctl00_MainPlaceHolder_rpTimeSelectDates_ctl0' + i +'_lbCalendarDate').addEventListener("click", function(){
//                 //document.getElementById("demo").innerHTML = "Hello World";
//                 //getServiceIdAndName()
//                 alert("you clicked me")
//               });
//         }
//     }//for
//}


$(document).ready(function () {
    onChange();

    /* 
    $(document).on('change', '#ctl00_MainPlaceHolder_CustomerPanel_txtSearchPersonalId', function () {
        let fellowId = this.value;
        onChange("fellowId", fellowId);
    });

    $(document).on('change', '#ctl00_MainPlaceHolder_ddServiceType', function () {
        let serviceTypeID = this.value;
        onChange("serviceType", serviceTypeID);
    });
    */

    // $(document).on('change', '#ctl00_MainPlaceHolder_ddCompositeAppointmentType', function() {
    //     let compositeAppointmentTypeID = this.value;
    //     onChange("compositeAppointmentType" ,compositeAppointmentTypeID);
    // });

    // $(document).on('change', '#ctl00_MainPlaceHolder_rpTimeSelectDates', function() {
    //     let timeSelectDates = this.value;
    //     onChange("timeSelectDates", timeSelectDates);
    // });

    // document.getElementsByClassName('.display-table-details').append(getTable())
    getTableData();
    //getTable()

    //1. when get to this area, 2. when it changed
    $(document).on('change', '#ctl00_MainPlaceHolder_lblVacantTimeHeader', function () {
        getServiceIdAndName()


        //onChange("serviceName" ,{serviceName: serviceName, serviceId: serviceId});  

        document.getElementById('ctl00$MainPlaceHolder$cmdFindCalendars').addEventListener("click", function () {

        })
    });

});


/*
$(document).on('change', '#ctl00_MainPlaceHolder_ddServiceType', function() {
		//console.log($(this).find("option:selected").text())
		//console.log(this.HTMLDocument())
        //console.log("ServiceType"); //+ ServiceType value
		let serviceID = this.value;
        console.log(serviceID)
  });

----------------------------------------------------------------------

//const urlIVR = 'http://test.quze.ai/dev/api/v1.0/IVR/qflow'; -> old IVR

//const url = 'http://p1.quze.ai:8080/api/ml/123'; -> get one duration(?) - old


//reloadDelegates() -> this is a function that load the page??

//let val = $('#ctl00_cphMain_lblServiceVal1')//.val();[0].textContent || [0].innerHTML
//console.log("val: " + val)

//let val = $('#ctl00_cphMain_pnlNewAppointment')//.val();[0].textContent || [0].innerHTML
//console.log("val: " + val)

//let val = $('#ctl00_cphMain_pnlAppointmentBody')//.val();[0].textContent || [0].innerHTML
//console.log("val: " + val)

the old http request:
    var data = JSON.stringify(getData());

    $.post({
        url: url,
        headers: {
            'Authorization': 'Bearer' + getToken(),
            'Content-Type': 'application/json',
          'Access-Control-Request-Headers': 'x-requested-with',
          'Access-Control-Allow-Origin': '*'
          //'Access-Control-Allow-Credentials': true
        },
      crossDomain: true,
      xhrFields:{withCredentials:true},

        data: data, // data can be `string` or {object}!
        success: function(result){
            console.log("success: ",result);
            let d = returnDuration(result);
            console.log("d: ",d)
        },
        error: function (err) {
            // successmessage = 'Error';
            console.log(err)
        }
    });

    //if the user have Email the id IS NOT "...ctl05..." but "...ctl06..."
    var userPhone = document.getElementById('ctl00_cphMain_CustomerPanel_rpDetails_ctl05_lblValue');
    if (userPhone != null)
      	console.log(userPhone.innerHTML)//.innerHTML

----------------------------------------------------------------------
 window.currentUserId = 2;
 window.currentUserGroupId = 1;
 window.qflow = window.qflow || {};
 window.qflow.currentCultureName = 'he_IL';
 window.qflow.dateTimeFormat = {
    "AMDesignator":"AM",
    "Calendar":
    {
        "MinSupportedDateTime":"0001-01-01T00:00:00",
        "MaxSupportedDateTime":"9999-12-31T23:59:59.9999999",
        "AlgorithmType":1,
        "CalendarType":1,
        "Eras":[1],
        "TwoDigitYearMax":2029,
        "IsReadOnly":true
    },
    "DateSeparator":"/",
    "FirstDayOfWeek":0,
    "CalendarWeekRule":0,
    "FullDateTimePattern":"dddd dd MMMM yyyy HH:mm:ss",
    "LongDatePattern":"dddd dd MMMM yyyy",
    "LongTimePattern":"HH:mm:ss",
    "MonthDayPattern":"dd MMMM",
    "PMDesignator":"PM",
    "RFC1123Pattern":"ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
    "ShortDatePattern":"dd/MM/yyyy",
    "ShortTimePattern":"HH:mm",
    "SortableDateTimePattern":"yyyy'-'MM'-'dd'T'HH':'mm':'ss",
    "TimeSeparator":":",
    "UniversalSortableDateTimePattern":"yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
    "YearMonthPattern":"MMMM yyyy",
    "AbbreviatedDayNames":["יום א","יום ב","יום ג","יום ד","יום ה","יום ו","שבת"],
    "ShortestDayNames":["א","ב","ג","ד","ה","ו","ש"],
    "DayNames":["יום ראשון","יום שני","יום שלישי","יום רביעי","יום חמישי","יום שישי","שבת"],
    "AbbreviatedMonthNames":["ינו","פבר","מרץ","אפר","מאי","יונ","יול","אוג","ספט","אוק","נוב","דצמ",""],
    "MonthNames":["ינואר","פברואר","מרץ","אפריל","מאי","יוני","יולי","אוגוסט","ספטמבר","אוקטובר","נובמבר","דצמבר",""],
    "IsReadOnly":true,
    "NativeCalendarName":"לוח שנה גרגוריאני",
    "AbbreviatedMonthGenitiveNames":["ינו","פבר","מרץ","אפר","מאי","יונ","יול","אוג","ספט","אוק","נוב","דצמ",""],
    "MonthGenitiveNames":["ינואר","פברואר","מרץ","אפריל","מאי","יוני","יולי","אוגוסט","ספטמבר","אוקטובר","נובמבר","דצמבר",""]
};
window.qflow.currentCulture = 'he-IL';
window.qflow.currentUICulture = 'he-IL';
window.qflow.isRTL = 'True' == 'True';

//window.qflow.menuItems -> list of the navbar


*/
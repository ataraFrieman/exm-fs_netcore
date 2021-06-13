
export function getBranchModalData(branch, streets, cities, addressChanged) {
    return [{
        cols: ["col-md-6", "col-md-6"]
        , properties:
            [
                {
                    label: "שם",
                    type: "text",
                    bsclass: "form-control",
                    placeholder: "שם הסניף",
                    name: "name",
                    value: branch.name
                },
                {
                    label: "טלפון",
                    type: "text",
                    bsclass: "form-control",
                    placeholder: "טלפון",
                    name: "phonNumber",
                    value: branch.phonNumber
                }
            ]
    },
    {
        cols: ["col-md-6"]
        , properties:
            [
                {
                    label: "מייל",
                    type: "email",
                    bsclass: "form-control",
                    placeholder: "מייל",
                    name: "emailAddress",
                    value: branch.emailAddress
                }
            ]
    },
    {
        cols: ["col-md-12"]
        , properties:
            [
                {
                    label: "כתובת",
                    type: "address",
                    bsclass: "form-control",
                    name: "address",
                    street: branch.street,
                    houseNumber: branch.houseNumber,
                    streets: streets,
                    cities: cities,
                    cityVal: branch.street.cityId,
                    streetVal: branch.street.id,
                    saveEvent: addressChanged
                }
            ]
    }];
}



export function getServiceProviderModalData(serviceProvider, serviceTypes,t) {
    return [{
        cols: ["col-md-6", "col-md-6"]
        , properties:
            [
                {
                    label:t("sProvid.firstname"),
                    type:"text",
                    bsclass: "form-control",
                    placeholder:t("sProvid.firstname"),
                    name: "firstName",
                    value: serviceProvider.firstName
                },
                {
                    label:t("sProvid.lastname"),
                    type: "text",
                    bsclass: "form-control",
                    placeholder:t("sProvid.lastname"),
                    name: "lastName",
                    value: serviceProvider.lastName
                }
            ]
    },
    {
        cols: ["col-md-6"]
        , properties:
            [
                {
                    label:t("sProvid.id"),
                    type: "text",
                    bsclass: "form-control",
                    placeholder:t("sProvid.id"),
                    name: "identityNumber",
                    value: serviceProvider.identityNumber
                }
            ]
    },
    {
        cols: ["col-md-6"]
        , properties:
            [
                {
                    label:t("sProvid.Serviceprovider"),
                    type: "checkBoxesList",
                    bsclass: "form-control",
                    placeholder:t("sProvid.Serviceprovider"),
                    name: "identityNumber",
                    serviceTypes: serviceTypes,
                    value: serviceProvider.serviceProvidersServiceTypes,
                    serviceProviderId: serviceProvider.id
                }
            ]
    }
        //    ,
        //{
        //    cols: ["col-md-6"]
        //    , properties:
        //        [
        //            {
        //                label: "התמחות",
        //                type: "text",
        //                bsclass: "form-control",
        //                placeholder: "התמחות",
        //                name: "serviceType",
        //                defaultValue: serviceProvider.serviceType
        //            }
        //        ]
        //    }
    ];
}

export function getFellowModalData(fellow,t) {
    return [{
        cols: ["col-md-6", "col-md-6"]
        , properties:
            [
                {
                    label: t("Mdata.firstname"),
                    type: "text",
                    bsclass: "form-control",
                    placeholder: t("Mdata.firstname"),
                    name: "firstName",
                    value: fellow.firstName
                },
                {
                    label:t("Mdata.lastname"),
                    type: "text",
                    bsclass: "form-control",
                    placeholder:t("Mdata.lastname"),
                    name: "lastName",
                    value: fellow.lastName
                }
            ]
    },
    {
        cols: ["col-md-6", "col-md-6"]
        , properties:
            [
                {
                    label: t("Mdata.id"),
                    type: "number",
                    bsclass: "form-control",
                    placeholder:t("Mdata.id"),
                    name: "identityNumber",
                    value: fellow.identityNumber
                },
                {
                    label: t("Mdata.phone"),
                    type: "number",
                    bsclass: "form-control",
                    placeholder:t("Mdata.phone"),
                    name: "phone",
                    value: fellow.phone
                }
            ]
    }
    ];
}

export function getData(queue, branches, serviceTypes, serviceProviders, serviceStations) {
    return [{
        cols: ["col-md-6", "col-md-6"]
        , properties:
            [
                {
                    label: " התחלה",
                    type: "time",
                    bsclass: "form-control",
                    placeholder: "זמן התחלה",
                    name: "beginTime",
                    value: queue.beginTime
                },
                {
                    label: "סיום",
                    type: "time",
                    bsclass: "form-control",
                    placeholder: "סיום",
                    name: "endTime",
                    value: queue.endTime
                }
            ]
    },
    {
        cols: ["col-md-6"]
        , properties:
            [
                {
                    label: "סניף",
                    type: "select",
                    bsclass: "form-control",
                    placeholder: "סניף",
                    name: "branchId",
                    defaultValue: queue.branchId,
                    options: branches,
                    optionName: "name"
                }
            ]
    }
        ,
    {
        cols: ["col-md-6", "col-md-6"]
        , properties:
            [
                {
                    label: "שרות",
                    type: "select",
                    bsclass: "form-control",
                    placeholder: "שרות",
                    name: "serviceType",
                    defaultValue: queue.serviceTypeId,
                    options: serviceTypes,
                    optionName: "description"
                },
                {
                    label: "נותן שרות",
                    type: "select",
                    bsclass: "form-control",
                    placeholder: "סניף",
                    name: "serviceProviderId",
                    defaultValue: queue.serviceProviderId,
                    options: serviceProviders,
                    optionName: "titledFullName"
                }
            ]
    }
    ];


}

//export function getTimaTableModalData(TT, branches) {
//    return [{
//        cols: ["col-md-6", "col-md-6"]
//        , properties:
//            [
//                {
//                    label: "תקף מתאריך",
//                    type: "date",
//                    bsClass: "form-control",
//                    placeholder: "מתאריך",
//                    name: "validFromDate",
//                    value: new Date(TT.validFromDate).toJSON().slice(0, 10),
//                    mandatory: true
//                },
//                {
//                    label: "עד תאריך",
//                    type: "date",
//                    bsClass: "form-control",
//                    placeholder: "עד תאריך",
//                    name: "validUntilDate",
//                    value: new Date(TT.validUntilDate).toJSON().slice(0, 10),
//                    mandatory: true
//                }

//            ]
//    },
//        {
//            cols: ["col-md-12"]
//            , properties:
//                [
//                    {
//                        label: "סניף",
//                        type: "select",
//                        bsClass: "form-control",
//                        placeholder: "סניף",
//                        name: "branchId",
//                        value: TT.branchId,
//                        options: branches,
//                        optionName: "name",
//                        mandatory: true
//                    }

//                ]
//        },
//        {
//        cols: ["col-md-6", "col-md-6"]
//        , properties:
//            [
//               {
//                    label: "משעה",
//                    type: "time",
//                    bsClass: "form-control",
//                    placeholder: "משעה",
//                    name: "beginTime",
//                    value: TT.timeTableLines[0].beginTime,
//                    mandatory: true
//                },
//                 {
//                    label: "עד שעה",
//                    type: "time",
//                    bsClass: "form-control",
//                    placeholder: "עד שעה",
//                    name: "endTime",
//                     value: TT.timeTableLines[0].endTime,
//                     mandatory: true
//                }

//            ]
//    }];
//}

export function getTimaTableModalData(TT, branches) {
    return [
        {
            cols: ["col-md-6", "col-md-6"]
            , properties:
                [
                    {
                        label: "תקף מתאריך",
                        type: "date",
                        bsclass: "form-control",
                        placeholder: "מתאריך",
                        name: "validFromDate",
                        value: TT.validFromDate
                    },
                    {
                        label: "עד תאריך",
                        type: "date",
                        bsclass: "form-control",
                        placeholder: "עד תאריך",
                        name: "validUntilDate",
                        value: TT.validUntilDate
                    }


                ]
        },
        {
            cols: ["col-md-12"]
            , properties:
                [
                    {
                        label: "סניף",
                        type: "select",
                        bsclass: "form-control",
                        placeholder: "סניף",
                        name: "branchId",
                        value: TT.branchId,
                        options: branches,
                        optionName: "name",
                        mandatory: true
                    }

                ]
        },
        {
            cols: ["col-md-6", "col-md-6"]
            , properties:
                [
                    {
                        label: "משעה",
                        type: "time",
                        bsclass: "form-control",
                        placeholder: "משעה",
                        name: "beginTime",
                        value: TT.timeTableLines[0].beginTime,
                        mandatory: true
                    },
                    {
                        label: "עד שעה",
                        type: "time",
                        bsclass: "form-control",
                        placeholder: "עד שעה",
                        name: "endTime",
                        value: TT.timeTableLines[0].endTime,
                        mandatory: true
                    }

                ]
        }];
}
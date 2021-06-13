import React, { Component } from "react";
import { Row, Col } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export class ServiceTypesCards extends Component {
    constructor(props) {
        super(props);
        this.state = {
            serviceTypesList: props.serviceTypes ? props.serviceTypes : []
        };
        console.log(props.serviceTypes)
    }
    getIconName(MinimalKitType) {
        switch (MinimalKitType) {
          
            case "מסמך":
                return "file";
            case "כרטיס":
                return "credit";
            case "בדיקה":
                return "eyedropper";
            default:
                return "file";
        }
    }

    //minimalKitRules -> to what it's referes
    
    render() {
        return( <div className="row">
            {this.state.serviceTypesList.map((item) => {
                return item && item.parentServiceId && item.parentServiceId > 1 ?
                    <div className="card col-3 m-3" key={item.id}>
                    <div className="card-body">
                        <h6 className="card-subtitle mb-4 font-pr text-muted text-center fs-16  basicColorBlue">{item.description}</h6>
                        <h6 serviceType="card-subtitle  text-muted text-right mb-3">מסמכים /דרישות חובה:</h6>
                            {item && item.minimalKitRules?item.minimalKitRules.map((mkr) => {
                            return <div className="row">
                                <div className=" col-2 p-0 ">
                                    <i className={"fs-24 pe-7s-"+this.getIconName(mkr.type)}/>

                                </div>
                                <span className=" col-10 p-0 text-right text-muted fs-12">{mkr.description}</span>
                            </div>;
                        }):""}
                    </div>
                </div>:"";
            })}
        </div>)

    }
};
export default ServiceTypesCards;

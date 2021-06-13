import React, { Component } from 'react'
import '../css/FellowDetails.css'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'


class FellowDetails extends Component {
    constructor(props) {
        super(props);
        console.log(props);
        this.state = {
            fellowName: props.location.state.name, 
            fellowId: props.location.state.id,
            FellowType: props.location.state.type 
        }
    }

    componentDidMount () {
        //call server to get the fellow details

    }

    render() {
        return (
        <div className="w-100vw">
            <div className="d-flex p-2">
                <div>{this.state.fellowName}</div>
                <br/>
                <div> {this.state.FellowType} - {this.state.fellowId}</div>
                {/* id + age  */}
            </div>
            <ul>
                <li className="example1 d-flex p-2">
                    <span><FontAwesomeIcon icon="angle-left"/></span>
                    {/* <FontAwesomeIcon icon="angle-up"/> */}
                    מלווה
                </li>
                <li className="example1 d-flex p-2">
                    <FontAwesomeIcon icon="angle-left"/>
                    אפוטרופוסים
                </li>
                <li className="example1 d-flex p-2">
                    <FontAwesomeIcon icon="angle-left"/>
                    פרטי קשר
                </li>
                <li className="example1 d-flex p-2">
                    <FontAwesomeIcon icon="angle-left"/>
                    זימונים
                </li>
                <li className="example1 d-flex p-2">
                    <FontAwesomeIcon icon="angle-left"/>
                    רגישות רפואית
                </li>
                <li className="example1 d-flex p-2">
                    <FontAwesomeIcon icon="angle-left"/>
                    הערות
                </li>
                <li className="example1 d-flex p-2">
                    <FontAwesomeIcon icon="angle-left"/>
                    ארכיון
                </li>
            </ul>
            
        </div>
        )
    }
}

export default FellowDetails

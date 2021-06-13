import React, { Component } from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import '../css/AddQbutton.css'

class AddQbutton extends Component {

    render() {
        var t=this.props.t;
        return (
            <div >

                <nav className="Qbuttons addNav"  >
{/* יש לשנות את הסי אס אס לימין ושמאל באנגלית */}
                    <div className="smbuttons" tooltip={t("AddQ.follow")}>
                        <FontAwesomeIcon className="smbuttons" icon='users' />
                    </div>
                    <div className="smbuttons" tooltip={t("AddQ.equipment")} >
                        <FontAwesomeIcon className="smbuttons" icon='briefcase-medical' />
                    </div>

                    <div className="smbuttons" tooltip={t("AddQ.room")}>
                        <FontAwesomeIcon className="smbuttons" icon='procedures' />
                    </div>

                    <div className="smbuttons" tooltip={t("AddQ.crewMember")} >
                        <FontAwesomeIcon className="smbuttons" icon='user-md' />
                    </div>


                    <div className="smbuttons" tooltip={t("AddQ.queue")}>
                        <FontAwesomeIcon className="smbuttons" icon='hospital' />
                    </div>


                    <div className="mainButton" tooltip={t("AddQ.add")}>
                        <FontAwesomeIcon className="mainButton" icon='plus' />
                    </div>
                </nav>
            </div>
        )
    }
}

export default AddQbutton

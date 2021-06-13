import React, { Component } from 'react'
import '../css/Mission.css'

class Mission extends Component {
    
    render() {
        var t=this.props.t;
        return (



            <div className='card-container'>
                <div className='card-mission'>
                    <div className='mission-title'>
                        <span>Moishy</span>
                    </div>

                    
                    <div className='mission-content'>
                        <span>משה יעקובביץ</span>
                        <br />
                        <span>מאוחדת</span>
                        <br />
                        <span>מאוחדת</span>
                    </div>
                   
                    <button className='btn btn-outline-dark col-3 p-0 rounded'>{t("Mission.reject")}</button>
                    <button className='btn btn-outline-dark col-3 p-0 rounded'>{t("Mission.perform")}</button>
                    </div>
                </div>
        )
    }
}



export default Mission;
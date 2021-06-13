import React, { Component } from 'react';
import history from '../helpers/History';
import HeaderTitlePage from './HeaderTitlePage';
import "./../css/Home.css"
export class Home extends Component {
    displayName = Home.name

    render() {
        return <div className="">


                        <HeaderTitlePage title="Welcome"/>

            <div className="row h-25 align-items-center justify-content-center p-2 fixRow">
                <img src={require('../pictures/red-logo.png')} className="center-block loginLogo" alt="red-logo" />
            </div>

            <div className=" row p-2 m-0 align-items-end justify-content-center">
                <h1 className="QuzeTitle">Quze</h1></div>
            <div className=" row p-2 m-0 align-items-end justify-content-center">

                <h1 className="QuzeTitle fs-24">Queueu managament for the 21'st century</h1>
            </div>
            <div className="row p-2 h-25  justify-content-center align-items-center fixRow">
                <div className="col-6">
                    <button className="btn col-6  " onClick={e => { history.push("/login?isNew=true"); window.location.reload(); }}>ארגון חדש</button>
                    <button className="btn col-6 " onClick={e => { history.push("/login"); window.location.reload(); }}>כניסה</button>
                </div>
            </div>
        </div>;
    }
}

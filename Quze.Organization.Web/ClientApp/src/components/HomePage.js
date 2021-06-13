import React, { Component } from "react";
import CurrentQueues from './CurrentQuze';
import Mission from './Mission';
import { withRouter } from 'react-router';
// import { getDateYYYMMDD, getTime } from '../helpers/TimeService';
// import { CurrentDate } from './time/CurrentDate';
// import { CurrentTime } from './time/CurrentTime';
// import AutoCompleteSearch from './task/AutoCompleteSearch'
import { getUser } from '../helpers/User';
import '../css/homePage.css';
import { observer } from "mobx-react";
import { withI18n } from "react-i18next";

class HomePage extends Component {
    constructor(props) {
        super(props);
        this.state = {
            user: [],
            closedQueues: []
        };
    }

    componentWillMount() {
        let user = getUser();
        if (user) {
            user.branchId = 2;
            this.setState({ user: user });
        }
    }

    render() {
       
        var t = this.props.t;
        return (
            <div>
                <div className="flex-container">
                    <CurrentQueues t={this.props.t} branchId={this.state.user.branchId} QueuesList={this.props.QueuesList} closedQueues={this.state.closedQueues} />

                </div>

                <div className='text-align'>
                    {//<h3>{t("HomePage.taskSay")}</h3>
                    }
                    <div className='mission-container'>

                        {
                            //<Mission t={this.props.t} />
                        }
                        {/* <Mission /> */}

                    </div>
                </div>

            </div>

        );
    }
}

export default withRouter(withI18n()(observer(HomePage)));


import React, { Component } from 'react';
import { NavMenu } from './NavMenu';
import HeaderPage from './HeaderPage'
import AddQ from './AddQbutton';
import { getUser } from '../helpers/User'

export class Layout extends Component {
    displayName = Layout.name;
    state = {
        id: null,
        showWrapperNav: false
    }

    componentWillMount() {
        let user = getUser();
        if (user) {
            this.setState({ id: user.id });
        }
    }

    //TODO: in future build logic that detect the language
    getDir = () => { //ltr or rtl 
        let dir = "ltr";
        dir = this.props.language == "he" ? "rtl" : "ltr";
        return dir
    }

    showWrapperNav = () => {
        this.setState({ showWrapperNav: !this.state.showWrapperNav })
    }
    render() {
        return <div className="h-100" dir={this.getDir()}>
            <div className="row m-0 p-0 w-100">
                {
                    this.state.id ? <div className="w-100"> <HeaderPage t={this.props.t}
                        language={this.props.language}
                        setLanguage={this.props.setLanguage}
                        showWrapperNav={this.showWrapperNav}
                    /></div> : ""
                }
            </div>
           
            <div className="wrapperNav " >
            {this.state.showWrapperNav?
                <NavMenu t={this.props.t} />:""}
                <div className='contant-page '>

                    {this.props.children}
                </div>
                {/* {this.state.id ? <AddQ t={this.props.t}/> : ""} */}
            </div>

        </div>;

    }
}

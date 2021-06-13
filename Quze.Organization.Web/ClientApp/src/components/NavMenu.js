import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import '../css/navMenu.css'
import Logo from '../pictures/red-logo.png'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import history from '../helpers/History'
import { getOrganization, logout, isUserAuthenticated } from '../helpers/User'

export class NavMenu extends Component {
    displayName = NavMenu.name
    state = {
        //organization: localStorage.getItem('organization'),
        orgName: "",
        b64: "",
        userName: "",
        isLoggedIn: isUserAuthenticated(),

    };


    componentDidMount() {
        // console.log(props);
        //let getOrganization = localStorage.getItem('organization');
        let organizationDetails = getOrganization()
        //console.log("before parse:" + getOrg);
        //let organizationDetails = JSON.parse(getOrganization); 
        //console.log("after parse:" + org)
        // console.log(organizationDetails) -> good
        if (organizationDetails == null) {
            return
        } else {

            this.setState({
                orgName: organizationDetails.Name,
                b64: organizationDetails.Icon,
                userName: organizationDetails.userName,
                addClass: false, //false = it's Hebrew 
                showLanguages: false  //false = don'n show the option to select the languages
                // isLoggedIn: organizationDetails.loggedIn
            });
            //console.log("after parse:" + getOrg);
            //let img = atob(this.state.b64);
            //console.log(img);
            // var b = new Buffer(organizationDetails.iconOrganization, 'base64')
            // var s = b.toString();
            // console.log(s);
        }

    }

    openMenu = (e) => {
        if (e.target == e.currentTarget || e.currentTarget.childNodes[0] == e.target)
            this.setState({ showLanguages: !this.state.showLanguages })
    }

    isEnglishOrHebrew = (e) => {

        //console.log(e.target.id) -> good
        if (e.target.id == "English") {
            this.setState({ addClass: true });//from false to true and the opposite
            localStorage.setItem('Language', 'en');
            //setLanguage();
            return;

        } else {
            this.setState({ addClass: false });
            localStorage.setItem('Language', 'he');
            //setLanguage();
            return;
        }

    }

    selectedLinkClass(href) {
        if (href == "languages")
            return "  ";
        if (window.location.pathname == ("/" + href))
            return " active ";
        else
            return "";
    }
    render() {

        var t = this.props.t;
        var logo = localStorage.getItem("organization");
        if (logo)
            logo = JSON.parse(logo);
        return (this.state.isLoggedIn ?
            <div className="">
                <nav className="sideBarNav">
                    
                    <div id="sidebar">
                        <ul className="list-group list-group-nav list-group-flush list-group-flush-nav p-2">
                            <li className="list-group-item list-group-item-nav list-group-item-action border-0 text-align text-align" >
                                {/* <Link className="list-group-item list-group-item-nav m-0 p-0 border-0" to="homePage"> */}
                                <Link className="list-group-item list-group-item-nav m-0 p-0 border-0" to="surgery">
                                    <div className="row m-0 p-0">
                                        <i className="fs-24 pe-7s-home col-3 basicColorBlue m-0 p-0"></i>
                                        {/* <span className={"switchLeng" + this.selectedLinkClass("homePage")}>{t("navMenu.home")}</span> */}
                                        <span className={"switchLeng" + this.selectedLinkClass("surgery")}>{t("navMenu.surgery")}</span>
                                    </div>
                                </Link>
                            </li>

                            <li className="list-group-item list-group-item-nav list-group-item-action border-0 text-align"  >
                                <Link className="list-group-item list-group-item-nav m-0 p-0 border-0" to="users">
                                    <div className="row m-0 p-0">
                                        <i className="fs-24 pe-7s-user col-3 basicColorBlue m-0 p-0"></i>
                                        <span className={"switchLeng" + this.selectedLinkClass("users")}>{t("navMenu.users")}</span>
                                    </div>
                                </Link>
                            </li>

                            {/* <li className="list-group-item list-group-item-nav list-group-item-action border-0 text-align">
                                <Link className="list-group-item list-group-item-nav m-0 p-0 border-0" to="branches">
                                    <div className="row m-0 p-0">
                                        <i className="fs-24 pe-7s-way col-3 basicColorBlue m-0 p-0"></i>
                                        <span className={"switchLeng" + this.selectedLinkClass("branches")}>{t("navMenu.branches")}</span>
                                    </div>
                                </Link>
                            </li>*/}

                            <li className="list-group-item list-group-item-nav list-group-item-action border-0 text-align" >
                                <Link className="list-group-item list-group-item-nav m-0 p-0 border-0" to="fellows">
                                    <div className="row m-0 p-0">
                                        <i className="fs-24 pe-7s-credit col-3 basicColorBlue m-0 p-0"></i>
                                        <span className={"switchLeng" + this.selectedLinkClass("fellows")} >{t("navMenu.fellows")}</span>
                                    </div>
                                </Link>
                            </li>

                            <li className="list-group-item list-group-item-nav list-group-item-action border-0 text-align">
                                <Link className="list-group-item list-group-item-nav m-0 p-0 border-0" to="serviceProviders">
                                    <div className="row m-0 p-0">
                                        <i className="fs-24 pe-7s-id col-3 basicColorBlue m-0 p-0"></i>
                                        <span className={"switchLeng" + this.selectedLinkClass("serviceProviders")} >{t("navMenu.serviceP")}</span>
                                    </div>
                                </Link>
                            </li>


                            {/*<li className="list-group-item list-group-item-nav list-group-item-action border-0 text-align">
                                <Link className="list-group-item list-group-item-nav m-0 p-0 border-0" to="SelectServiceType">
                                    <div className="row m-0 p-0">
                                        <i className="fs-24 pe-7s-eyedropper col-3 basicColorBlue m-0 p-0"></i>
                                        <span className={"switchLeng" + this.selectedLinkClass("SelectServiceType")}> {t("navMenu.serviceT")}</span>
                                    </div>
                                </Link>
                            </li>*/}


                            {/*<li className="list-group-item list-group-item-nav list-group-item-action border-0 text-align">
                                <Link className="list-group-item list-group-item-nav m-0 p-0 border-0" to="queues">
                                    <div className="row m-0 p-0">
                                        <i className="fs-24 pe-7s-airplay col-3 basicColorBlue m-0 p-0"></i>
                                        <span className={"switchLeng" + this.selectedLinkClass("queues")}>{t("navMenu.manageQ")}</span>
                                    </div>
                                </Link>
                            </li>*/}
                            {
                                //{<li className="list-group-item list-group-item-nav list-group-item-action border-0 text-align">
                                //    <Link className="list-group-item list-group-item-nav m-0 p-0 border-0" to="tasks">
                                //        <span className={"switchLeng" + this.selectedLinkClass("tasks")}>{t("navMenu.tastks")}</span>
                                //    </Link>
                                //</li>
                            }

                            {/*<li className="list-group-item list-group-item-nav list-group-item-action border-0 text-align">
                                <Link className="list-group-item list-group-item-nav m-0 p-0 border-0" to="schedual">
                                    <div className="row m-0 p-0">
                                        <i className="fs-24 pe-7s-date col-3 basicColorBlue m-0 p-0"></i>
                                        <span className={"switchLeng" + this.selectedLinkClass("schedual")}>{t("navMenu.schedual")}</span>
                                    </div>
                                </Link>
                            </li>*/}
                            {/* <li className="list-group-item list-group-item-nav list-group-item-action border-0 text-align">
                                <Link className="list-group-item list-group-item-nav m-0 p-0 border-0" to="surgery">
                                    <div className="row m-0 p-0">
                                        <i className="fs-24 pe-7s-timer col-3 basicColorBlue m-0 p-0"></i>
                                        <span className={"switchLeng" + this.selectedLinkClass("surgery")}>{t("navMenu.surgery")}</span>
                                    </div>
                                </Link>
                            </li> */}
                        </ul>



                        <div className="logo-down">
                            <h6>{t("navMenu.Poweredby")} <img src={Logo} width="30" height="30" alt="logo" /></h6>
                        </div>

                    </div>

                </nav>
            </div>


            :
            ""
            // <div>not loggedin</div>
        );
    }//render
}

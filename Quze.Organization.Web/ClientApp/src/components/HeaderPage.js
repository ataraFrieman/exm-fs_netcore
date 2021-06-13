import React, { Component } from 'react'
import CurrentQueue from './CurrentQuze'
// import * as http from '../helpers/Http';
import CurrentDate from './time/CurrentDate'
import CurrentTime from './time/CurrentTime'
import AutoCompleteSearch from './task/AutoCompleteSearch'
import Logo from '../pictures/red-logo.png'
import { logout, getOrganization, getUser } from '../helpers/User'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import '../css/navMenu.css'
import $ from 'jquery'
import ReactFlagsSelect from 'react-flags-select';
import 'react-flags-select/css/react-flags-select.css';

class HeaderPage extends Component {
    constructor(props) {
        super(props);
        this.state = {
            user: [],
            // orgName: "",
            b64icon: "",
            showIcon: false,
            title: "",
            count: 0
            // addQ: false,
            // addProp: false

        };
        this.getCountry = this.getCountry.bind(this);
        this.onSelectLan = this.onSelectLan.bind(this);
    }

    componentWillMount() {
        let user = getUser();
        if (user) {
            // user = JSON.parse(user);
            user.branchId = 2;  // ??????
            this.setState({ user: user });
        }

        let organization = getOrganization()
        if (organization === null) {
            return
        } else {
            this.setState({
                // orgName: organization.Name,
                b64icon: organization.Icon
            });
        }
    }

    handleClick = () => {
        var t = this.props.t;
        //To fix the bug
        let showIcon = this.state.showIcon
        let title = this.state.title
        let count = this.state.count
        count = count + 1
        this.setState({ count: count })
        if (!showIcon)
            //t(..."openMenu")
            title = t("AutoSearch.closemenu")
        else
            //t(..."closeMenu")
            title = t("AutoSearch.openmenu")
        if (count !== 0 && count % 2 === 0) {
            this.setState({
                showIcon: !showIcon,
                title: title
            })
        }


        /*.on => click 1 times but works in the second click*/
        // $('#sidebarCollapse').one('click', () => {
        //     $('#sidebar').toggleClass('active');
        // });  

        /*.on => click 2 times*/
        // $('#sidebarCollapse').on('click', () => {
        //     $('#sidebar').toggleClass('active');
        // });  

        $('#sidebarCollapse').on('click', () => {
            $('#sidebar').toggleClass('active');
        });


    }

    // ChangeIcon = () => {

    // }
    onSelectLan(selectedCountry) {
        var lan = selectedCountry === "IL" ? "he" : "en";
        this.props.setLanguage(lan);

    }
    getCountry() {
        var lan = "US";
        switch (this.props.language) {
            case "he" || "HE":
                lan = "IL"; break;
            case "en" || "EN":
                lan = "US"; break;

        }
        console.log(lan);
        return lan;
    }
    getDir = () => { //ltr or rtl 
        let dir = "ltr";
        dir = this.props.language === "he" ? "rtl" : "ltr";
        return dir
    }

    render() {
        var t = this.props.t;
        return (
            <div>
                <div className="headerPage row m-0 mb-3" dir={this.getDir()}>
                    <div className="navbar navbar-light col-1 pt-0">
                        <button className="navbar-toggler" type="button" aria-expanded="false" onClick={()=>this.props.showWrapperNav()}>
                            <span className="navbar-toggler-icon"></span>
                        </button>
                    </div>
                    <div className="right col-1 p-0 mt-1">
                        <span className="mr-2 ml-2"><CurrentTime className="time" /> </span>
                    </div>

                    <div className="right col-1 p-0 mt-1">
                        <h5 className="mr-2 ml-2 basicColorBlue">RAMBAM</h5>
                    </div>
                    {/* center side */}
                    <div className="search col-6 p-0 mt-1 mb-2">
                        <AutoCompleteSearch placeholder={this.props.t("AutoSearch.placeHolder")} />
                    </div>

                    {/* left side */}
                    <div className='left col-3'>
                        <div className="row ">
                            <div className="headerLanSelect col-4">
                                <ReactFlagsSelect
                                    countries={["US", "IL"]} showSelectedLabel={false}
                                    showOptionLabel={false}
                                    selectedSize={14} defaultCountry={this.getCountry()} onSelect={this.onSelectLan} />
                            </div>


                            <div className=" col-8 row m-0 p-0  justify-content-start">

                                <i className=' fs-36 pe-7s-user' />
                                <div className="dropdown  col-7 m-0 p-0 row font-pr">
                                    <button className="btn  dropdown-toggle w-100 m-0" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                        {this.state.user && this.state.user.fullName ? this.state.user.fullName : " "}
                                    </button>
                                    <div className="dropdown-menu" aria-labelledby="dropdownMenuButton">
                                        <button className="btn btn-light w-100" data-toggle="modal" data-target="#logoutModal">{t("AutoSearch.Logout")}</button>
                                        <button className="btn btn-light w-100"><a className="dropdown-item bg-t" href="profile">{t("AutoSearch.EditProfile")}</a></button>
                                    </div>


                                </div>
                            </div>

                        </div>
                    </div>

                    {/* modal */}
                    <div className="modal fade" id="logoutModal" tabIndex="-1" role="dialog" aria-hidden="true" aria-labelledby="modalLabel" data-backdrop={false}>
                        <div className="modal-dialog modal-sm ">
                            <div className="modal-content ">
                                <div className="modal-header">
                                    <h4 className="modal-title" id="modalLabel">{t("AutoSearch.exit")}</h4>
                                    <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">×</span>
                                    </button>
                                </div>
                                <div className="modal-body ">
                                    {/* t(...logoutMessage) */}
                                    <p className="">{t("AutoSearch.Areyousureyouwanttoexit")}</p>
                                    <div className="actionsBtns">
                                        <form className="worn">
                                            <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={logout}>{t("AutoSearch.exit")}</button>
                                            <button type="button" className="btn btn-danger" data-dismiss="modal">{t("AutoSearch.cancel")}</button>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>

            </div>
        )
    }

}

export default HeaderPage

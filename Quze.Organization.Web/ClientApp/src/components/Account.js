import React, { Component } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import "../css/site.css"
import "../css/general.css"
import { withRouter } from 'react-router-dom';
import { setLogin } from '../helpers/User';
import HeaderTitlePage from './HeaderTitlePage';


export class Account extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isRegister: false,
            isLoading: false,
            isStepB: false,
            isStepBSucceed: false,
            phone:"",
            id:"",
            password:"",
            errorMessageStepB: "",
            isInvalidIdNumber: false,
            isInvalidPhoneNumber: false,
            isUserAuthenticated: false,
            // organizationName: "" //add
        };
    }

    isValidIsraeliID(idNum) {
        return true;
        let id = String(idNum).trim();
        let intId = parseInt(id);
        if (id.length > 9 || id.length < 5 || !intId || isNaN(intId)) return false;

        // Pad string with zeros up to 9 digits
        id = id.length < 9 ? ("00000000" + id).slice(-9) : id;

        return Array.from(id, Number)
            .reduce((counter, digit, i) => {
                const step = digit * ((i % 2) + 1);
                return counter + (step > 9 ? step - 9 : step);
            }) % 10 === 0;
    }

    isValidPhone(phoneNum) {
        var phoneRe = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/;
        return phoneRe.test(phoneNum);
    }

    LoginStepA(state) {
        //localStorage.clear();
        var isUserAuthenticated = state.isUserAuthenticated;
        var data = {
            countryCode: '+972',
            phoneNumber: this.state.phone,
            identityNumber: this.state.id
        };

        if (!data || !data['phoneNumber'] || !data['identityNumber'])
            return;
        var isValidIdNumber = this.isValidIsraeliID(data['identityNumber']);
        var isValidPhoneNumber = this.isValidPhone(data['phoneNumber']);
        if (!isValidIdNumber || !isValidPhoneNumber)
            this.setState({ isLoading: false, isStepB: false, phone: data ? data['phoneNumber'] : "", id: data ? data['identityNumber'] : "", isInvalidIdNumber: !isValidIdNumber, isInvalidPhoneNumber: !isValidPhoneNumber, isUserAuthenticated: isUserAuthenticated ? isUserAuthenticated : false });
        else {
            var thisObj = this;
            fetch('api/account/stepA', {
                method: 'POST',
                headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            })
            .then(function (response) {
                thisObj.setState({ isLoading: false, isStepB: response.ok, phone: data ? data['phoneNumber'] : "", id: data ? data['identityNumber'] : "", isInvalidIdNumber: false, isInvalidPhoneNumber: false, isUserAuthenticated: isUserAuthenticated ? isUserAuthenticated : false });
            })
            .catch(err => console.log(err))    
        }
    }
    LoginStepB(state) {
        var data = {
            countryCode: '972',
            phoneNumber: this.state.phone,
            identityNumber: this.state.id,
            password: this.state.password,
            organizationName: this.state.organizationName //??
        };

        if (!data || !data['password'])
            return;
        var thisObj = this;
        let fetchTask = fetch('api/account/stepB', {
            method: 'POST',
            headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(function (response) {
                //saveUserInConfig(response.user);
                console.log(response);
                if (!response.isSuccess) {
                    alert(response.error);
                    throw response.error;
                }
                localStorage.setItem("securityToken", response.token);
                localStorage.setItem("ExpirationTime", response.expirationTime);
                localStorage.setItem("user", JSON.stringify(response.user));
                // let organizationObj = {"organizationName": response.organization.name, 'iconOrganization': response.organization.iconOrganization}
                let organizationObj = JSON.stringify(response.organization);
                // console.log(organizationObj);
                localStorage.setItem("organization", organizationObj);
                //console.log(this.state.isUserAuthenticated);
                //setLogin(state.isUserAuthenticated);
                thisObj.setState({ isLoading: false, isStepB: false, isStepBSucceed: true });
                // window.location ="homePage"//  thisObj.props.history.push("/homePage", { branch: 123 });
                window.location ="surgery"
            })
            .catch(error => {
                thisObj.setState({ isLoading: false, isStepB: true, isStepBSucceed: false });
            });
        //addTask(fetchTask);
    }

    LoadUserDetails() {
        var user = localStorage.getItem("user");
        if (user) {
            user = JSON.parse(user);
            this.setState({ isLoading: false, phone: user ? user['userName'] : "", id: user ? user['identityNumber'] : "", isUserAuthenticated: true });
        }
    }

    componentDidMount() {
        // console.log(this.props)
        this.LoadUserDetails();
        var isNew = window.location.search ? window.location.search.split("=") : null;
        if (isNew && isNew.length > 1 && isNew[1] == "true")
            this.setState({ isRegister: true });

    }

    render() {
        var t = this.props.t;
        
        return <div>
        {/* if user register its disiner register if not it is designer enter */}
            <HeaderTitlePage title={this.state.isRegister ?t("Account.enter"):t("Account.Signup") } />


            <div className="container">
                <div className="row">
                    <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                        <div className="card card-signin my-5">
                            <div className="card-body">

                                <form className="form-signin" onSubmit={e => {
                                    e.preventDefault()
                                    this.state.isStepB ? this.LoginStepB(this.state) : this.LoginStepA(this.state);
                                }}>
                                    <div className="">

                                        {!this.state.isStepB ?
                                            <div className="">
                                                <input autoFocus type="phone" name="phone" className="form-control " placeholder={t("Account.Mobile")} value={this.state.phone}
                                                    onChange={e => this.setState({ phone: e.target.value })} />
                                                {this.state.isInvalidPhoneNumber ? <div className="fs-12 text-danger ">{t("Account.IncorrectNumber")}</div> : ""}
                                                <br/>
                                            </div> : ""}



                                        {!this.state.isStepB ?
                                            <div className="">
                                                <input type="text" name="id" className="form-control" placeholder={t("Account.id")} value={this.state.id}
                                                    onChange={e => this.setState({ id: e.target.value })} />
                                                <br/>                                {this.state.isInvalidIdNumber ? <span className="fs-12 text-danger">{t("Account.massegId")}</span> : ""}
                                            </div> : ""}


                                        {!this.state.isStepB && this.state.isRegister ?
                                            <div className="">
                                                <input type="text" name="organizationName" className="form-control" placeholder={t("Account.nameOrganization")} value={this.state.organizationName}
                                                    onChange={e => this.setState({ organizationName: e.target.value })} />
                                            </div> : ""}

                                        {this.state.isStepB ?
                                            <div className="">
                                                <input autoFocus type="text" className="form-control " placeholder={t("Account.code")} value={this.state.password}
                                                    onChange={e => this.setState({ password: e.target.value })} />
                                            </div> : ""}
                                        {this.state.isStepB ? <small className="fs-14 text-secondary pr-3">{t("Account.massegeToMobile")}</small> : ""}


                                        <div className="">
                                            {!this.state.isStepB ?
                                                < input type="submit" value={t("Account.next")} className="btn btn-lg btn-primary btn-block " />
                                                : < input type="submit" value={t("Account.enter")} className="btn btn-lg btn-primary btn-block " />}
                                        </div> 
                                     
                                        </div>                           
                                 </form>


                                    </div>
                                                
                            </div>
                        </div>
                    </div>
                </div>

        </div>



    }
};

export default withRouter(Account);
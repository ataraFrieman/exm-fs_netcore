import React, { Component } from 'react';
import {
    faUserCircle, faKey, faMobileAlt, faFileAlt, faCheck, faCalendar, faClock, faUserMd, faMapMarkerAlt, faCommentDots, faLocationArrow,
    faCheckSquare, faHome, faCog, faHistory, faPaperPlane, faSignOutAlt, faExclamationCircle, faExclamation, faFileMedical, faFileSignature,
    faFile, faVial, faUpload, faCircle, faHospital, faCalendarPlus, faUsers, faEdit, faAngleDoubleRight, faChartLine, faAngleDown, faBuilding,
    faArrowLeft, faTag, faTasks, faTags, faIdCard, faListUl, faThLarge, faTable, faLanguage, faMinus, faPlusCircle, faClipboard, faSearch, faTimesCircle,
    faMale, faFemale, faBriefcaseMedical, faPlus, faEllipsisV, faCogs, faIdCardAlt, faChevronCircleRight, faChevronCircleLeft, faAngleLeft, faAngleUp,
    faProcedures, faTrash, faBell, faChevronLeft, faChevronRight,
} from '@fortawesome/free-solid-svg-icons';
import { Route, Switch, BrowserRouter } from 'react-router-dom';
import { Layout } from './components/Layout';
//import { Home } from './components/Home';
import Users from './components/Users';
import { SelectServiceType } from './components/serviceType/SelectServiceType';
import { Account } from './components/Account';
//import { Organizations } from './components/Organization'; - OLD
//import Branches from './components/Branches'; - OLD
//import { Branch } from './components/Branch'; - OLD
import Schedual from './components/Schedual';
import  Surgery  from './components/Surgery';
//import HomePage from './components/HomePage'; - OLD
import { ServiceProvider } from './components/ServiceProvider';
import ServiceProvidersList from './components/ServiceProviders';
//import { Tasks } from './components/task/Tasks'; - OLD
//import { ManageScreen } from './components/manage/ManageScreen'; - OLD
import QueuesComponent from './components/QueueComponent';
import Fellow from './components/FellowDetails';
import FellowsList from './components/Fellows';
import { isUserAuthenticated, getUser, getLanguage, setLanguageStorege } from './helpers/User'
import { library } from '@fortawesome/fontawesome-svg-core'
import filtersList from './stores/schedualFilters';
import providersList from './stores/serviceProviders';
import serviceTypesStore from './stores/serviceTypes';
//import branchesStore from './stores/branches'; - OLD
//import queuesStore from './stores/queues'; - OLD
import locationsStore from './stores/Locations'
import departmentsStore from './stores/Departments'
import FellowsStore from './stores/fellows';
import EquipmentStore from './stores/Equipment';
import serviceQueuesStore from './stores/serviceQueues'
import cancelationReasonsStore from './stores/CancelationReasons'
import { observer } from 'mobx-react';
import i18n from "i18next";
import detector from "i18next-browser-languagedetector";
import { withI18n, reactI18nextModule } from "react-i18next";
import { englishData } from "./locales/en/translation";
import { hebrewData } from "./locales/he/translation";
import * as $ from 'jquery';
import Departments from './stores/Departments';

library.add(faUserCircle, faKey, faMobileAlt, faFileAlt, faCheck, faCalendar, faClock, faUserMd, faMapMarkerAlt, faCommentDots, faLocationArrow,
    faCheckSquare, faHome, faCog, faHistory, faPaperPlane, faSignOutAlt, faCalendarPlus, faExclamationCircle, faFileMedical, faFileSignature, faArrowLeft, faTag,
    faFile, faVial, faExclamation, faUpload, faCircle, faHospital, faCalendarPlus, faUsers, faEdit, faAngleDoubleRight, faClipboard, faAngleDown,
    faChartLine, faBuilding, faTasks, faTags, faIdCard, faListUl, faThLarge, faTable, faLanguage, faMinus, faPlusCircle, faSearch, faTimesCircle,
    faMale, faFemale, faBriefcaseMedical, faPlus, faEllipsisV, faCogs, faIdCardAlt, faChevronCircleRight, faChevronCircleLeft, faAngleLeft, faAngleUp,
    faProcedures, faTrash, faBell, faChevronLeft, faChevronRight);



const cssHE = `.text-align {text-align: right;}
.addNav {left: 0;}
.direction {direction:rtl;}
`;
const cssEN = `.text-align {text-align: left;}
.addNav {right:0;}
.direction { direction: ltr;}

`;
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
var language = getLanguage();
i18n
    .use(reactI18nextModule) // passes i18n down to react-i18next
    .use(detector) //step 0) add detector what language i am using, and in 'lng' i am tell him to take it from the localStorage
    .init({
        resources: {
            he: { translation: hebrewData() },
            en: { translation: englishData() }
        },
        lng: getLanguage(),
        fallbackLng: "en",
        interpolation: {
            escapeValue: false
        },
        react: {
            bindI18n: 'languageChanged'
        }
    });
class App extends Component {
    constructor(props) {
        super(props);


        if (isUserAuthenticated()) {
            console.log("log in")
            var serviceProvidersList = providersList.fromJS();
            serviceProvidersList.getServiceProviders();
            var serviceTypes = serviceTypesStore.fromJS();
            serviceTypes.loadServiceTypes();
            var roomsList = locationsStore.fromJS();
            roomsList.getServiceStations();
            var departmentsList = departmentsStore.fromJS();
            departmentsList.getDepartments();
            /* infrastructure to get all ServiceQueues from the Mobx */
            var serviceQueuesList = serviceQueuesStore.fromJS();
            serviceQueuesList.getServiceQueues();
            var cancelationReasonsList = cancelationReasonsStore.fromJS();
            cancelationReasonsList.getCancelationReasons()
            var fellows = FellowsStore.fromJS();
            fellows.getFellows();
            //var SchedualfiltersList = filtersList.fromJS(); - old
            //var branches = branchesStore.fromJS(); - old
            //var queues = queuesStore.fromJS(); - old
            var user = getUser();
            var equipments = EquipmentStore.fromJS(); 
            equipments.getEquipments();
            var language = user && user.language ? user.language : "en"
            this.state = {
                serviceProvidersList: serviceProvidersList,
                serviceTypes: serviceTypes,
                roomsList: roomsList,
                departmentsList: departmentsList,
                serviceQueuesList: serviceQueuesList,
                cancelationReasonsList: cancelationReasonsList,
                fellows: fellows,
                equipments:equipments,
                language: language
            };
        } else {
            var user = getUser();
            var language = user && user.language ? user.language : "en"
            this.state = {
                /*SchedualfiltersList: SchedualfiltersList, branches: branches, queues: queues - OLD*/
                language: language
            }
        }
        
        
        //this.getBranchesComponent = this.getBranchesComponent.bind(this) 
        //this.getSchedualComponent = this.getSchedualComponent.bind(this)
        //this.getQueuesComponent = this.getQueuesComponent.bind(this)
        //this.getHomePage = this.getHomePage.bind(this)
        this.getFellowsComponent = this.getFellowsComponent.bind(this)
        this.getSurgeryComponent = this.getSurgeryComponent.bind(this)
        this.getUserComponent = this.getUserComponent.bind(this)
        this.renderAuthonticatedRouters = this.renderAuthonticatedRouters.bind(this)
        this.renderAnonimusRouters = this.renderAnonimusRouters.bind(this)
        this.getSPComponent = this.getSPComponent.bind(this)
        this.setLanguage = this.setLanguage.bind(this)
        this.GetSelectServiceType=this.GetSelectServiceType.bind(this)
    }

    setLanguage(lan) {
        this.setState({ language: lan }, this.initLanguageCss);
        setLanguageStorege(lan);
        language = lan;
        i18n.changeLanguage(lan, (err, t) => {
            if (err) return console.log('something went wrong loading', err);
        })
    }
  
    /*OLD*/
    // getSchedualComponent() {

    //     var schedualComponent = <Schedual
    //         filtersList={this.state.SchedualfiltersList.filtersList}
    //         serviceProviders={this.state.serviceProvidersList.serviceProviders}
    //         serviceTypesList={this.state.serviceTypes.serviceTypesList}
    //         serviceTypes={this.state.serviceTypes}
    //         // branches={this.state.branches} - old
    //         t={this.props.t}
    //     />;
    //     return schedualComponent;

    // }

    getSurgeryComponent() {
        var surgeryComponent = <Surgery 
            t={this.props.t} 
            serviceProviders={this.state.serviceProvidersList.serviceProviders} 
            serviceTypes={this.state.serviceTypes}
            fellows={this.state.fellows}
            rooms={this.state.roomsList}
            departments={this.state.departmentsList}
            cancelationReasons={this.state.cancelationReasonsList}
            serviceQueues={this.state.serviceQueuesList.serviceQueuesList}
            equipments={this.state.equipments}
            />;
       return surgeryComponent;
    }

    getUserComponent() {
        return <Users t={this.props.t} serviceProviders={this.state.serviceProvidersList} type={2} />;
    }

    /*OLD*/
    // getBranchesComponent() {
    //     return <Branches t={this.props.t} branches={this.state.branches} />;
    // }

    getFellowsComponent() {
        return <FellowsList t={this.props.t} fellows={this.state.fellows} />;
    }

    getSPComponent() {
        return <ServiceProvidersList t={this.props.t} serviceProviders={this.state.serviceProvidersList} />;
    }

    /*OLD*/
    // getQueuesComponent() {
    //     return <QueuesComponent t={this.props.t} QueuesList={this.state.queues.queuesList} Queues={this.state.queues} filtersList={this.state.SchedualfiltersList.filtersList} filterStore={this.state.SchedualfiltersList} />;
    // }

    /*OLD*/
    // getHomePage() {
    //     return <HomePage t={this.props.t}  QueuesList={this.state.queues.queuesList} Queues={this.state.queues}/>
    // }

    GetSelectServiceType()
    {
        return <SelectServiceType t={this.props.t}></SelectServiceType>
    }

    /*OLD*/
    // getTasksComponent = () =>
    // {
    //     return <Tasks t={this.props.t}></Tasks>
    // }

    getAccountComponent=()=>
    {
        return <Account t={this.props.t} ></Account>
    }

    renderAuthonticatedRouters = () => {

        return (
            <div>
                <Switch>
                    {/* <Route exact path='/' component={this.getHomePage} /> */}
                    {/* <Route path='/organization' component={Organizations} /> */}
                    {/* <Route path='/home' component={Organizations} /> */}
                    {/* <Route path='/branches' component={this.getBranchesComponent} /> */}
                    {/* <Route path='/branch' component={Branch} /> */}
                    {/* <Route path='/schedual' component={this.getSchedualComponent} /> */}
                    {/* <Route path='/homePage' component={this.getHomePage} /> */}
                    {/* <Route path='/tasks' component={this.getTasksComponent}/> */}
                    {/* <Route path='/manageScreen' component={ManageScreen} /> */}
                    {/* <Route path='/SelectServiceType' component={this.GetSelectServiceType} /> */}
                    {/* <Route path='/queues' component={this.getQueuesComponent} /> */}
                    <Route exact path='/' component={this.getSurgeryComponent} />
                    <Route path='/login' component={this.getAccountComponent} />
                    <Route path='/serviceProviders' component={this.getSPComponent} />
                    <Route path='/serviceProvider' component={ServiceProvider} t={this.props.t}/>
                    <Route path='/surgery' component={this.getSurgeryComponent} />
                    <Route path='/users' component={this.getUserComponent} />
                    <Route path='/fellow' component={Fellow} fellows={this.state.fellows} t={this.props.t} />
                    <Route path='/fellows' component={this.getFellowsComponent} />
                    <Route component={this.NotFound}></Route>
                </Switch>
            </div>
        );
    }

    renderAnonimusRouters = () => {
        return (
            <div>
                <Switch>
                    {/* <Route exact path='/' component={HomePage} /> */}
                    <Route exact path='/' component={this.getAccountComponent} />
                    <Route path='/login' component={this.getAccountComponent}/>
                    <Route component={this.NotFound}></Route>
                </Switch>
            </div>
        );
    }

    NotFound = () => {
        var t=this.props.t;
        return t("app.notFound");
    }

    render() {
        
        return (
            <div className="h-100 a" >
                {this.state.language == "he" ? <style>{cssHE}</style> : <style>{cssEN}</style>}
                <BrowserRouter basename={baseUrl} >
                    <Layout t={this.props.t} language={this.state.language} setLanguage={this.setLanguage}>
                        {
                            isUserAuthenticated() ? this.renderAuthonticatedRouters()
                                : this.renderAnonimusRouters()//if user exist return nav with all components
                        }
                    </Layout>
                </BrowserRouter>
            </div>
        );
    }
}
export default observer(App);
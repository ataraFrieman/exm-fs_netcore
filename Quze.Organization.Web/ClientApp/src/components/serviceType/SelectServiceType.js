import React, { Component } from 'react';
import "../../css/site.css"
import "../../css/general.css"
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import * as http from "../../helpers/Http";
import ButtonServiceType from './ButtonServiceType';
import ServiceTypeDetails from './ServiceTypeDetails';
import history from '../../helpers/History';
import AddTast from './AddTast';
import AddDocument from './AddDocument';
import ViewReqiuredTasks from './ViewReqiuredTasks';
import ViewReqiuredDocuments from './ViewReqiuredDocuments';
import AncestorsLinkList from './AncestorsLinkList';
import { Carousel } from 'react-bootstrap';
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import { PropagateLoader } from 'react-spinners';
import './../../css/SelectServiceType.css'
import { array } from 'prop-types';
import { withI18n } from 'react-i18next';
import { timingSafeEqual } from 'crypto';



//to represent a tree node.
class TreeNode {
    constructor(content, childrenTreeNode) {
        this.content = content;
        this.childrenTreeNode = childrenTreeNode;
    }
}


export class SelectServiceType extends Component {
    constructor(props) {
        super(props);
        this.state = {
            tree: TreeNode,
            loading: true,
            parentId: null,
            parent: TreeNode,
            SPList: [],
            loadingTaskList: true,
            taskList: [],
            loadingDocumentsList: true,
            documentsList: [],
            buttonClicked: false,
            key: 1,
            parentsArray: [],
            isEditServiceType: false,
            backList: [],
            hading: "",
            styleClickDi:true,
            styleClickD:false,
            styleClickS:false,
            sty:"font-pr",
            resBack:""
        };

        this.handleSelect = this.handleSelect.bind(this);
    }

    showModal = () => {
        this.setState({ showModal: true });
    }

    closeModal = () => {
        this.setState({ showModal: false });
    }

    componentDidMount = () => {
        http.get('api/ServiceTypes/GetSTAsTree')
            .then(res => {
                if (!res || res.length == 0||(res="No Content")) {
                    console.log("Get failed...")
                    this.setState({
                        resBack:res,
                        loading:false
                    });
                } else {
                    this.setState({
                        tree: res,
                        loading: false,
                        parent: res.root,
                        parentId: res.root.content.id
                    });

                }
            }).then(() => {
                this.getTasks();
                this.getDocument();
            });
    }

    clickedST = (node) => {

        this.setState({
            parent: node,
            parentId: node.content.id,

        })
        this.setState(prevState => ({
            parentsArray: [...prevState.parentsArray, this.state.parent],
            parent: node,
            parentId: node.content.id,
        }))

        http.get('api/RequiredTasks/GetTasksBySTAsync?id=' + node.content.id)
            .then(response => {
                this.setState({
                    taskList: response.entities,
                    loadingTaskList: false,
                })
            }).then(() => {
                this.getTasks();
                this.getDocument();
            })
    }

    clickeOlddST = (node) => {

        this.setState({
            parent: node,
            parentId: node.content.id,

        })

        http.get('api/RequiredTasks/GetTasksBySTAsync?id=' + node.content.id)
            .then(response => {
                this.setState({
                    taskList: response.entities,
                    loadingTaskList: false,
                })
            }).then(() => {
                this.getTasks();
                this.getDocument();
            })
        let flag = true;
        let newArray = new Array();
        for (var i = 0; i < this.state.parentsArray.length && flag; i++) {
            if (this.state.parentsArray[i] == node)
                flag = false;
            else newArray.push(this.state.parentsArray[i]);
        }
        this.setState({ parentsArray: newArray });
    }

    getTasks = () => {
        http.get('api/RequiredTasks/GetTasksBySTAsync?id=' + this.state.parentId)
            .then(response => {
                this.setState({
                    taskList: response.entities,
                    loadingTaskList: false,
                })
            })

    }

    getDocument = () => {
        http.get('api/RequiredDocuments/GetDocumentsByST?id=' + this.state.parentId)
            .then(response => {
                this.setState({
                    documentsList: response.entities,
                    loadingDocumentsList: false,
                })
            })
    }

    getID = () => {
        return this.state.parentId
    }

    handleSelect(key) {
        this.setState({ key: key });
    }

    handleSelect = (selectedIndex, e) => {
        this.setState({
            index: selectedIndex,
            direction: e.direction,
        });
    }

    sT = (node) => {
        this.setState({
            parent: node,
            parentId: node.content.id,

        })

        this.setState(prevState => ({
            parent: node,
            parentId: node.content.id,
        }))

        http.get('api/RequiredTasks/GetTasksBySTAsync?id=' + node.content.id)
            .then(response => {
                this.setState({
                    taskList: response.entities,
                    loadingTaskList: false,
                })
            }).then(() => {
                this.getTasks();
                this.getDocument();
            })
    }

    openCurrentTS = (node, c) => {
        this.setState({ isEditServiceType: true });
        this.sT(node);
        this.setState({ backList: c });
    }

    showList = () => {
        this.setState({ isEditServiceType: false });
        this.sT(this.state.backList);
    }
    
    render = () => {
        var t = this.props.t;
        const hading = this.state.loading == false&&this.state.resBack=="" ? <span className="m-3 p-3 "><h6 className="text-align-justify"><AncestorsLinkList list={this.state.parentsArray} click={this.clickeOlddST} listLength={this.state.parentsArray.length} /></h6></span> : "";
        var s = [], s1 = [];

        if (this.state.loading == false&&this.state.resBack=="") {
            s1 = [];

            for (var i = 0; i < this.state.parent.childrenTreeNode.length / 5; i++) {
                s = [];
                for (var j = 0; j < 5 && i * 5 + j < this.state.parent.childrenTreeNode.length; j++) {
                    s.push(<span key={i * 5 + j}>
                        <ButtonServiceType list={this.state.parent} openCurrent={this.openCurrentTS} 
                        TreeNode={this.state.parent.childrenTreeNode[i * 5 + j]} SetServiceTypeToPresent={this.clickedST} />
                    </span>)
                }
                if (i == 0)
                    s1.push(<div className="carousel-item active bg-light" key={i}>{s}</div>);
                else s1.push(<div className="carousel-item bg-light" key={i}>{s}</div>);

            }
        }

        var decscription = this.state.loading == false&&this.state.resBack=="" ? this.state.parent.content.description : "";

        var addNewTask = <AddTast t={this.props.t} serviceTypeID={this.state.parentId} serviceName={decscription} tab="task" getTasks={this.getTasks} />;

        var task = this.state.loadingTaskList == false&&this.state.resBack=="" ? <ViewReqiuredTasks t={this.props.t} serviceTypeID={this.state.parentId} serviceName={decscription} list={this.state.taskList} loadingTaskList={this.state.loadingTaskList} getTasks={this.getTasks} /> :
         <div className="ProgressStyle"> <PropagateLoader color={'#3c519d'} /> </div>;
        var addNewDocument = <AddDocument t={this.props.t} serviceTypeID={this.state.parentId} serviceName={decscription} tab="task" getDocuments={this.getDocument} />;
        var document = this.state.loadingDocumentsList == false ? <ViewReqiuredDocuments t={this.props.t} serviceTypeID={this.state.parentId} serviceName={decscription} list={this.state.documentsList} loadingdocumentskList={this.state.loadingDocumentsList} getDocument={this.getDocument} /> : <div className="ProgressStyle "> <PropagateLoader color={'#3c519d'} /> </div>;
        if (this.state.loading == false&&this.state.resBack=="") {
            var c = this.state.parent.childrenTreeNode.length;
        }
        else
            c = 0;
        var whatservice = c > 0?<h4 className="text-secondary">
            {t("SelectServiceT.what")} {this.state.parent.content.description} {t("SelectServiceT.servicesneed")} </h4>
            :this.state.loading == false||this.state.resBack=="No Content"?<h4 className="text-secondary"> {t("SelectServiceT.noservise")}</h4>:<div className="ProgressStyle "> <PropagateLoader color={'#3c519d'} /> </div>
        var listData = this.state.loading == false&&this.state.resBack=="" ? <Tabs activekey={this.state.key} onSelect={this.handleSelect} id="controlled-tab-example" >
            {/* <TabList className="text-align" > */}
                <Tab eventkey={1} title={t("SelectServiceT.directives")} onClick={e=>{this.setState({styleClickDi:true});
                this.setState({styleClickD:false}); 
                this.setState({styleClickS:false});}}>
                <p className={this.state.styleClickDi?"font-pr text-info hovertabs":"font-pr hovertabs"}>
                {t("SelectServiceT.directives")}
            </p>     
                </Tab>
                <Tab eventkey={2} title={t("SelectServiceT.Documents")}onClick={e=>{this.setState({styleClickDi:false});
                this.setState({styleClickD:true}); 
                this.setState({styleClickS:false});}}>
                <p className={this.state.styleClickD?"font-pr text-info hovertabs":"font-pr hovertabs"}>
                    {t("SelectServiceT.Documents")}
                </p>
                </Tab>
                <Tab eventkey={3} title={t("SelectServiceT.fgiveService")}
                onClick={e=>{this.setState({styleClickDi:false});
                this.setState({styleClickD:false}); 
                this.setState({styleClickS:true});}}>
                <p className={this.state.styleClickS?"font-pr text-info hovertabs":"font-pr hovertabs"}>
                    {t("SelectServiceT.giveService")}
                    </p>
                </Tab>
            {/* </TabList> */}
            <div className="bg-light">
                <TabPanel className="bg-light" >
                    {addNewTask}
                    {task}
                </TabPanel>
                <TabPanel className="bg-light">
                    {addNewDocument}
                    {document}
                </TabPanel>
                <TabPanel>
                    "בפיתוח:)"
        </TabPanel>
            </div>
        </Tabs> : "";

        return <div>
            
            {!this.state.isEditServiceType&&this.state.loading == false&&this.state.resBack==""?
                <div>{hading}</div> : ""}
            <br />
            {this.state.isEditServiceType ?
                <button className="btn btn-link font-pr p-0 border-0 col-12 text-align" onClick={this.showList}> {t("MQ.backtolist")}</button> : ""}
            <div id="carouselExampleControls" className="carousel slide" data-ride="carousel" data-interval="false">
                {!this.state.isEditServiceType ?
                    <div className="carousel-inner">
                        {whatservice}
                        <br></br>
                        {s1}
                    </div> : ""}
                <a className="carousel-control-prev" href="#carouselExampleControls" role="button" data-slide="prev">
                    <span className="carousel-control-prev-icon" aria-hidden="true"></span>
                    <span className="sr-only">{t("SelectServiceT.pre")}</span>
                </a>
                <a className="carousel-control-next" href="#carouselExampleControls" role="button" data-slide="next">
                    <span className="carousel-control-next-icon" aria-hidden="true"></span>
                    <span className="sr-only">{t("SelectServiceT.next")}</span>
                </a>
            </div>
            {this.state.isEditServiceType?
                <div>
                    {listData}</div> :""}
                 

        </div>
    }
};

export default withI18n()(SelectServiceType);

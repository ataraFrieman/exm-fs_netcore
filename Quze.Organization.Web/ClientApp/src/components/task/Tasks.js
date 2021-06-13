
import React, { Component } from 'react';
import {Table, Col, Row, Card, Button  } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import TasksCard from './TasksCard'
import TasksTable from './TasksTable'
import TasksGrid from './TasksGrid'
import TasksCompleted from './TasksCompleted'
import HeaderTitlePage from '../HeaderTitlePage';
import { getTasks } from '../../helpers/TasksFromServer';
import { getUser, logout } from '../../helpers/User';
import * as http from '../../helpers/Http';

export class Tasks extends Component {
    state = {
         tasksToHandle: [
        
         ],
        tasksFinished: [],
        // isUrgent: false,
        activeTabIndex: 1,
        userId : null,
        tasksList: [],
        userName: ""
    }
    
    componentDidMount() {
        this.loadTasksDatails();
    }
    loadTasksDatails() {
        let user = getUser();
        //console.log(user)
        if (!user.id) {
            this.setState({ userId: null });
            return;
        }
        var thisObj = this;
        http.get('api/Users/GetUserTasks/' + user.id)
            .then(function (response) {
                //console.log(response.entities.length);
                if (!response.entities)
                    return;
                //do for to check the time and to see what to put in the tasks list, and then do setState
                let printItem = null;
                let res = response.entities;
                let resLength = response.entities.length;
                for(let i = resLength - 1; i >= 0; i--) {
                    //status
                    if(res[i].executionDateTime !== null) {
                        //console.log(res[i].executionDateTime)
                        printItem = res.splice(i, 1);
                        console.log(printItem); 
                    }
                }
                thisObj.setState({
                    userId: user.id,
                    userName: user.fullName,
                    tasksList: res
                })
                //console.log(this.state.tasks)
            });
        
    }

    //position: relative;
    //aria-expanded

    handleClickTab(event, index, thisObj) {
        event.stopPropagation();
        event.nativeEvent.stopImmediatePropagation();
        thisObj.setState({ activeTabIndex: index })
    }

    handelDelete = (id) => {
        //console.log("handelUpdate: " + id);

        //remove from the 'task list'
        const listT = this.state.tasksList.filter(item => { //tasksToHandle
            return item.id !== id
        });
        
        //get the task that removed and add it to 'task completed'
        const listF = this.state.tasksList.filter(item => { //tasksToHandle
            return item.id == id;
        }) 
        console.log(listF);
        let data = { 
            Entity: {
                id: listF[0].id,
                appointmentId: listF[0].appointmentId,
                content: listF[0].content,
                executionDateTime: listF[0].executionDateTime,
                taskType: listF[0].taskType,
                title: listF[0].title,
                userId: listF[0].userId
            } 
         }
         console.log(data)
         var thisObj = this;
        //call to server and send the time + saved the task that finished
        http.put('api/Users/CloseExecutedUserTask', data)
        .then(function (response) {
            if (!response.entity)
                    return;
            console.log(response)
            let tasksCompleted = [...thisObj.state.tasksFinished, listF]
            thisObj.setState({
                tasksList: listT,  //tasksToHandle
                tasksFinished: tasksCompleted
            });
        });
    }

    // handleLogout = () => {
    //     console.log("Logout")
    // }

    // handleChange = (e) => {
    //     if (e.target.value != "") {
    //         console.log(e.target.value);
    //         // console.log(e.currentTarget.value);
    //     }

    // }

    /*needed ? 
      remove from the 'task completed'
      get the task that removed and add it to 'task list', there is a problem with that 'tasksFinished
    */
    // handleAdd = (id) => {
    //     console.log("handleAdd: " + id);
    //     const listComplet = this.state.tasksFinished.filter(item => {
    //         return item.id !== id;
    //     }) 
    //     const listTask = this.state.tasksFinished.filter(item => {
    //         return item.id == id
    //     });
    //     let listBack = [...this.state.tasksToHandle, listTask] //tasksToHandle
    //     this.setState({
    //         tasksToHandle: listBack,  //tasksToHandle
    //         tasksFinished: listComplet
    //     })

    // }
        
    render() {
        var t=this.props.t;
        return (
            <div className="">
                    <HeaderTitlePage title={t("tasks.tasks")}/>


                <ul className="nav nav-tabs p-2" role="tablist">
                {/*  col-7 -> move the tabs left  */}
                    <li className="nav-item" >
                        <a className={"nav-link " + (this.state.activeTabIndex == 1 ? " active " : "")}
                            onClick={(event) => { this.handleClickTab(event, 1, this) }} role="tab">
                            {/* טבלת משימות */}
                            <FontAwesomeIcon icon="table" data-toggle="tooltip" title={t("tasks.tasksTable")}/>
                        </a>
                    </li>
                    <li className="nav-item" >
                        <a className={"nav-link " + (this.state.activeTabIndex == 2 ? " active " : "")}
                            onClick={(event) => { this.handleClickTab(event, 2, this) }} role="tab">
                            {/* קלפי משימות */}
                            <FontAwesomeIcon icon="th-large" data-toggle="tooltip"  title={t("tasks.Taskcards")}/>
                        </a>
                    </li>
                    <li className="nav-item" >
                        <a className={"nav-link " + (this.state.activeTabIndex == 3 ? " active " : "")}
                            onClick={(event) => { this.handleClickTab(event, 3, this) }} role="tab">
                            {/* רשת משימות */}
                            <FontAwesomeIcon icon="list-ul" data-toggle="tooltip" title={t("tasks.Tasknetwork")}/>
                        </a> 
                    </li>
                    <li className="nav-item">
                        <a className={"nav-link " + (this.state.activeTabIndex == 4 ? " active " : "")}
                            onClick={(event) => { this.handleClickTab(event, 4, this) }} role="tab">
                            {/* משימות שטופלו */}
                            <FontAwesomeIcon icon="history" data-toggle="toolip" title={t("tasks.Taskshandled")}/>
                        </a>
                    </li>
                </ul>

                <div className="tab-content h-75" id="myTabContent" >
                    {this.state.activeTabIndex == 1 ? 
                        <div className="tab-pane fade active show">
                            {/* tasksToHandle */}
                            <TasksTable t={this.props.t} tasksList={this.state.tasksList} handelDelete={this.handelDelete}/>
                        </div> : ""
                    }
                    {this.state.activeTabIndex == 2 ?
                        <div className="tab-pane fade active show">
                            {/* tasksToHandle */}
                            <TasksCard t={this.props.t} tasksList={this.state.tasksList} handelDelete={this.handelDelete}/>
                        </div> : ""
                    }
                    {this.state.activeTabIndex == 3 ? 
                        <div className="tab-pane fade active show">
                            {/* tasksToHandle */}
                            <TasksGrid t={this.props.t} tasksList={this.state.tasksList} handelDelete={this.handelDelete}/>
                        </div> : ""
                    }
                    {/* && this.state.tasksFinished.length ? */}
                    {this.state.activeTabIndex == 4 ? 
                        <div className="tab-pane fade active show"> 
                         
                            <TasksCompleted t={this.props.t} listCompleted={this.state.tasksFinished} handleAdd={this.handleAdd}/>
                            {/* <div className="text-right strong fs-24 p-2 m-2">שום משימה לא טופלה</div> */}
                        </div> : ""
                        // <div className="text-right strong fs-24 p-2 m-2">שום משימה לא טופלה</div>
                    }
                </div>

                {/* אפשר אולי לעשות איזה סוויצ' ולפי לחיצת כפתור לתת את המשימות כטבלה או כקלף */}

            
            </div>
    )
  }
}

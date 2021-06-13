import React, { Component } from 'react';
import { withI18n } from "react-i18next";
import { getDir } from '../helpers/User';
import MinimalKit from '../stores/MinimalKit';
import * as http from '../helpers/Http';

export class Task extends Component {

    displayName = "Task"

    constructor(props) {
        super(props);
        var minimalKitStore = MinimalKit.fromJS();
        this.state = {
            descriptionTask: "",
            isRequired: false,
            tasks: this.props.tasks,
            tasksToAdd: props.appointment.appointmentTasks,
            minimalKitStore: minimalKitStore,
            isAddTask: false,
            disabledBtn: true
        };
        this.addTask = this.addTask.bind(this);
        this.showAddTask = this.showAddTask.bind(this);
        this.handelChange = this.handelChange.bind(this);
        this.renderMinimalKitTasks = this.renderMinimalKitTasks.bind(this);
    }

    showAddTask() {
        if(this.state.isAddTask == true) {
            this.setState({ isAddTask: !this.state.isAddTask, descriptionTask: "", disabledBtn: true });
        } 
        else {
            this.setState({ isAddTask: !this.state.isAddTask });
        }
    }

    addTask() {
        var tasks = this.state.tasksToAdd;
        
        http.post("api/MinimalKit/AddTask", { description: this.state.descriptionTask, isRequired: this.state.isRequired, appointmentId: this.props.appointment.id })
        .then(response => {
            this.showAddTask();
            tasks.push(response);
            this.setState({ tasksToAdd: tasks, descriptionTask: "", isRequired: false });
        })
        .catch(error => { console.log("Error:", error);});

    }

    //renderMinimalKit() {
    //    var appointment = this.props.appointment;
    //    let id = appointment.fellow.id; // ?

    //    let tasks = appointment.appointmentTasks;


    //    if (appointment.appointmentTasks.length > 0) {
    //        appointment.appointmentTasks.forEach(task => {
    //            console.log("task: ", task.requiredTask.description)
    //        })
    //    }

    //    return <span className='' > {/* mkDisplay */}
    //        {
    //            tasks.length ?
    //                tasks.map(task => this.renderTask(task))
    //                : ""
    //        }
    //    </span>
    //}


    handelChange(e) {
        //console.log("input is: ", e.target.value)
        // this.setState({ [e.target.id]: e.target.value })
        if(e.target.value == "")
            this.setState({ descriptionTask: e.target.value, disabledBtn: true });
        else
            this.setState({ descriptionTask: e.target.value, disabledBtn: false });
    }

    renderMinimalKitTasks(tasks) {
        //var appointment = this.props.appointment;
        // if (appointment.appointmentTasks.length > 0) {
        //     appointment.appointmentTasks.forEach(task => {
        //         console.log("task: ", task.requiredTask.description)
        //     })
        // }
        //let tasks = appointment.appointmentTasks;
        return <span className='' > {/* mkDisplay */}

            {
                tasks.length ?
                    tasks.map(task => this.renderTask(task))
                    :
                    ""
            }
        </span>
    }

    renderTask(item) {
        var t = this.props.t
        let dir = getDir()
        var itemClassName, itemDataToggle, itemTitle = ""
        if(item.requiredTask.isRequired) {
            itemClassName = "fs-20 pe-7s-attention basicColorRed";
            itemDataToggle = "tooltip";
            itemTitle = "Required";
        }
        //console.log("itemClassName: ", itemClassName, " itemDataToggle:" , itemDataToggle, " itemTitle" , itemTitle);
        return (
            <li className="minimalkitStyle" key={item.id} id="main" dir={dir}>
                <div className="p-0">
                    <input className='checkboxMinstyle' type="checkbox" id={"check" + item.id} checked={item.approved} onChange={(e) => this.taskHandler(e, item.id)} />
                    <span className={itemClassName} data-toggle={itemDataToggle} title={itemTitle}></span>
                    <span className="itemText">
                        {item.requiredTask.description}
                        {/* {
                            item.isRequired ?     cursor: pointer
                                <FontAwesomeIcon icon="exclamation" className={dir == "rtl" ? "mr-1" : "ml-1"} color={"#ed1c40"}></FontAwesomeIcon>
                                :
                                ""
                        } */}
                    </span>
                    <i id="edit" className={item.approved ? "fs-20 pe-7s-check basicColorAquq" : "fs-20 pe-7s-check basicColorRed"} data-toggle="tooltip" title={t("Users.done")} />

                </div>
            </li>
        );
    }

    taskHandler(e, id) {
        console.log("task: ", e.target.checked, " id: ", id)
        let updatedTasks = this.state.tasks;
        let itemToUpdate = updatedTasks.find(x => x.id === id);
        itemToUpdate.approved = e.target.checked;
        for(let i = 0; i < updatedTasks.length; i++){
            if(updatedTasks[i].id === itemToUpdate.id){
                updatedTasks[i] = itemToUpdate
            }
        }
        //updatedTasks.push(itemToUpdate)
        this.setState({ tasks: updatedTasks });
        this.props.saveTasks(updatedTasks)

    }
    render() {
        let tasks = this.state.tasks;
        return <div className="container fs-14">
            <div className="row">
                <div className="col-12">
                    <form>

                        {this.renderMinimalKitTasks(tasks)}
                    </form>

                    <div className="row">
                        <i id="add-task" className="fs-20 col-1 pe-7s-plus basicColorRed" title="Add task" style={{ cursor: "pointer" }} onClick={this.showAddTask} />
                    </div> 

                    {this.state.isAddTask ?
                        <div >
                            <br />
                            <p className='minimalkitStyle p-0' >
                                <input autoFocus type="search" id="descriptionTask" placeholder="Description task" className="form-control col-7" value={this.state.descriptionTask} onChange={this.handelChange} /> <br />
                                <input className='checkboxMinstyle' type="checkbox" onChange={(e) => this.setState({ isRequired: e.target.checked })} />
                                <span className='itemText'>Is required task</span><br />
                                <button className="btn btn-light font-pr col-2" disabled={this.state.disabledBtn} onClick={this.addTask}> save</button>
                            </p>
                        </div>
                        : ""}
                    <div className="row justify-content-end">
                        {
                            //<button className="btn btn-primary font-pr" onClick={this.saveOperationStatuse}>
                            //    save
                            //</button>
                        }
                    </div>
                </div>
            </div>

        </div>

        }
}
export default Task;
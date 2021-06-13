import React, { Component } from 'react'
import {Table, Col, Row, Card, Button, Modal, Form } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import ModalM from '../serviceType/AddTast'

class RequiredTasks extends Component {
  
    constructor(props){
        super(props);
        this.state = {
            tasks: this.props.listTasks,
            selectedTask: this.props.serviceType,
            taskTab: "task"
            // show: true
        }
        // console.log(`from Tasks\n `,props) -> good
    }

    // handleEdit = () => {
    //     alert("not available right now")
    // }

    handleClose = () => {
        //this.setState({ show: false });
      }

    handleAddTask = (serviceType) => {
        //alert("add task")  -> work good
        console.log("serviceType: ",serviceType);
        //this.setState({ show: true });

    }

    handleMouseEnter = (time) => {
        // console.log("Mouse over!!!");
        //console.log("time: ", time);
        const str = `<div>${time}</div>`
        //console.log(str);
        //return `<div>${time}</div>`;
    }

    handleMouseLeave = () => {
        // console.log("Mouse out!!!"); 
    }

    render() {
    //console.log(this.props) -> good
    
    return (
        // "text-center"
        <div className="p-2 m-2 ">
            {
                this.props.listTasks.map((item , i) => 
                    item.indexT == this.props.serviceType ? 
                            item.tasks.map((subItem , j) => {
                                //console.log(subItem);
                                // const hour = subItem.hoursBeforeToAlert
                                return(
                                    <div className="card card-task" key={subItem.id}>
                                        {/* "worn card-body" */}
                                        <div className="">
                                            <p className="card-text text-right worn">
                                                <span>
                                                    <span className="strong p-2 ">משימה: </span>
                                                    {subItem.description}
                                                </span>
                                                {/* text-left */}
                                                <span className="">
                                                    <FontAwesomeIcon icon="angle-down" className="m-2" data-toggle="tooltip" title="התראות" data-toggle="collapse" data-target={"#collap" + subItem.id} aria-expanded="false" aria-controls={subItem.id}/>
                                                    {/* <button className="btn btn-primary btn-sm" type="button" data-toggle="collapse" data-target={"#collap" + subItem.id} aria-expanded="false" aria-controls={subItem.id}>התראות</button> */}
                                                    <FontAwesomeIcon icon="plus-circle" className="m-2" data-toggle="tooltip" title="הוסף משימה!" onClick={() => {this.handleAddTask(this.props.serviceType)}}
                                                        data-toggle="modal" data-target={"#editTask"} />
                                                </span>
                                            </p>
                                            <div className="collapse" id={"collap" + subItem.id}>
                                                <div className="p-1">
                                                    {
                                                        subItem.hoursBeforeToAlert.map((sub , k) => {
                                                            return (
                                                                <p className="card-text text-right">
                                                                    <span className="strong p-2">התראה: </span>
                                                                    יש לשלוח התראה בנוסח: "{subItem.description} " {sub.hour == 0 ? sub.befor : sub.hour + " " + sub.befor } 
                                                                    {/* {sub.message + " " }  <span className="strong">{sub.hour}</span> {" " + sub.befor} */}
                                                                </p>
                                                            );
                                                        }
                                                    )} {/*map3*/}
                                                </div>
                                            </div>
                                            {/* {modal}  */}                                            
                                            <ModalM modalId={"editTask"} serviceType={this.props.serviceType} serviceName={this.props.serviceName} tab={this.state.taskTab} ></ModalM>
                                        </div>
                                    </div>
                                )
                            })//map-2
                            : ""
                )
            }
            

             {/* {modal}  */}
            {/* <Table className="table text-center" striped  hover> 
                <thead className="">
                    <tr className="">
                        <th className="fs-16">#</th>
                        <th className="fs-16">התראה</th>
                        <th className="fs-16">ניהול</th>
                        <th>
                            <FontAwesomeIcon icon="plus-circle" data-toggle="tooltip" title="הוסף משימה!" onClick={() => {this.handleAddTask(this.props.serviceType)}}
                            data-toggle="modal" data-target={"#editTask"} />
                        </th> */}
                        {/* { this.state.show? <ModalM modalId={"editTask"} showButton={false} /> : "" } */}
                        {/* <ModalM modalId={"editTask"} serviceType={this.props.serviceType} serviceName={this.props.serviceName} tab={this.state.taskTab} ></ModalM> */}

                        {/* <th></th> */}
                    {/* </tr>
                </thead>
                <tbody>
                {
                    this.props.listTasks.map((item , i) => 
                        item.indexT == this.props.serviceType ? 
                            item.tasks.map((subItem , j) => {
                                console.log(subItem);
                                 const hour = subItem.hoursBeforeToAlert
                                return(
                                    onMouseEnter={()=>{this.handleMouseEnter(hour)}} onMouseLeave={this.handleMouseLeave}
                                    <tr key={subItem.id} className="" >
                                        <td>{subItem.id}</td>
                                        <td>{subItem.description}</td> 
                                        <td> */}
                                            {/* onClick={this.handleEdit} */}
                                            {/* <button className="btn btn-primary btn-sm" type="button" data-toggle="collapse" data-target={"#collap" + subItem.id} aria-expanded="false" aria-controls={subItem.id}>התראות</button>
                                        </td>
                                        <td></td> */}
                                        {/* <td className="" key={subItem.id}>{subItem.description}</td> */}
                                        {/* <div className="collapse" id={"collap" + subItem.id}>
                                            <div className="p-1">
                                                {
                                                    subItem.hoursBeforeToAlert.map((sub , k) => {
                                                        return (
                                                            <p >{sub.message + " " + sub.hour + " " + sub.befor}</p>
                                                        );
                                                    }
                                                )} //map3 
                                            </div>
                                        </div>
                                    </tr>
                                )
                                        
                            })//map2
                            : ""
                        ) 
                    }
                </tbody>
            </Table>*/}
            {/* <tr>
                        <dt>1</dt>
                        <td>יש להפסיק לאכול פירות וירקות 120 שעות לפני הבדיקה</td>
                        <td>
                            <button className="btn btn-primary btn-sm" onClick={this.handleClick}>עריכה</button>
                        </td>
                    </tr>

                    <tr>
                        <dt>2</dt>
                        <td>יש לקחת חומר משלשל 36 שעות לפני הבדיקה - מנה ראשונה</td>
                        <td>
                            <button className="btn btn-primary btn-sm" onClick={this.handleClick}>עריכה</button>
                        </td>
                    </tr>

                    <tr>
                        <dt>3</dt>
                        <td>יש להתחיל לצום - צום מלא!</td>
                        <td>
                            <button className="btn btn-primary btn-sm" onClick={this.handleClick}>עריכה</button>
                        </td>
                    </tr>

                    <tr>
                        <dt>4</dt>
                        <td>יש להתחיל לצום - צום מלא, אין לשתות משקאות ממותקים כולל קפה ותה! אלא מים בלבד!</td>
                        <td>
                            <button className="btn btn-primary btn-sm" onClick={this.handleClick}>עריכה</button>
                        </td>
                    </tr> */}
      </div>
    )//return
  }
}

export default RequiredTasks

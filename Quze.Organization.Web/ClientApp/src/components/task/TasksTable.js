import React, { Component } from 'react'
import {Table, Col, Row, Card, Button  } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { withI18n } from 'react-i18next';

class TasksTable extends Component {
    constructor(props){
        super(props);
        this.state = {
            listT: []
        };
            }

    componentDidMount() {
        if(this.props.tasksList){
            this.setState({
                listT: this.props.tasksList
            })
        }
        // let lis = this.state.listT;
        // let len = this.state.listT.length;
        // for(let i = 0; i < lis.length; i++){ //good!!!
        //     console.log(lis[i].id + "\n")
        //     console.log(lis[i].name + "\n")
        //     console.log(lis[i].age + "\n")
        //     console.log(lis[i].gender + "\n")
        //     console.log(lis[i].phone + "\n")
        //     console.log(lis[i].email + "\n")
        //     // console.log(lis[i].title + "\n") //[object Object]
        // }
        // this.setState({
        // })
    }

    handleClick = () => {
        var t=this.props.t;
        console.log(t("tasks.Done"));
    }

   render() {
       var t=this.props.t;
     return (
        <div className="p-2 m-2 text-center">
            {/* { this.props.listTasks?this.props.listTasks.length:"no" } */}
            {
                this.props.tasksList.length ? 
                <Table className="table" striped  hover> 
                    <thead className="">
                        <tr className="">
                            <th className="fs-16">#</th>
                            <th className="fs-16">{t("tasks.title")}</th>
                            <th className="fs-16">{t("tasks.Details")}</th>
                            <th className="fs-16">{t("tasks.Meetingnumber")} </th>
                            <th className="fs-16"> {t("tasks.treatednottreated")}</th>
                            {/* <th className="fs-16">עמית</th>
                            <th className="fs-16">סוג שירות</th>
                            <th className="fs-16">נותן שירות</th>
                            <th className="fs-16">תאריך</th>
                            <th className="fs-16">פרטים</th>
                            <th className="fs-16">מייל</th>
                            <th className="fs-16">טלפון</th>
                            <th className="fs-16">ג / א</th>
                            <th className="fs-16">טופל</th> */}
                        </tr>
                    </thead>
                    <tbody>
                        {
                            this.props.tasksList.map((item , index) => 
                            // className={item.isUrgent ? "border-danger strong" : ""}
                            <tr className="strong" key={item.id} > {/* key={item.id} */}
                                <td>{item.id}</td> {/* {item.id} */}
                                <td>{item.title}</td> {/* {item.name} ({item.age}) */}
                                <td>{item.content} {/* {item.serviceType} */}
                                    {item.taskType == 2 ? <FontAwesomeIcon icon="exclamation-circle"/> : ""}
                                </td> 
                                <td>{item.appointmentId ? item.appointmentId : ""}</td>
                                {/*<td>{item.providerService}</td>
                                <td>{item.date}</td>
                                {
                                typeof(item.details == 'object') ?
                                    <td>
                                            
                                        {item.details.map((subItem , j) => 
                                            <div className="" key={j}>
                                                {subItem.description}
                                            </div>
                                            
                                            )
                                        } //map2 
                                        {item.noShow ? <FontAwesomeIcon icon="exclamation-circle"/> : ""}
                                    </td>
                                    : null 
                                }*/}
                                {/* <td>{item.email}</td>
                                <td>{item.phone}</td>
                                <td>{item.gender}</td> */}
                                <td>
                                    <button className="btn btn-primary btn-sm" onClick={() => {
                                        this.props.handelDelete(item.id);
                                        this.forceUpdate();
                                        }}>{t("tasks.treated")}</button>
                                </td>
                            </tr>
                           )
                        }
                        
                    </tbody>
                </Table>
                :
                <div className="text-center">{t("tasks.Notaskstodo")}</div>
            }     
        </div>
      )
    }//render
}//tasksTable

export default withI18n()(TasksTable);
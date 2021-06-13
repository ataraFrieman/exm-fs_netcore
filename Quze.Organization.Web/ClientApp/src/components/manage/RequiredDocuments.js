import React, { Component } from 'react'
import {Table, Col, Row, Card, Button  } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import ModalM from '../serviceType/AddTast'

class RequiredDocuments extends Component {
    
  constructor(props){
        super(props);
        this.state = {
            documents: this.props.listDocuments,
            selectedTask: this.props.serviceType,
            docTab: "document"
            // show: false
        }
        // console.log(`from doc\n `,props) -> good
  }
  
//   handleEdit = () => {
//     alert("not available right now")
//   }

  handleAddDocument = (serviceType) => {
      //alert("add document")  -> work good
      console.log("serviceType: ",serviceType)
  }

  handleMouseEnter = (time) => {
    // console.log("Mouse over!!!");
    //console.log("time: ", time);
    const str = `<div>${time}</div>`
    //console.log(str);
    return `<div>${time}</div>`;
  }

  handleMouseLeave = () => {
        // console.log("Mouse out!!!");
  }
  //handleMouseEnter = () => this.setState({hovering : true});
  //handleMouseLeave = () => this.setState({hovering : false});

  render() {
    // const style = {
    //     backgroundColor: this.state.hovering ? "red" : undefined
    // };
    
    //console.log(this.props) -> good
     

    return (
        <div className="p-2 m-2 text-center">
            {/* <div style={style} onMouseEnter={handleMouseEnter} onMouseLeave={handleMouseLeave}>
                Hover over me!
            </div> */}
             {
                this.props.listDocuments.map((item , i) => 
                    item.indexT == this.props.serviceType ? 
                            item.documents.map((subItem , j) => {
                                //console.log(subItem);
                                // const hour = subItem.hoursBeforeToAlert
                                return(
                                    <div className="card card-task" key={subItem.id}>
                                        {/* "worn card-body" */}
                                        <div className="">
                                            <p className="card-text text-right worn">
                                                <span>
                                                    <span className="strong p-2 ">מסמך: </span>
                                                    {subItem.description}
                                                </span>
                                                {/* text-left */}
                                                <span className="">
                                                    <FontAwesomeIcon icon="angle-down" className="m-2" data-toggle="tooltip" title="התראות" data-toggle="collapse" data-target={"#collap" + subItem.id} aria-expanded="false" aria-controls={subItem.id}/>
                                                    {/* <button className="btn btn-primary btn-sm" type="button" data-toggle="collapse" data-target={"#collap" + subItem.id} aria-expanded="false" aria-controls={subItem.id}>התראות</button> */}
                                                    <FontAwesomeIcon icon="plus-circle" className="m-2" data-toggle="tooltip" title="הוסף מסמך!" onClick={() => {this.handleAddTask(this.props.serviceType)}}
                                                        data-toggle="modal" data-target={"#editDoc"} />
                                                </span>
                                            </p>
                                            <div className="collapse" id={"collap" + subItem.id}>
                                                <div className="p-1">
                                                    {
                                                        subItem.hoursBeforeToAlert.map((sub , k) => {
                                                            return (
                                                                <p className="card-text text-right">
                                                                    <span className="strong p-2">התראה: </span>
                                                                    יש לשלוח התראה בנוסח: "{subItem.description}" {sub.befor == "עכשיו" ? sub.befor : sub.hour + " " + sub.befor }
                                                                    {/* {sub.message + " " }  <span className="strong">{sub.hour}</span> {" " + sub.befor} */}
                                                                </p>
                                                            );
                                                        }
                                                    )} {/*map3*/}
                                                </div>
                                            </div>
                                            {/* {modal}  */}                                            
                                            <ModalM modalId={"editDoc"} serviceType={this.props.serviceType} serviceName={this.props.serviceName} tab={this.state.docTab} ></ModalM>
                                        </div>
                                    </div>
                                )
                            })//map-2
                            : ""
                )
            }
            {/* <Table className="table" striped  hover> 
                <thead className="">
                    <tr className="">
                        <th className="fs-16">#</th>
                        <th className="fs-16">מסמך</th>
                        <th className="fs-16">ניהול</th>
                        <th>
                            <FontAwesomeIcon icon="plus-circle" data-toggle="tooltip" title="הוסף מסמך!" onClick={() => {this.handleAddDocument(this.props.serviceType)}}
                            data-toggle="modal" data-target={"#editDoc"} />
                        </th>
                        <ModalM modalId={"editDoc"} serviceType={this.props.serviceType} serviceName={this.props.serviceName} tab={this.state.docTab}></ModalM> */}
                        {/* <th></th> */}
                    {/* </tr>
                </thead>
                <tbody className="">
                {
                    this.props.listDocuments.map((item , i) => 
                        item.indexT == this.props.serviceType ? 
                            item.documents.map((subItem , j) => {
                                console.log(subItem);
                                const hour = subItem.hoursBeforeToAlert
                                return(
                                    <tr key={subItem.id} className=""  onMouseEnter={()=>{this.handleMouseEnter(hour)}} onMouseLeave={this.handleMouseLeave}>
                                        <td>{subItem.id}</td>
                                        <td>{subItem.description}</td>
                                        <td> */}
                                            {/* onClick={this.handleEdit} */}
                                            {/* <button className="btn btn-primary btn-sm" type="button" data-toggle="collapse" data-target={"#collap" + subItem.id} aria-expanded="false" aria-controls={subItem.id}>התראות</button>
                                        </td>
                                        <td></td>
                                        <div className="collapse" id={"collap" + subItem.id}>
                                            <div className="p-1">
                                                {
                                                    subItem.hoursBeforeToAlert.map((sub , k) => {
                                                        return (
                                                            <p >{sub.message + "  " + sub.hour + " " + sub.befor}</p>
                                                        );
                                                    }
                                                )}
                                            </div>
                                        </div>
                                    </tr>
                                ) 
                            }) map2
                            : ""
                        )
                        
                }*/}
                    {/* <tr>
                        <dt>1</dt>
                        <td>אישור חברת הביטוח עבור תשלום רופא</td>
                        <td>
                            <button className="btn btn-primary btn-sm" onClick={this.handleClick}>עריכה</button>
                        </td>
                    </tr>
                    <tr>
                        <dt>1</dt>
                        <td>אישור חברת הביטוח לשיפוי בית החולים</td>
                        <td>
                            <button className="btn btn-primary btn-sm" onClick={this.handleClick}>עריכה</button>
                        </td>
                    </tr> */}
                {/* </tbody>
            </Table> */}

    
        </div>
    )//return
  }
}

export default RequiredDocuments

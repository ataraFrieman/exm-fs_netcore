import React, { Component } from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { withI18n } from 'react-i18next';

class TasksCard extends Component {
  
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
        });
    }
  }

  render() {
      var t=this.props.t;
    return (
        <div className="card-deck p-2 m-2">
            {
                //props.tasksList -> state.listT
                this.props.tasksList.length ? 
                this.props.tasksList.map((item , index) =>
                    <div className="col-sm-4" key={item.id}> {/* key={item.id} */}
                        <div className="card card-task mb-2" > {/* className={item.isUrgent ? "border-danger card card-task mb-2" : "card card-task mb-2"} */}
                            {/* card-header */}
                            <div className="card-header text-right worn">
                                {item.title}
                                {/* {
                                typeof(item.title == 'object') ? 
                                    item.title.map((subbItem , j) => 
                                        <div >
                                            {subbItem.descrissption} 
                                        </div>
                                    )//map2 
                                    : "" 
                                } */}
                                <span>
                                    {item.taskType == 2 ? <FontAwesomeIcon icon="exclamation-circle"/> : ""}
                                    <button type="button" className="btn btn-primary btn-sm m-1" onClick={() => {this.props.handelDelete(item.id)}}>{t("tasks.treated")}</button>
                                </span>
                            </div>
                            {/* card-body */}
                            <div className="card-body text-right strong">
                                <span>{item.content}</span>
                            {/* 
                                <span className="">{item.name} ({item.age}), {item.gender}</span> <br/>
                                <span className="">{item.date}</span> <br/>
                                <span className="">
                                { 
                                    typeof(item.details == 'object') ?
                                    <td>
                                        {item.details.map((subItem , k) => 
                                        <div className="">
                                                {subItem.description}
                                            </div>
                                    
                                            )
                                        }  //map3 
                                        
                                    </td>
                                    : null 
                                }
                                </span> <br/> 
                                <span className="">{item.phone}</span> <br/>
                                <span className="">{item.providerService}</span> */}
                            </div>
                        </div>
                    </div>
                ) : 
                <div className="container"> 
                    <div className="text-center">{t("tasks.Notaskstodo")}</div>
                </div>
            }
    
        </div>
      )
    }//render
}//tasksCard

export default withI18n()(TasksCard);
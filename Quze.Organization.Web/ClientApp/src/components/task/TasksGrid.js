import React, { Component } from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

class TasksGrid extends Component {

  //how to add few defrent element in the same line/element 
  //example: https://stackoverflow.com/questions/16998947/putting-text-and-images-on-same-line-within-a-list-item-html-css        
  
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
        
      <div className="container p-2">
           {
               //props.tasksList -> this.state.listT 
               this.props.tasksList.length ?
                  this.props.tasksList.map((item , index) => 
                    <div className="card card-task mb-2 text-right" key={item.id}> {/* key={item.id} {className=item.isUrgent ? "border-danger card card-task mb-2 text-right" : "card card-task mb-2 text-right" } */}
                        {/* card-header */}
                        <div className= "card-header worn">
                            {item.title}
                            {/* {
                            typeof(item.title == 'object') ? 
                                item.title.map((subbItem , j) => 
                                    <div className="" key={j}>
                                        {subbItem.description} 
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
                        <div className="strong">  
                            <span className="p-2">{item.content}</span> {/* {item.name} ({item.age}), {item.gender}, */}
                            {/* <span className="p-2">{item.phone},</span> 
                            <span className="">{item.providerService},</span>
                            <span className="p-2">{item.date}</span>
                            <span className="p-2">
                            { 
                                typeof(item.details == 'object') ?
                                <ul className="">
                                    {
                                        item.details.map((subItem , k) => 
                                    <div className="" key={k}>
                                            {subItem.description}
                                        </div>
                                
                                        )
                                    }  //map3 
                                    
                                </ul>
                                : null 
                            }
                            </span> */}
                        </div>
                    </div>
                ) : 
                <div className="text-center">{t("tasks.Notaskstodo")}</div>
            }
      </div>
    )
  }
}

export default TasksGrid
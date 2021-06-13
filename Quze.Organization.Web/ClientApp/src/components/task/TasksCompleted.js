import React, { Component } from 'react'
import { withI18n } from 'react-i18next';
//import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';


// insted props.listCompleted
const TasksCompleted = ({listCompleted, handleAdd}) => {
  
   //handleAdd
  // constructor(props){
  //   super(props);
  //   this.state = {
  //     tasksCompleted: this.props.listCompleted
  //   }
  // }
    //console.log(props)
    const todoList = listCompleted.length ? (
      // <div className="">
            // this.state || props.tasksCompleted
            listCompleted.map((item, index) => {
              item=item[0];
              console.log(item)
              return(
                <div className="card card-task text-right mb-2" key={index}> {/*key={item.id} className={item.isUrgent ? "border-danger card card-task mb-2" : "card card-task text-right mb-2" */}
                    <div className="card-header  worn">
                       {item.title}
                       {/* {
                        typeof(item.title == 'object') ? 
                            item.title.map((subbItem , j) => 
                                <div className="">
                                  {subbItem.description} 
                               </div>
                            )//map2 
                            : "" 
                        }
                         {item.noShow ? <FontAwesomeIcon icon="exclamation-circle"/> : ""} 
                          <button type="button" className="btn btn-primary btn-sm" onClick={() => {handleAdd(item.id)}}>שחזר to fix</button> */}
                    </div>
                    {/* card-body  */}
                    <div className="card-body strong">  
                        <span className="p-2">{item.content}</span> {/* {item.name} ({item.age}), {item.gender} */}
                        {/*<span className="p-2">{item.phone},</span>
                        <span className="p-2">{item.providerService},</span>
                        <span className="p-2">{item.date}</span> 
                        <span className="p-2">
                        { 
                            typeof(item.details == 'object') ?
                            <ul>
                                {
                                  item.details.map((subItem , k) => 
                                    <div className="">
                                      {subItem.description}
                                    </div>
                        
                                  )
                                }   //map3 
                            </ul>
                            : null 
                        }
                        </span> */}
                    </div>
                </div>
              )//return
          })
    ) : (
      <div className="text-center"></div>
      // {t("tasks.Notaskshasbeencompleted")}
    )
    // render() {
      // var t=this.props.t;
    return (
        <div className="container p-2">
            {todoList}
        </div>
    )//return

  // }//render
}

export default withI18n()(TasksCompleted);

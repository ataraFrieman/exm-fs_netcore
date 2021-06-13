import React, { Component } from 'react';
import "../../css/site.css"
import "../../css/general.css"
import EditTask from './EditTask';
import * as http from '../../helpers/Http';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { withI18n } from 'react-i18next';
import { getFellowModalData } from "../../helpers/ModalData";


export class ViewReqiuredTasks extends Component {
    constructor(props) {
        super(props);
       
    }
  
    click = (taskId) => {
        console.log(taskId);
        console.log(this.state);

        const data = {
            EntityId: taskId,
        }

        http.deleteX('api/RequiredTasks', data, null)
            .then(response => {
                if (!response || !response.length)
                    return;
                this.setState({
                    scrolledList: response,
                    loading: false,
                })
            })
    }
   

   
  


    render = () => {
        var t = this.props.t;


        
        var list = this.props.loadingTaskList == false ? this.props.list.map(
            line =><EditTask
                key={line.id}
                task={line}
                tab="task"
                reloadData={this.props.getTasks}
                type="task"
                delete={this.click}
                serviceName={this.props.serviceName}
                t={this.props.t}
            />  
        ) :"";

        return (
            <div className="col-12">
              <span>{list}</span>
            </div>
        );
    }
};

export default withI18n()(ViewReqiuredTasks);

import React, { Component } from 'react';
import "../../css/site.css"
import "../../css/general.css"
import { withI18n } from 'react-i18next';



export class ButtonServiceType extends Component {
    constructor(props) {
        super(props);
    }

    click = () => {
        this.props.SetServiceTypeToPresent(this.props.TreeNode);
    }

    
    render = () => {
        var t = this.props.t;
        return <span> 
            <div  className={" rounded-0 border-wight btn btn-default col-lg-2 col-md-2 col-sm-4 m-1 bg-white text-primary font-pr "}>
            <div className=" card-body ">
                <div className="row justify-content-end">
                    <div className="" data-toggle="modal" onClick={(evt) => {this.props.openCurrent(this.props.TreeNode,this.props.list);}} >
                        <i id="edit" className={"icon fs-24 pe-7s-note"} data-toggle="tooltip" title={t("Users.edit")} />
                    </div>
                    <i id="open" className={"icon fs-24 pe-7s-bottom-arrow"} onClick={(evt) => { this.click(); }} data-toggle="tooltip" title={t("SelectServiceT.serv") + " " + this.props.TreeNode.content.description} />
                </div>
                {this.props.TreeNode.content.description}
            </div>
        </div>
        </span>
    }
};

export default withI18n()(ButtonServiceType);

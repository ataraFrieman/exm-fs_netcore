import React, { Component } from 'react';
import history from '../helpers/History';
import { FormInputs } from './FormInputs';

export class Modal extends Component {

    constructor(props) {
        super(props);
        this.state = {
            modalId: props.modalId,
            showButton: props.showButton,
            item: props.item,
            formInputsData: null
        };
        this.handleInputChange = this.handleInputChange.bind(this);
        this.hideModal = this.hideModal.bind(this);
        this.validation = this.validation.bind(this);
        this.valid = [];
    }

    handleInputChange(event) {
        var newitem = this.props.item;
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;
        if (newitem) {
            var isUserKey = name in newitem ? true : false;
            if (isUserKey)
                newitem[name] = value;
            else
                newitem[name] = value;
        }
        else
            newitem[name] = value;
        this.setState({ item: newitem });
        if (this.props.updateEvent)
            this.props.updateEvent(newitem);
    }

    validation() {
        let valid = true;
        this.valid.map(f => {
            if (f && f.inputRef && f.inputRef.current && f.inputRef.current.validation && !f.inputRef.current.validation())
                valid = false;
        });
        return valid;
    }

    hideModal() {
        this.setState({ hideModal: true });
        if (this.props.hideModal)
            this.props.hideModal();
    }

    componentWillReceiveProps(nextProps) {
        this.initState(nextProps);
    }

    componentDidMount() {
        this.initState(this.props);
    }

    initState(props) {
        this.setState({
            hideModal: props.hideModal ? true : false,
            formInputsData: props.formInputsData,
            item: props.item
        });
    }
    render() {
        var t=this.props.t;
        var thisObj = this;
        var formInputsData = this.props.updateEvent ? this.props.formInputsData : this.state.formInputsData;
        var item = this.props.updateEvent ? this.props.item : this.state.item;
        return <div className="">

            {this.state.showButton ? <button type="button" className="btn" data-toggle="modal" data-target={"#" + this.state.modalId}>
                </button> : ""}
            {/*  d-block */}
            <div className="modal d-block" tabIndex="-1" role="dialog" id={this.props.modalId} show={"true"}>
                <div className="modal-dialog direction" role="document">
                    <div className="modal-content">
                        <div className="modal-header border-0">
                            <h5 className="modal-title" > </h5>
                            <button type="button" className="close m-0 p-1" data-dismiss="modal" onClick={this.hideModal}>
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">
                            <form className="text-align">
                                {formInputsData ? formInputsData.map((item, index) => {
                                    var props = item.properties;
                                    if (props )
                                        for (var i = 0; i < props.length; i++) {
                                            props[i].onChange = this.handleInputChange;
                                        }
                                    if (item.cols && props)
                                        return <FormInputs
                                            key={index}
                                            ncols={item.cols}
                                            proprieties={props}
                                            ref={(input) => { this.valid.push(input) }}
                                        />;
                                    else
                                        return t("model.no");
                                }
                                ) : "-"}
                            </form>
                        </div>
                        <div className="modal-footer" style={{ justifyContent: "space-between" }}> 
                            <button type="button" className="btn btn-secondary" data-dismiss="modal" onClick={this.hideModal}>{t("model.close")}</button>
                            <input type="submit" className="btn btn-primary" onClick={e => {
                                if (!this.validation())
                                    return
                                else
                                    thisObj.props.saveEvent(e, item);
                            }} value={t("model.save")} />
                        </div>
                    </div>
                </div>
            </div>
        </div>;
    }
}

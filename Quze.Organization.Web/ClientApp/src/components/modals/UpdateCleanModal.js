import React, { Component } from 'react';
import '../../css/Modal.css'


export class UpdateCleanModal extends Component {
    constructor(props) {
        super(props)
        this.state = {

        }

    }



    render() {
        var t = this.props.t
        return (
            <div className="modal d-blocke" id="UpdateCleanModal" tabIndex="-1" role="dialog" data-backdrop="false">
                <div className="modal-dialog" role="document">
                    <div className="modal-content modal">

                        <div className="modal-header border-1 py-0 row justify-content-end">
                            <button type="button" className="close m-0 p-0 col-1" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>

                        <div className="modal-body font-pr">
                            <div>update clean</div>
                        </div>
                        <div className="row modal-footer justify-content-end m-0 p-0 mt-2">
                            <button className="btn btn-outline-primary font-pr m-2"
                               >{t("model.save")}</button>
                        </div>


                    </div>
                </div>
            </div >
        )
    }
}
export default UpdateCleanModal

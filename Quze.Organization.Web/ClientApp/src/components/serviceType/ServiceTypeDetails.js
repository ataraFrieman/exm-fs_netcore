import React, { Component } from 'react';
import "../../css/site.css"
import "../../css/general.css"
import * as http from '../../helpers/Http';
import { withI18n } from 'react-i18next';



export class ServiceTypeDetails extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            title: this.props.title,
            scrolledList: [],
            serviceTypeId: this.props.getID()
        };
    }

    componentDidMount() {
        var id = this.props.getID();

        this.setState({
            serviceTypeId: id
        })

            //http.get('api/ServiceProvidersServiceType/GetSPByServiceType?serviceTypeID=' + id)
            //    .then(response => {
            //        if (!response || !response.length)
            //            return;
            //        console.log(response);
            //        console.log(this.state);
            //        this.setState({
            //            scrolledList: response,
            //            loading: false,

            //        })
            //    }).then(response => console.log(this.state));

    }

    click = () => {
        console.log(this.state);

    }

    render()
    {
        var t = this.props.t;

        var list = this.state.loading == false ?
            this.state.scrolledList.map(
                line => <li className="rounded-0 border-white list-group-item" key={line.id}>
                    {line.id} {line.titledFullName}
                </li>) : "empty";

        var buttons = <div>
            <button onClick={this.click} type="button" className="btn rounded-0 m-4"> עריכה</button>
            <button type="button" className="btn rounded-0 m-4"> הוספת הערה</button>
        </div>;

        var modal = <span>
            <button type="button" className=" btn btn-outline-danger bg-white mb-4 btn-sm col col-3 p p-3 m m-1 rounded-0" data-toggle="modal" onClick={this.click} data-target="#viewSPList" ><strong>מצא רופא</strong>
            </button>
            <div className="modal fade modal-fix" id="viewSPList" tabIndex="0" role="dialog" data-backdrop="false" aria-labelledby="myModalLabel">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="exampleModalLongTitle">נותני השירות הזמינים:</h5>
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                            </button>
                        </div>
                        <div className="modal-body">
                            <ul className="list-group">
                                {list}
                            </ul>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-dismiss="modal">סגור</button>
                        </div>
                    </div>
                </div>

            </div>

        </span>

        var title = <h5 className="text-dark">מצאו רופא המספק את השירות {this.state.title}</h5>;


        return <span> {modal}</span>
    }
};

export default withI18n()(ServiceTypeDetails);

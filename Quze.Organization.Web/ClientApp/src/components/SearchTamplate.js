import React, { Component } from 'react';
import '../css/SearchTemplate.css';
import $ from 'jquery';
export class SearchTemplate extends Component {

    constructor(props) {
        super(props);
        this.state =
            {
                fadeStatuse: true,
            }
    }
    getInput = () => {
        if (this.state.fadeStatuse) {
            $('#content').fadeIn(1000);
        }
        else {
            $('#content').fadeOut(1000);
        }
        this.setState({ fadeStatuse: !this.state.fadeStatuse });
    }

    render() {
        var t = this.props.t;
        return <div className="container">
            <div className="row">
                <div className="col-md-1 searcheePluse text-primary m-1 p-0" onClick={this.props.click} title={this.props.titelnUser}>
                    <i className="pe-7s-plus fs-20 font-weight-bold basicColorRed"></i>
                </div>
                <div className={this.state.fadeStatuse?"col-md-1 searcheePluse text-primary m-1 p-0 ":"col-md-1 nobackground text-primary m-1 p-0"}>
                    <div className="searchee text-primary" onClick={this.getInput}><i className="pe-7s-search fs-20 font-weight-bold basicColorRed"></i></div>
                    <input id="content" style={{
                        display: "none", backgroundColor: "red", width: "300px", paddingTop: "13px",
                        cursor: "text", borderRadius: "30px", height: "40px", fontSize: "15px", textAlign: "center"
                    }} className="font-pr" type="text" placeholder={this.props.placeholdersearch} onChange={this.props.change}></input>
                </div>
            </div>
        </div>
    }
};


export default SearchTemplate;
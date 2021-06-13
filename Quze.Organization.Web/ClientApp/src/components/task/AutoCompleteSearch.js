import React from 'react';
import * as http from '../../helpers/Http';
// import { getUser } from '../../helpers/User'
import history from '../../helpers/History'
import '../../css/AutoCompleteSearch.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { withI18n } from "react-i18next";

//AutoCompleteSearch
class AutoCompleteSearch extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            suggestions: [],
            activeSuggestion: 0,
            showSuggestions: false,
            suggestionsSuggestion: null,
            userInput: ''
        }

    };

    ResultType = {
        SP: 0,
        Fellow: 1,
        ServiceType: 2
    };

    // Event fired when the user presses a key down
    onKeyDown = e => {
        let { activeSuggestion, suggestions } = this.state;

        // User pressed the enter key, update the input and close the
        // suggestions
        if (e.keyCode === 13) {
            this.onSuggestionSelected(activeSuggestion);
        }
        // User pressed the up arrow, decrement the index
        else if (e.keyCode === 38) {

            if (activeSuggestion === 0) {
                return;
            }
            activeSuggestion--;

            this.setState({
                activeSuggestion: activeSuggestion,
                userInput: suggestions[activeSuggestion].name
            });
            console.log(activeSuggestion + " " + suggestions.length)
        }
        // User pressed the down arrow, increment the index
        else if (e.keyCode === 40) {

            if (activeSuggestion === suggestions.length - 1) {
                return;
            }
            activeSuggestion++;
            this.setState({
                activeSuggestion: activeSuggestion,
                userInput: suggestions[activeSuggestion].name
            });
            console.log(activeSuggestion + " " + suggestions.length)
        }
    };

    handleChange = (e) => {
        //this.setState({
        //    text: e.target.value
        //});
        const value = e.target.value; //console.log(e.target) //'e.target' is all the 'input' element
        this.setState({ userInput: value });
        console.log(value); //'e.target.value' ->  good

        if (value !== '' && value.length > 2) {
            let thisObj = this;//save on the objet before toing to the server and can get other "this"
            if (this.controller)
                this.controller.abort();
            this.controller = new AbortController();
            this.signal = this.controller.signal;
            var url = 'api/Organization/GlobalSearch?term=' + value;
            if (this.props.searchType == "fellowSearch")
                url = 'api/Organization/FellowSearch?term=' + value;
            if (this.props.searchType == "serviceTypeSearch")
                url = 'api/Organization/serviceTypeSearch?term=' + value;
            http.get(url, null, null, this.signal)//
                .then(res => {
                    console.log("Response:", res);
                    if (!res || res.length == 0) {
                        thisObj.setState({
                            suggestions: [],
                            showSuggestions: true

                        });
                    } else {
                        thisObj.setState({
                            suggestions: res,
                            showSuggestions: true,
                            activeSuggestion: 0
                        });
                    }
                });//.then
        } else {
            this.setState({
                suggestions: [],
                showSuggestions: false
            });
            return;
        }


    }

    //2. לחיצת עכבר על הצעה
    handleClick = (item, callback) => {
        console.log(item);
        const { suggestions } = this.state;

        for (let index = 0; index < suggestions.length; index++) {
            if (suggestions[index].id == item.id) {
                console.log(index)
                this.setState({
                    activeSuggestion: index,
                    userInput: suggestions[index].name
                });
                this.onSuggestionSelected(index, callback);
            }
        }
    }
    //1. יש באג אם לחצו אנטר ואין תוצאות כלל
    // 2. גם יש בעיה עם לחיצת העכבר על הצעה כלשהי
    onSuggestionSelected = (index, callback) => {
        var t=this.props.t;
        const { suggestions } = this.state;
        //1. תיקון באג עבור אנטר
        if (suggestions.length === 0 /*|| currentSuggestion === "undefined" */) { //no results
            return null;
        }
        const currentSuggestion = suggestions[index];
        this.setState({
            suggestions: [],
            suggestionsSuggestion: [currentSuggestion],
            userInput: currentSuggestion.name
        });
        console.log(currentSuggestion)
        let item = '';
        console.log(this.props.redirectAfterSelect);
        this.setState({ showSuggestions: false })
        if (this.props.redirectAfterSelect == false) {
            if (callback)
                return callback(currentSuggestion);
            else return;
        }
        switch (currentSuggestion.searchResultType) {
            case this.ResultType.Fellow:
                console.log(t("AutoC.Fellows") + currentSuggestion.id);
                //Fellow page
                //this.props.history.push("/login");
                item = { name: currentSuggestion.name, id: currentSuggestion.id, type: currentSuggestion.searchResultType }
                history.push("/fellow", item)
                // window.location.reload();
                break;
            case this.ResultType.SP:
                console.log(t("AutoC.SP") + currentSuggestion.id);
                //ServiceProvider page
                //var  item = { serviceProvider: item, branches: thisObj.props.branches }
                item = { name: currentSuggestion.name, id: currentSuggestion.id, type: currentSuggestion.searchResultType }
                //history.push("/serviceProvider") // -> missing things to send
                // window.location.reload();
                break;
            case this.ResultType.ServiceType:
                console.log(t("AutoC.ST") + currentSuggestion.id);
                //ServiceType page that called "manageScreen"
                item = { name: currentSuggestion.name, id: currentSuggestion.id, type: currentSuggestion.searchResultType }
                history.push("/manageScreen", item)
                // window.location.reload();
                break;

            default:
                console.log(t("AutoC.DefaulthowdidIgethere"));
        }

        window.location.reload();
    }

    handleDeleteClick = () => {
        var t=this.props.t;
        console.log(t("AutoC.deletestr") + this.state.userInput);
        this.setState({
            userInput: "",
            showSuggestions: false,
            suggestions: []
        });
        if (this.props.callback)
            this.props.callback(null);

    }

    renderSwitch = (type) => {
        var t=this.props.t;
        switch (type) {
            case this.ResultType.SP:

                return <FontAwesomeIcon icon="user-md" />;

            case this.ResultType.Fellow:

                //if(gender == "female") 
                //  return <FontAwesomeIcon icon="female"/>
                //else
                return <FontAwesomeIcon icon="male" />;

            case this.ResultType.ServiceType:

                return <FontAwesomeIcon icon="briefcase-medical" />;

            default:
                console.log(t("AutoC.Defaulthowdidyougethere"));
                return;
        }
    }

    renderSuggestion = () => {
        var t=this.props.t;
        const { suggestions, activeSuggestion, showSuggestions } = this.state;
        if (suggestions.length === 0 && showSuggestions) /*|| !showSuggestions */ {
            return<ul>
                    <li className="fs-12 suggestion-empty text-right ">{t("AutoC.noResult")}</li>
            </ul>
        }
        return (
            <ul>  {/* className="suggestion-list" */}
                {
                    suggestions.map((item, index) => {
                        // console.log(item)
                        // let className;
                        // if (index === activeSuggestion) { 
                        //     className = "suggestion-active";
                        // }
                        return (
                            //"suggestion-active" 
                            <li key={index} className={index === activeSuggestion ? "suggestion-active" : ""} onClick={() => this.handleClick(item, this.props.callback)} >
                                {this.renderSwitch(item.searchResultType)}
                                <span className="m-2">{item.name}</span>
                            </li>
                        )
                    }
                    )
                }
            </ul>
        )
    }

    render() {
        const { suggestions, activeSuggestion, userInput } = this.state;

        return (
            <div className={"autoCompleteSearch " + (this.props.classes ? this.props.classes : "")}>
                {/*  */}
                <div className="row m-0">
                    <input type="text"
                        className={"col-11 bg-t "+this.props.inputClasses}
                        id="search"
                        value={userInput}
                        onChange={this.handleChange}
                        onKeyDown={this.onKeyDown}
                        autoComplete="off"
                        placeholder={this.props.placeholder ? this.props.placeholder : this.props.t("AutoSearch.placeHolder")}
                    // placeholder="&#xf002;"
                    />

                    <span className="search-span col-1 justify-content-end p-0 mt-1">
                        {
                            userInput.length == 0 ? <FontAwesomeIcon icon="search" /> : <FontAwesomeIcon icon="times-circle" className="search-delete" onClick={this.handleDeleteClick} />
                        }
                    </span>
                </div>
                {this.renderSuggestion()}


            </div>
        );
    }
}
export default withI18n()(AutoCompleteSearch);
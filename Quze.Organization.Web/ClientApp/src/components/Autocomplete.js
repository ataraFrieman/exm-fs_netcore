import React, { Component, Fragment } from "react";
import * as http from '../helpers/Http';
import '../css/Autocomplete.css';

class Autocomplete extends Component {

    static defaultProps = {
        suggestions: []
    };

    constructor(props) {
        super(props);

        this.state = {
            // The active selection's index
            activeSuggestion: 0,
            // The suggestions that match the user's input
            filteredSuggestions: [],
            // Whether or not the suggestion list is shown
            showSuggestions: false,
            // What the user has entered
            userInput: ""
        };
    }

    // Event fired when the input value is changed
    onChange = e => {
        const { suggestions } = this.props;
        const value = e.currentTarget.value;
        var filteredSuggestions = [];
        if (true) {
            let thisObj = this;//save on the objet before toing to the server and can get other "this"
            if (this.controller)
                this.controller.abort();
            this.controller = new AbortController();
            this.signal = this.controller.signal;
            var url = 'api/Organization/GlobalSearch?term=' + value;
            if (this.props.searchType == "fellowSearch")
                url = 'api/Organization/FellowSearch?term=' + value;
            http.get(url, null, null, this.signal)//
                .then(res => {
                    console.log("Response:", res);
                    if (!res || res.length == 0) {
                        //console.log("there no User,Doctor or Service with this name")
                        console.log("לא קיים רופא,עמית או שירות עם השם הזה")
                        
                            filteredSuggestions= []

                      
                    } else {
                       
                        filteredSuggestions= res
                        // Update the user input and filtered suggestions, reset the active
                        // suggestion and make sure the suggestions are shown
                    }
                    thisObj.setState({
                            activeSuggestion: 0,
                            filteredSuggestions,
                            showSuggestions: true,
                            userInput: value
                        });
                });//.then
        } else {
            //console.log("err,empty value") 
            console.log("שגיאה,מחרוזת ריקה")
         
            filteredSuggestions= []

        }


    };

    // Event fired when the user clicks on a suggestion
    onClick = e => {
        // Update the user input and reset the rest of the state
        this.setState({
            activeSuggestion: 0,
            filteredSuggestions: [],
            showSuggestions: false,
            userInput: e.currentTarget.innerText
        });
    };

    // Event fired when the user presses a key down
    onKeyDown = e => {
        const { activeSuggestion, filteredSuggestions } = this.state;

        // User pressed the enter key, update the input and close the
        // suggestions
        if (e.keyCode === 13) {
            this.setState({
                activeSuggestion: 0,
                showSuggestions: false,
                userInput: filteredSuggestions[activeSuggestion]
            });
        }
        // User pressed the up arrow, decrement the index
        else if (e.keyCode === 38) {
            if (activeSuggestion === 0) {
                return;
            }

            this.setState({ activeSuggestion: activeSuggestion - 1 });
        }
        // User pressed the down arrow, increment the index
        else if (e.keyCode === 40) {
            if (activeSuggestion - 1 === filteredSuggestions.length) {
                return;
            }

            this.setState({ activeSuggestion: activeSuggestion + 1 });
        }
    };

    render() {
        const {
            onChange,
            onClick,
            onKeyDown,
            state: {
                activeSuggestion,
                filteredSuggestions,
                showSuggestions,
                userInput
            }
        } = this;

        let suggestionsListComponent;

        if (showSuggestions && userInput) {
            if (filteredSuggestions.length) {
                suggestionsListComponent = (
                    <ul class="suggestions">
                        {filteredSuggestions.map((suggestion, index) => {
                            let className;

                            // Flag the active suggestion with a class
                            if (index === activeSuggestion) {
                                className = "suggestion-active";
                            }

                            return (
                                <li
                                    className={className}
                                    key={suggestion.id}
                                    onClick={onClick}
                                >
                                    {suggestion.name}
                                </li>
                            );
                        })}
                    </ul>
                );
            } else {
                suggestionsListComponent = (
                    <div class="no-suggestions">
                        <em>No suggestions, you're on your own!</em>
                    </div>
                );
            }
        }

        return (
            <Fragment>
                <input
                    type="text"
                    onChange={onChange}
                    onKeyDown={onKeyDown}
                    value={userInput}
                    className="suggestions"
                />
                {suggestionsListComponent}
            </Fragment>
        );
    }
}

export default Autocomplete;
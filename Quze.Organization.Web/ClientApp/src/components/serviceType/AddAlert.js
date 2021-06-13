import React from 'react';
import * as $ from 'jquery';
import "../../css/general.css"
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import AlertTime from './AlertTime'
import { RadioGroup, Radio } from 'react-radio-group';
import { withI18n } from 'react-i18next';


class AddAlert extends React.Component {

    constructor(props) {
        super(props);
        this.textAreaId = "myTextArea" + this.props.number;
        this.isDeleted = false;
        this.specialFields = [];
        this.specialFieldsIndex = 0;
        this.textArea = React.createRef();

        this.state = ({
            selected: "1",
        })
        this.divForDelete = React.createRef();

    }

    //If you want to add another button, as another option to add, you need to copy- pase one button, chaing the name, innerHtml, and add a live on key-pressed
    insertText = (e) => {
        e.preventDefault();

        this.specialFields[this.specialFieldsIndex++] = e.target.id;

        var txtarea = this.textArea.current; // document.getElementById(this.textAreaId);
        var scrollPos = txtarea.scrollTop;
        var caretPos = txtarea.selectionStart;
        var text = " <" + e.target.name + "> ";

        var front = (txtarea.value).substring(0, caretPos);
        var back = (txtarea.value).substring(txtarea.selectionEnd, txtarea.value.length);
        txtarea.value = front + text + back;
        caretPos = caretPos + text.length;
        txtarea.selectionStart = caretPos;
        txtarea.selectionEnd = caretPos;
        txtarea.focus();
        txtarea.scrollTop = scrollPos;

        var index = txtarea.innerText.indexOf(e.target.name);
        if (index >= 0)
            txtarea.setSelectionRange(index, index + e.name.length);

    }

    keyDown = (e) => {
        this.clearTextAreaMassege();

        var KeyID = e.keyCode;
        switch (KeyID) {
            case 8:
            case 46:
                var txtarea = this.textArea.current;
                var scrollPos = txtarea.scrollTop;
                var caretPos = txtarea.selectionStart;

                var front;
                var back = (txtarea.value).substring(txtarea.selectionEnd, txtarea.value.length);

                if (txtarea.value.length > 7)
                    if (txtarea.value.charAt(caretPos - 1) == '>') {
                        if (txtarea.value.charAt(caretPos - 8) == '<')
                            front = (txtarea.value).substring(0, caretPos - 9);
                        else if (txtarea.value.charAt(caretPos - 11) == '<')
                            front = (txtarea.value).substring(0, caretPos - 12);
                        else if (txtarea.value.charAt(caretPos - 13) == '<')
                            front = (txtarea.value).substring(0, caretPos - 14);
                        else if (txtarea.value.charAt(caretPos - 16) == '<')
                            front = (txtarea.value).substring(0, caretPos - 17);
                        else if (txtarea.value.charAt(caretPos - 14) == '<')
                            front = (txtarea.value).substring(0, caretPos - 15);
                        else if (txtarea.value.charAt(caretPos - 12) == '<')
                            front = (txtarea.value).substring(0, caretPos - 13);
                        else if (txtarea.value.charAt(caretPos - (this.props.serviceName.length + 2)) == '<')
                            front = (txtarea.value).substring(0, caretPos - (this.props.serviceName.length + 3));
                    }

                if (front != null)
                    txtarea.value = front + back;

            default:
                break;
        }

    }

    delete = () => {
        let removeMe = this.divForDelete.current;
        this.isDeleted = true;
        removeMe.remove();
    }

    clearTextAreaMassege = () => {
        this.refs.fillTheTextArea.innerHTML = "";
    }

    clearIntervalMassege = () => {
        this.refs.fillTheInteval.innerHTML = "";
    }

    rdnBtnSelected = (e) => {
        this.setState({ selected: e })
    }

    sendData = () => {
        var t=this.props.t;
        var isValid = true;

        var d = this.textArea.current.value.split(/[<>]/);

        for (var i = 0; i < d.length; i++) {
            switch (d[i]) {

                case "שם המטופל":
                    d[i] = "FellowName";
                    break;
                case "שם נותן השירות":
                    d[i] = "ServiceProviderName";
                    break;
                case "מקום הטיפול":
                    d[i] = "AppointmentAddress";
                    break;
                case "הארגון":
                    d[i] = "OrganizationName";
                    break;
                case "תאריך הטיפול":
                    d[i] = "AppointmentDate";
                    break;
                case "שעת הטיפול":
                    d[i] = "AppointmentTime";
                    break;

                default:
            }

        }

        var massege = "";

        for (var i = 0; i < d.length; i++) {
            massege += d[i];
        }

        if (this.state.selected == 0) {
            this.refs.fillTheAlertType.innerHTML =t("AddAlert.shuldTocompleted") ;
            isValid = false;
        }
        if (massege.length == 0) {
            
            this.refs.fillTheTextArea.innerHTML =t("AddAlert.mustCaption")  ;
            isValid = false;
        }
        if (this.refs.hoursBefore.getTime() == null || this.refs.hoursBefore.getTime() == 0) {
            this.refs.fillTheInteval.innerHTML =t("AddAlert.mustBeforeTreatment") ;
            isValid = false;
        }

        if (!isValid)
            return false;

        let s = {
            AlertTypeID: this.state.selected,
            HoursBeforeToAlert: this.refs.hoursBefore.getTime(),
            Description: massege
        }
        return s;
    }



    render() {
        var t = this.props.t;

        return (
            <div className="form-group border rounded bg-white p-4" ref={this.divForDelete}>
                <div className="form-group" id={this.props.number}>
                    <div className="minus-6-merge">
                        <span className="bg-white px-3"> {t("AddAlert.alertNum")}{this.props.number + 1}</span>
                        <span >
                            <button className=" bg-white border-0 float-right px-3" onClick={this.delete}>
                                <span className="trash">
                                    <span></span>
                                    <i></i>
                                </span>
                            </button>
                        </span>
                    </div>
                </div>
                {/* ---------------------------------------- */}
                <AlertTime t={this.props.t} ref="hoursBefore" number={this.props.number} />
                <br />
                <textarea className="form-control m m-2 text-align"
                    rows="2"
                    id={this.textAreaId}
                    onKeyDown={this.keyDown}
                    ref={this.textArea}
                >

                </textarea>
                <div className="text-danger" ref="fillTheTextArea"></div>
                <span className="center-elemnt">

                    <div className="btn-group" role="group" aria-label="Basic example">
                        <button type="button" onClick={this.insertText} className="btn btn-secondary m-1" id="FellowName" name={t("AddAlert.patientsName")}>{t("AddAlert.patientsName")}</button>
                        <button type="button" onClick={this.insertText} className="btn btn-secondary m-1" id="ServiceProviderName" name={t("AddAlert.serviceProvider")}>{t("AddAlert.serviceProvider")}</button>
                        <button type="button" onClick={this.insertText} className="btn btn-secondary m-1" id="AppointmentAddress" name={t("AddAlert.PlaceOfTreatment")}>{t("AddAlert.PlaceOfTreatment")}</button>
                    </div>
                </span>

                <span className="center-elemnt">

                    <div className="btn-group" role="group" aria-label="Basic example">
                        <button type="button" onClick={this.insertText} className="btn btn-secondary m-1" id="OrganizationName" name={t("AddAlert.organization")}>{t("AddAlert.organization")}</button>
                        <button type="button" onClick={this.insertText} className="btn btn-secondary m-1" id="AppointmentDate" name={t("AddAlert.date")}>{t("AddAlert.date")}</button>
                        <button type="button" onClick={this.insertText} className="btn btn-secondary m-1" id="AppointmentTime" name={t("AddAlert.hour")}>{t("AddAlert.hour")}</button>
                    </div>
                </span>

                <RadioGroup
                    name={"alertRule" + this.props.number}
                    selectedValue={this.state.selected}
                    onChange={this.rdnBtnSelected}>
                    <label>
                        <Radio value="1" />{t("AddAlert.sms")}
                             </label>
                    <label>
                        <Radio value="2" />{t("AddAlert.mail")}
                             </label>
                    <label>
                        <Radio value="3" />{t("AddAlert.aplication")}
                            </label>
                    <label>
                        <Radio value="4" /> {t("AddAlert.voiceCall")}
                            </label>
                </RadioGroup>

            </div >
        );
    }

}

export default AddAlert;
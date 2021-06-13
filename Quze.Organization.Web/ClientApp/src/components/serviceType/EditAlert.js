import React from 'react';
import * as http from '../../helpers/Http';
import * as $ from 'jquery';
import "../../css/general.css"
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import AlertTime from './AlertTime';
import { RadioGroup, Radio } from 'react-radio-group';
import { withI18n } from 'react-i18next';
 

class EditAlert extends React.Component {

    constructor(props) {
        super(props);
        this.textAreaId = "myTextArea" + this.props.number;
        this.isDeleted = false;
        this.changed = false;
        this.specialFields = [];
        this.specialFieldsIndex = 0;
        this.textArea = React.createRef();

        this.state = {
            description: this.props.alert.description,
            selected: this.props.alert.alertTypeID
        };

        this.des = this.props.alert.description;
        console.log(this.props.ref)
    }



    rdnBtnSelected = (e) => {
        this.setState({ selected: e })
        this.clearRadioButtonMassege();
        this.changed = true;

    }

    //If you want to add another button, as another option to add, you need to copy- pase one button, chaing the name, innerHtml, and add a live on key-pressed
    insertText = (e) => {
        e.preventDefault();
        this.changed = true;
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

    componentDidMount = () => {
        this.setState({ selected: "" + this.props.alert.alertTypeID })
    }

    keyDown = (e) => {
        this.changed = true;
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
        var removeButton = document.getElementById(this.props.number);
        this.isDeleted = true;
        removeButton.remove();

        const data = {
            EntityId: this.props.alert.id,
        }

        http.deleteX('api/AlertRules', data, null)
    }

    clearRadioButtonMassege = () => {
        this.refs.fillTheAlertType.innerHTML = "";
    }

    clearTextAreaMassege = () => {
        this.refs.fillTheTextArea.innerHTML = "";
    }

    clearIntervalMassege = () => {
        console.log("hi1")
    }

    hundleTxtChange = (e) => {
        this.setState({
            description: e.target.value,
        })
        this.changed = true;
    }

    sendData = () => {
        var t=this.props.t;
        let isValid = true;
        let time = this.refs.hoursBefore.getTime();
        var d = this.textArea.current.value.split(/[<>]/);

        if (this.state.selected == 0) {
            // ובה למלא את סוג ההתראה"
            // Alert type must be filled
            this.refs.fillTheAlertType.innerHTML =t("EditA.mustBeFilled");
            isValid = 0;
        }
        if (d.length == 0) {
            // "חובה להכניס נוסח להודעה"
            // Required text is required
            this.refs.fillTheTextArea.innerHTML =t("EditA.textIsRequired");
            isValid = 0;
        }

        if (!isValid)
            return false;

        if (this.props.alert.hoursBeforeToAlert == time && this.changed == false)
            return 1;

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

        let s = {
            AlertTypeID: this.state.selected,
            HoursBeforeToAlert: time,
            Description: massege,
            Id: this.props.alert.id
        }

        return s;
    }
    render() {
        var t=this.props.t;
        
        return (<span className="p-1">
            <div className="form-group border rounded bg-white p-4" id={this.props.number}>
                <div className="minus-6-merge">

                      <span className="bg-white px-3">{t("EditA.alertNum")} {this.props.number + 0.5}</span>
                    <span >
                        <button className=" bg-white border-0 float-right px-3" onClick={this.delete}>
                            <span className="trash">
                                <span></span>
                                <i></i>
                            </span>

                        </button></span>
                </div>
                    <AlertTime ref="hoursBefore" number={this.props.number} initialHour={this.props.alert.hoursBeforeToAlert} />

                    <textarea className="form-control m m-2"
                        rows="2"
                        id={this.textAreaId}
                        onChange={this.hundleTxtChange}
                        onKeyDown={this.keyDown}
                        ref={this.textArea}
                        value={this.state.description}
                    >

                </textarea>
                    <div className="text-danger " ref="fillTheTextArea" ></div>
                    <span className="center-elemnt">
                        <div className="btn-group text-center center-elemnt" role="group">
                            <button type="button" onClick={this.insertText} className="btn btn-secondary m-1" id="FellowName" name="שם המטופל">{t("EditA.patientName")}</button>
                            <button type="button" onClick={this.insertText} className="btn btn-secondary m-1" id="ServiceProviderName" name="שם נותן השירות">{t("EditA.serviceProvider")}</button>
                            <button type="button" onClick={this.insertText} className="btn btn-secondary m-1" id="AppointmentAddress" name="מקום הטיפול">{t("EditA.PlaceOfTreatment")}</button>

                        </div>
                    </span>
                    <span className="center-elemnt">
                        <div className="btn-group text-center center-elemnt" role="group">
                            <button type="button" onClick={this.insertText} className="btn btn-secondary m-1" id="OrganizationName" name="הארגון">{t("EditA.organization")}</button>
                            <button type="button" onClick={this.insertText} className="btn btn-secondary m-1" id="AppointmentDate" name="תאריך הטיפול">{t("EditA.date")}</button>
                            <button type="button" onClick={this.insertText} className="btn btn-secondary m-1" id="AppointmentTime" name="שעת הטיפול">{t("EditA.hour")}</button>
                        </div>
                    </span>

                <br/>
                    <RadioGroup
                        name={"alertRule" + this.props.number}
                        selectedValue={this.state.selected}
                        onChange={this.rdnBtnSelected}>
                        <label>
                            <Radio value="1" />{t("EditA.sms")}
                             </label>
                        <label>
                            <Radio value="2" />{t("EditA.mail")}
                             </label>
                        <label>
                            <Radio value="3" />{t("EditA.app")}
                            </label>
                        <label>
                            <Radio value="4" />{t("EditA.ivr")}
                            </label>
                    </RadioGroup>
                    <div className="text-danger" ref="fillTheAlertType"></div>
                </div>
        </span>);
    }

}

export default withI18n()(EditAlert);
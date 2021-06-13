import React, { Component } from 'react';
import '../css/minimalKit.css';
import { getDir } from '../helpers/User';
import { NavModels } from './NavModels';
import Task from './Task';
import Document from './Document';
import Test from './Test'
import * as http from '../helpers/Http';
import * as $ from "jquery";

// import { t } from 'i18next';

class MinimalKit extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isChecked: false,
            form: {
                operationId: props.appointment.operationId,
                appointmentId: props.appointment.id,
                docs: props.appointment.appointmentDocs,
                tasks: props.appointment.appointmentTasks,
                tests: props.appointment.appointmentTests
            },
            // premitted: ["pdf", "docx", "doc", "png", "jpg", "jepg"],
            navHeadlines: [],//t('Minimalkit.documents')
            minimalKitMode: "Document"
        };
        this.cancelMinimalKit = this.cancelMinimalKit.bind(this);
        this.modesOfMinimalKit = this.modesOfMinimalKit.bind(this);
        this.getModalBodyContent = this.getModalBodyContent.bind(this);
        this.onCloseModal = this.onCloseModal.bind(this);
        this.editnavHeadlines = this.editnavHeadlines.bind(this);
        //make one function instead 3
        this.saveDocuments = this.saveDocuments.bind(this);
        this.saveTasks = this.saveTasks.bind(this);
        this.saveTests = this.saveTests.bind(this);
        this.saveMinimalKit = this.saveMinimalKit.bind(this);

    }

    editnavHeadlines() {
        var doc=null, task=null, test=null;
        if (this.props.appointment.appointmentDocs !== null&&this.props.appointment.appointmentDocs.length!=0)
            doc = "Document";
        if (this.props.appointment.appointmentTasks !== null && this.props.appointment.appointmentTasks.length!=0)
            task = "Task";
        if (this.props.appointment.appointmentTests !== null && this.props.appointment.appointmentTests.length!=0)
            test = "Test";
        this.setState({ navHeadlines: [doc, task, test] })
       
    }

    componentWillMount() {
        this.editnavHeadlines()
    }

    cancelMinimalKit() {
        let docs = this.props.appointment.appointmentDocs;
        let tasks = this.props.appointment.appointmentTasks;

        //docs.forEach(doc => { doc.approved = false });
        //tasks.forEach(task => { task.approved = false })

    }

    getModalBodyContent(appointment) {
        //let docs = appointment.appointmentDocs;
        //let tasks = appointment.appointmentTasks;
        switch (this.state.minimalKitMode) {
            case this.state.navHeadlines[0]:
                {
                    if (this.props.appointment.appointmentDocs !== null)
                        return <Document
                            appointment={this.props.appointment}
                            docs={this.state.form.docs}
                            saveDocuments={this.saveDocuments}
                            t={this.props.t}

                        //saveStatus={this.props.saveStatus}
                        />
                }
            case this.state.navHeadlines[1]:
                {
                    if (this.props.appointment.appointmentTasks !== null)
                        return <Task
                            appointment={this.props.appointment}
                            tasks={this.state.form.tasks}
                            saveTasks={this.saveTasks}
                            t={this.props.t}

                        //saveStatus={this.props.saveStatus}
                        />
                }
            case this.state.navHeadlines[2]:
                {
                    if (this.props.appointment.appointmentTests !== null)
                        return <Test
                            appointment={this.props.appointment}
                            tests={this.state.form.tests}
                            saveTests={this.saveTests}
                            t={this.props.t}

                        //saveStatus={this.props.saveStatus}
                        />
                }

        }
    }

    modesOfMinimalKit(e) {
        this.setState({ minimalKitMode: e.target.value });
    }

    saveDocuments(updatedDocs) {
        //console.log("save docs")
        let newForm = this.state.form;
        newForm.docs = updatedDocs;
        this.setState({ form: newForm });
    }

    saveTasks(updatedTasks) {
        //console.log("save tasks")
        let newForm = this.state.form;
        newForm.tasks = updatedTasks;
        this.setState({ form: newForm });
    }

    saveTests(updatedTests) {
        //console.log("save tests");
        let newForm = this.state.form;
        newForm.tests = updatedTests;
        this.setState({ form: newForm });
    }

    saveMinimalKit() {
        if (!this.state.form.docs.length && !this.state.form.tasks.length && !this.state.form.tests.length) {
            return;
        }
        //console.log("saving MK!")
        // saveMK(minimalKitData){
        let thisObj = this
        http.post('api/MinimalKit/SaveMinimalKit', this.state.form)
            .then(result => {
                console.log("data: ", result)
                console.dir(result)
                alert("Minimal Kit saved!")
                let updateForm = thisObj.state.form
                updateForm.docs = result.docs;
                updateForm.tasks = result.tasks;
                updateForm.tests = result.tests;
                thisObj.onCloseModal();
                thisObj.props.changeMkStautsAndDetails(result.mkStauts, updateForm);
                thisObj.setState({ form: updateForm })
            })
            .catch(x => { console.log("Error: ", x) })
    }

    onCloseModal = () => {
        //this.setState({ isShowDetailsModal: false });
        $("#MinimalKitModal").modal('hide');
        this.setState({ minimalKitMode: "Document" })
    }

    render() {
        var t = this.props.t;
        let appointment = this.props.appointment
        return (
            <div className="modal fade" id="MinimalKitModal" tabIndex="-1" role="dialog" data-backdrop="false" aria-hidden="true">
                {/* fixed-top overflow-auto */}
                <div className="modal-dialog inner-modal " role="document">
                    <div className="modal-content">
                        <div className="modal-header p-0 m-0 row">

                            <NavModels col-11 navHeadlines={this.state.navHeadlines}
                                threeModesOfEditing={this.modesOfMinimalKit}>
                            </NavModels>
                        <button type="button" className="close m-0 p-0 col-1" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div className="modal-body font-pr">
                        {this.state.minimalKitMode ? this.getModalBodyContent(appointment) : ""}
                    </div>
                    {
                        //<div className="modal-body modalBody">
                        //    {this.renderMinimalKit(appointment)}
                        //</div>
                    }
                    <div className="row modal-footer justify-content-end m-0 p-0 mt-2">
                        <button className="btn btn-outline-primary font-pr m-2"
                            onClick={this.saveMinimalKit}>
                            {t('model.save')}
                        </button>
                    </div>

                </div>
            </div>
            </div >
        );
    }
}

export default MinimalKit;

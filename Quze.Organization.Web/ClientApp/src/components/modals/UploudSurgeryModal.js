import React, { Component } from 'react';
import * as $ from "jquery";
import Http from '../../helpers/Http';
import XLSX from 'xlsx';
import * as http from '../../helpers/Http';
import { getOrganizationId } from '../../helpers/AccountService';
import InputField from './Input';
import { t } from 'i18next';
export class UploadSurgeryModal extends Component {

    constructor(props) {
        super(props);
        this.state = {
            beginTime: new Date(this.props.operationQueue.serviceQueue.beginTime).toISOString().substring(0, 10),
            OperationsList: null,
            fileName: "",
            uploadType: 1//1=plan 2=actual
        };
        this.handelChange = this.handelChange.bind(this);
        this.Upload = this.Upload.bind(this);
        this.setUploadType = this.setUploadType.bind(this);
        this.UploadPlan = this.UploadPlan.bind(this);
    }

    handelChange(e) {
        //console.log("input is: ", e.target.value)
        // this.setState({ [e.target.id]: e.target.value })
        this.setState({ beginTime: e.target.value });

    }

    UploadFile(fileUpload) {
        var t = this.props.t;
        if (fileUpload.currentTarget.files[0]) {

            var fileName = fileUpload.currentTarget.files[0].name.toLowerCase();
            if (fileName.endsWith(".xlsx") || fileName.endsWith(".xls")) {
                this.setState({ fileName: fileUpload.currentTarget.files[0].name });
                if (typeof (FileReader) !== "undefined") {
                    var reader = new FileReader();
                    //For Browsers other than IE.
                    if (reader.readAsBinaryString) {
                        var obj = this;
                        reader.onload = function (e) {
                            obj.ProcessExcel(e.target.result);
                        };
                        reader.readAsBinaryString(fileUpload.currentTarget.files[0]);
                    } else {
                        //For IE Browser.
                        reader.onload = function (e) {
                            var data = "";
                            var bytes = new Uint8Array(e.target.result);
                            for (var i = 0; i < bytes.byteLength; i++) {
                                data += String.fromCharCode(bytes[i]);
                            }
                            this.ProcessExcel(data);
                        };

                        reader.readAsArrayBuffer(fileUpload.files[0]);
                    }
                }
                else {
                    // alert("This browser does not support HTML5.");
                    alert(t("uploadSurgery.notSupport"));
                }
            }
            else {
                // alert("Please upload a valid Excel file.");
                alert(t("uploadSurgery.validExcel"));
            }
        }
    }

    ProcessExcel(data) {
        //Read the Excel File data.
        var workbook = XLSX.read(data, {
            type: 'binary'
        });
        var firstSheet = workbook.SheetNames[0];
        //Read all rows from First Sheet into an JSON array.
        var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[firstSheet], { raw: false });

        this.setState({ OperationsList: excelRows });
    }

    setUploadType(type) { this.setState({ uploadType: type }); }

    Upload() {
        this.state.uploadType === 1 ? this.UploadPlan() : this.UploadActual();
    }

    UploadPlan() {
        var t = this.props.t;
        var isOperationQueu = new Date(this.props.operationQueue.serviceQueue.beginTime).toLocaleDateString() === new Date(this.state.beginTime).toLocaleDateString();
        var OperationsList = this.state.OperationsList;
        for (var i = 0; i < OperationsList.length; i++)
        {
            OperationsList[i].BeginTime = new Date(new Date(this.state.beginTime).toLocaleDateString() + " " + OperationsList[i].BeginTime + ":00").toLocaleString("he-il");
        }


        this.setState({ OperationsList: OperationsList });
        let data = {
            beginTime: new Date(this.state.beginTime),
            Entities: this.state.OperationsList,
            ServiceQueueId: this.props.serviceQueueId !== -1 && isOperationQueu ? this.props.serviceQueueId : -1,
            SortOrderByBeginTime: this.state.OperationsList[0].BeginTime ? true : false,
            OperationQueue: this.props.operationQueue && isOperationQueu ? this.props.operationQueue : null,
            State:this.props.state
        };

        if (new Date(this.state.beginTime) !== "" && this.state.OperationsList !== []) {
            $("#UploadSurgeryModal").modal('hide');
            this.setState({
                organizationId: "",
                OperationsList: null,
                fileName: "",
                file: null
            });
            this.props.load();
            var url = 'api/Operations';
            http.post(url, data)
                .then(res => {
                    this.props.Upload(res);
                    console.log(res);
                });
        }
        else {
            alert(t("uploadSurgery.missingDetails"));
        }
    }

    UploadActual() {
        let data = { ActaulOPList: this.state.OperationsList };

        var a = this.state.OperationsList;
        $("#UploadSurgeryModal").modal('hide');
        this.props.load();
        var url = 'api/Operations/UploadActual';
        http.post(url, data)
            .then(res => {
                this.props.UploadActual(res);
                console.log("actual");
                console.log(res);

            });
    }

    render() {
        var t = this.props.t;
        return (
            <div className="modal d-blocke" id="UploadSurgeryModal" tabIndex="-1" role="dialog" data-backdrop="false">
                <div className="modal-dialog" role="document">
                    <div className="modal-content modal">

                        <div className="modal-header border-1 py-0 row justify-content-end">
                                <button type="button" className="close m-0 p-0 col-1" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                        </div>

                        <div className="modal-body font-pr">
                            <form autoComplete="off" className="text-align">
                                {
                                    //<label >Begin time:</label>
                                    //<input type="time" id="beginTime" placeholder="Begin time:" className="form-control" value={this.state.beginTime} onChange={this.handelChange} />
                                }
                                {this.props.state !== "" ?
                                    <div className="btn-group  btn-group-toggle p-1" data-toggle="buttons" >
                                        <label className="btn btn-sm btn-light border active rounded-0" onClick={(e) => { this.setUploadType(1); }}>
                                            <input type="radio" name="options" id="Plan" autoComplete="off" checked />
                                            {/* Plan */}
                                            {t("uploadSurgery.plan")}
                                        </label>
                                        <label className="btn btn-sm btn-light border rounded-0" onClick={(e) => { this.setUploadType(2); }}>
                                            <input type="radio" name="options" id="Acual" autoComplete="off" />
                                            {t("uploadSurgery.acual")}
                                            {/* Acual */}
                                        </label>
                                    </div> : <span />}

                                <div>
                                    <div className="input-group">
                                        <div className="input-group-prepend">
                                            <span className="input-group-text" id="inputGroupFileAddon01">
                                                {t("uploadSurgery.beginTime")}
                                            </span>
                                        </div>
                                        <div className="custom-file">
                                            <input type="date" id="beginTime" placeholder={t("uploadSurgery.beginTime")} className="form-control" value={this.state.beginTime} onChange={this.handelChange} />
                                        </div>
                                    </div>
                                    <br />

                                    <div className="input-group">
                                        <div className="input-group-prepend">
                                            <span className="input-group-text" id="inputGroupFileAddon01">
                                                <i className="pe-7s-upload  font-weight-bold " ></i>
                                            </span>
                                        </div>
                                        <div className="custom-file">
                                            <input type="file" className="custom-file-input" id="inputGroupFile01"
                                                aria-describedby="inputGroupFileAddon01" onChange={(file) => this.UploadFile(file)}
                                                title={this.state.fileName ? this.state.fileName : t("uploadSurgery.noFileSelected")} />
                                            <label className="custom-file-label" htmlFor="inputGroupFile01">{this.state.fileName ? this.state.fileName : t("uploadSurgery.chooseFile")}</label>
                                        </div>
                                    </div>
                                    {

                                        //<div className="col-5">
                                        //    <input type="file" lang="fr" onChange={(file) => this.UploadFile(file)} />
                                        //</div>
                                    }
                                </div>
                            </form>


                        </div>
                        <div className="row modal-footer justify-content-end m-0 p-0 mt-2">
                            <button className="btn btn-outline-primary font-pr m-2"
                                onClick={this.Upload} >{t("model.save")}</button>
                        </div>


                    </div>
                </div>
            </div >
        );
    }
}

export default UploadSurgeryModal;


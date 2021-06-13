import { getDir } from '../helpers/User';
import React, { Component } from 'react';
import { withI18n } from "react-i18next";
import MinimalKit from '../stores/MinimalKit';
import * as http from '../helpers/Http';

export class Document extends Component {
    displayName = "Document"
    constructor(props) {
        super(props);
        var minimalKitStore = MinimalKit.fromJS();
        this.state = {
            descriptionDoc: "",
            isRequired: false,
            docs: this.props.docs,
            premitted: ["pdf", "docx", "doc", "png", "jpg", "jepg"],
            documents: props.appointment.appointmentDocs,
            minimalKitStore: minimalKitStore,
            isAddDocument: false,
            disabledBtn: true

        };

        this.renderMinimalKitDocuments = this.renderMinimalKitDocuments.bind(this);
        this.addDocument = this.addDocument.bind(this);
        this.showAddDocument = this.showAddDocument.bind(this);
        this.handelChange = this.handelChange.bind(this);
    }

    showAddDocument() {
        if(this.state.isAddDocument == true) {
            this.setState({ isAddDocument: !this.state.isAddDocument, descriptionDoc: "", disabledBtn: true });
        }
        else {
            this.setState({ isAddDocument: !this.state.isAddDocument });
        }
    }

    addDocument() {
        var documents = this.state.documents;

        //var response =  this.state.minimalKitStore.addDocument(this.state.descriptionDoc, true, this.props.appointment.id);
        //if ( response) {
        //    this.showAddDocument();
        //    documents.push(response);
        //}
        if(this.state.descriptionDoc == "")
            return
        else {

            http.post("api/MinimalKit/AddDocument", { description: this.state.descriptionDoc, isRequired: this.state.isRequired, appointmentId: this.props.appointment.id })
            .then(response => {
                this.showAddDocument();
                documents.push(response);
                this.setState({ documents: documents, descriptionDoc: "", isRequired: false });
            })
            .catch(error => { console.log("Error:", error); });
        }
    }


    renderMinimalKitDocuments(docs) {
        //let id = appointment.fellow.id; // ?
        //let docs = appointment.appointmentDocs;
        
        // var appointment = this.props.appointment;
        // if (appointment.appointmentDocs.length > 0) {
        //     appointment.appointmentDocs.forEach(doc => {
        //         console.log("doc: ", doc.requiredDocument.description);
        //     });
        // }


        return <span className='' > {/* mkDisplay */}
            {
                docs.length ?
                    docs.map(doc => this.renderDocument(doc))
                    :
                    ""
            }
        </span>
    }

    renderDocument(item) {
        var t = this.props.t;
        let dir = getDir();
        var itemClassName, itemDataToggle, itemTitle = ""
        if(item.requiredDocument.isRequired) {
            itemClassName = "fs-20 pe-7s-attention basicColorRed";
            itemDataToggle = "tooltip";
            itemTitle = "Required";
        }
        //console.log("itemClassName: ", itemClassName, " itemDataToggle:" , itemDataToggle, " itemTitle" , itemTitle);
        //data-toggle="tooltip" title="Upload file"
        return (
            <li className='minimalkitStyle' key={item.id} dir={dir}>
                <div className="p-0">
                    <input className='checkboxMinstyle' type="checkbox" id={"check" + item.id} checked={item.approved} onChange={(e) => this.documentHandler(e, item.id)} />
                    <span className={itemClassName} data-toggle={itemDataToggle} title={itemTitle}></span>
                    <span className='itemText'>
                        {item.requiredDocument.description}
                        {/* {
                            item.isRequired ?
                                <FontAwesomeIcon icon="exclamation" className={dir == "rtl" ? "mr-1" : "ml-1"} color={"#ed1c40"}></FontAwesomeIcon>
                                :
                                ""
                        } */}
                    </span>
                    <input type="file" id={item.id} onChange={(e) => this.documentHandler(e, item.id)} hidden />
                    <label htmlFor={item.id} className="">
                        <i id="edit" className={item.approved ? "fs-20 pe-7s-upload basicColorAquq" : "fs-20 pe-7s-upload basicColorRed"} data-toggle="tooltip" title="Upload file" style={{ cursor: "pointer" }} />
                    </label>
                    {/* <div className="col-1 mt-2">
                            <i id="edit" className="fs-20 pe-7s-upload  basicColorRed" data-toggle="tooltip" title="Upload file" style={{ cursor: "pointer" }}/>
                    </div> */}
                </div>
                <div>
                    <small className="basicColorRed" id={'massage' + item.id}></small>
                </div>
            </li>
        );
    }

    documentHandler(e, id) {
        var t = this.props.t;
        console.log("doc: ", e.target.checked);
        let updatedDocs = this.state.docs;
        let itemToUpdate = updatedDocs.find(x => x.id === id);
        //let itemToUpdate = this.state.docs.find(x => x.id === id);
        let ifExists = updatedDocs.find(x => x.id === id);
        ifExists ? ifExists = itemToUpdate : updatedDocs.push(itemToUpdate);

        //upload file
        if (e.target.type === "file") {
            let type = e.target.files[0].name.split('.').pop();
            if (!this.state.premitted.includes(type.toLowerCase())) {
                return document.querySelector('#massage' + id).textContent = t('Minimalkit.onlydocOrImg');
            }
            else if (type = e.target.files[0].size > 5000000) {
                return document.querySelector('#massage' + id).textContent = t('Minimalkit.only5mb');
            }
            else document.querySelector('#massage' + id).textContent = "";


            this.getBase64(e.target.files[0])
                .then(b64file => {
                    itemToUpdate.fileContent = b64file;
                });

            itemToUpdate.approved = true;
            for(let i = 0; i < updatedDocs.length; i++){
                if(updatedDocs[i].id === itemToUpdate.id){
                    updatedDocs[i] = itemToUpdate
                }
            }
            this.setState({ docs: updatedDocs });
        }
        //have file
        else if (e.target.type === "checkbox") {
            itemToUpdate.approved = e.target.checked;
            for(let i = 0; i < updatedDocs.length; i++){
                if(updatedDocs[i].id === itemToUpdate.id){
                    updatedDocs[i] = itemToUpdate
                }
            }
            this.setState({ docs: updatedDocs });
        }

        this.props.saveDocuments(updatedDocs)

    }

    getBase64(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => {
                let encoded = reader.result.replace(/^data:(.*;base64,)?/, '');
                if ((encoded.length % 4) > 0) {
                    encoded += '='.repeat(4 - (encoded.length % 4));
                }
                resolve(encoded);
            };
            reader.onerror = error => reject(error);
        });
    }

    handelChange(e) {
        //console.log("input is: ", e.target.value)
        // this.setState({ [e.target.id]: e.target.value })
        if(e.target.value == "")
            this.setState({ descriptionDoc: e.target.value, disabledBtn: true });
        else
            this.setState({ descriptionDoc: e.target.value, disabledBtn: false });
    }

    render() {
        let docs = this.state.docs;
        return <div className="container fs-14">
            <div >
                <form>
                    {this.renderMinimalKitDocuments(docs)}
                </form>

                <div className="row">
                    <i id="add-document" className="fs-20 col-1  pe-7s-plus  basicColorRed" title="Add document" style={{ cursor: "pointer"}} onClick={this.showAddDocument} />
                </div>

                {this.state.isAddDocument ?
                    <div >
                        <br />
                        <p className='minimalkitStyle p-0' >
                            <input autoFocus type="search" id="descriptionDoc" placeholder="Description document" className="form-control col-7" value={this.state.descriptionDoc} onChange={this.handelChange} /><br />
                            <input className='checkboxMinstyle' type="checkbox" onChange={(e) => this.setState({ isRequired: e.target.checked })} />
                            <span className='itemText'>Is required document</span><br />
                            <button className="btn btn-light font-pr col-2" disabled={this.state.disabledBtn} onClick={this.addDocument}> save</button>
                        </p>
                    </div>
                    : ""}

                <div className="row justify-content-end">
                    {
                        //<button className="btn btn-primary font-pr" onClick={this.saveOperationStatuse}>
                        //    save
                        //</button>
                    }
                </div>
            </div>
        </div>;
    }
}
export default Document;

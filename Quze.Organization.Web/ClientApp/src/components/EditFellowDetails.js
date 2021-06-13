import React, { Component } from 'react';
import { render } from 'react-dom';
import { text } from '@fortawesome/fontawesome-svg-core';
import { forEachChild } from 'typescript';

export class EditFellowDetails extends Component {
    constructor(props) {
        super(props)
        this.state = {
            nextPage: "ServiceProvidersDetails",
            BackPage: "SurgeryDetails",
            fellows: this.props.fellows,
            filterList: false,
            //Required
            FellowWeight:this.props.appointment.fellow.weight?this.props.appointment.fellow.weight:0,
            FellowGender: this.props.appointment.fellow.gender,
            IsHBP: false,
            IsFellowDiabetic: false,
            IsXrayDeviceRequired: false,
            formValues: {
                FellowName: this.props.appointment.fellow.fullName,
                FellowCode: String(this.props.appointment.fellow.identityNumber),                //Required 
                FellowAge:this.props.appointment.fellow.age?this.props.appointment.fellow.age.toString():
                    String(Math.floor((new Date() - new Date(this.props.appointment.fellow.birthDate).getTime()) / 3.15576e+10) + 1)                      //Required
            },
            formErrors: {
                FellowName: "",
                FellowCode: "",
                FellowAge: ""
            },
            formValidity: {
                FellowName: false,
                FellowCode: false,
                FellowAge: false
            },
            isSubmitting: false
        };

        this.setFellowDetails = this.setFellowDetails.bind(this);
        this.saveFellowDetails = this.saveFellowDetails.bind(this);
        this.onChangeInput = this.onChangeInput.bind(this);
        this.btnBackFellowDetails = this.btnBackFellowDetails.bind(this);
        this.fillFellowDetails = this.fillFellowDetails.bind(this);
    }

    handleChange = ({ target }) => {
        const { formValues } = this.state;
        formValues[target.name] = target.value;
        this.setState({ formValues });
        this.handleValidation(target);
    };
    handleValidation = target => {
        const { name, value } = target;
        const fieldValidationErrors = this.state.formErrors;
        const validity = this.state.formValidity;
        const isFellowName = name === "FellowName";
        const isFellowCode = name === "FellowCode";
        const isFellowAge = name === "FellowAge";


        validity[name] = value.length > 0;
        fieldValidationErrors[name] = validity[name]
            ? ""
            : `${name} is required and cannot be empty`;
        if (validity[name]) {
            if (isFellowName) {
                validity[name] = value.replace(/[^A-Za-z]/ig, '');
                fieldValidationErrors[name] = validity[name]
                    ? ""
                    : `${name} is not a valid character`;
            }

            if (isFellowCode) {
                validity[name] = value.replace(/[^0-9]/ig, '');
                fieldValidationErrors[name] = validity[name]
                    ? ""
                    : `${name} Must be a number type character `;
            }
            if (isFellowAge) {
                {
                    validity[name] = value.replace(/[^0-9]/ig, '');
                    fieldValidationErrors[name] = validity[name]
                        ? ""
                        : `${name} Must be a number type character`

                }

            };
        }
        this.setState({
            formErrors: fieldValidationErrors,
            formValidity: validity
        });
    };
    handleSubmit = event => {
        event.preventDefault();
        this.setState({ isSubmitting: true });
        const { formValues, formValidity } = this.state;
        if (Object.values(formValidity).every(Boolean)) {
            this.saveFellowDetails();
            this.setState({ isSubmitting: false });
        }
        else {
            for (let key in formValues) {
                let target = {
                    name: key,
                    value: formValues[key]
                };
                this.handleValidation(target);
            }
            this.setState({ isSubmitting: false });
        }
    };
    setFellowDetails(e) {
        this.setState({ filterList: false });
        switch (e.target.id) {
            case "FellowName":
                {
                    this.setState({ FellowName: e.target.value })
                    this.props.appointment.fellow.fullName = e.target.value;
                    if (e.target.value != "") {
                        this.fillFellowDetails(e);
                        this.setState({ filterList: true });
                    }
                    break;
                }
            case "FellowCode":
                {
                    this.setState({ FellowCode: e.target.value });
                    // this.props.appointment.fellow.id = e.target.value;
                    // }
                    break;

                }
            case "FellowAge":
                {
                    this.setState({ FellowAge: e.target.value });

                    break;
                }
            case "Weight":
                {
                    this.setState({ FellowWeight: parseInt(e.target.value) });
                    break;
                }
            case "FellowGenderM":
                {
                    this.setState({ FellowWeight: "M" });
                    break;
                }
            case "FellowGenderF":
                {
                    this.setState({ FellowWeight: "F" });
                    break;
                }
            case "IsHBPTrue":
                {
                    this.setState({ IsHBP: true });
                    break;
                }
            case "IsHBPFalse":
                {
                    this.setState({ IsHBP: false });
                    break;
                }
            case "DiabetesTrue":
                {
                    this.setState({ IsFellowDiabetic: true });
                    break;
                }
            case "DiabetesFalse":
                {

                    this.setState({ IsFellowDiabetic: false });
                    break;
                }
            case " XrayTrue":
                {
                    this.setState({ IsFellowDiabetic: true });
                    break;
                }
            case "XrayFalse":
                {

                    this.setState({ IsFellowDiabetic: false });
                    break;
                }

        }
    }
    onChangeInput(e) {
        this.handleChange(e)
        this.setFellowDetails(e)
    }

    saveFellowDetails() {
        this.props.setFellowDetails(this.state.formValues.FellowName, this.state.formValues.FellowCode, parseInt(this.state.formValues.FellowAge), parseInt(this.state.FellowWeight), this.state.FellowGender, this.state.IsHBP, this.state.IsFellowDiabetic, this.state.IsXrayDeviceRequired)
        this.props.setContentEditSurgery(this.state.nextPage)
    }
    btnBackFellowDetails() {
        this.props.setContentEditSurgery(this.state.BackPage)
    }
    
    fillFellowDetails(e) {
        let id = "", age = "", weight = "", gender = "";
        this.props.fellows.forEach(fellow => {
            var fellowResult = this.props.fellows.find(fel=>fel.fullName == e.target.value) 
            id = fellowResult.identityNumber;
            age = Math.floor((new Date() - new Date(fellowResult.birthDate).getTime()) / 3.15576e+10);
           
        })
        this.state.formValues.FellowName = e.target.value;
        this.state.formValues.FellowCode = id;
        this.state.formValues.FellowAge = age;
        this.state.FellowWeight= weight;
    }

      

    render() {
            var appointment = this.props.appointment;
            const { formValues, formErrors, isSubmitting } = this.state;
        return (
            <div className = "container" >
                    <div className="row row-centered">
                        <div className="col-lg-12">
                            <form onSubmit={this.handleSubmit}>
                                <div className="form-group">

                                    <input id="FellowName" type="search" className="inputInerModal"
                                        placeholder={appointment && appointment.fellow ? appointment.fellow.fullName : "Fellow Name"}
                                        list="fellows"
                                        onChange={this.onChangeInput}
                                        value={this.state.formValues.FellowName}
                                        name="FellowName"
                                        title="Fellow Name"
                                        className={`form-control ${
                                            formErrors.FellowName ? "is-invalid" : ""
                                            }`}
                                    />

                                    <div className="invalid-feedback">{formErrors.FellowName}
                                    </div>
                                    <datalist id="fellows" className="text-left">
                                        {
                                            this.props.fellow ?
                                                this.props.fellows.map((fellow, i) => {
                                                    return <option className="text-left" key={i}
                                                    >{fellow.fullName}</option>
                                                }) : ""
                                        }
                                    </datalist>
                                </div>

                                <div className="form-group">
                                    <input id="FellowCode" type="search" className="inputInerModal"
                                        placeholder={appointment && appointment.fellow ? appointment.fellow.id : "Code"}
                                        list="fellowId"
                                        name="FellowCode"
                                        onChange={this.onChangeInput}
                                        value={this.state.formValues.FellowCode}
                                        title="Fellow Code"
                                        className={`form-control ${
                                            formErrors.FellowCode ? "is-invalid" : ""
                                            }`}
                                    />

                                    <div className="invalid-feedback">{formErrors.FellowCode}
                                    </div>
                                    <datalist id="fellowId" className="text-left">
                                        {
                                            this.props.fellow ?
                                                this.props.fellows.map((item, i) => {
                                                    return <option className="text-left" key={i} >{item.identityNumber}</option>
                                                }) : ""
                                        }
                                    </datalist>
                                </div>


                                <div className="form-group">
                                    <input id="FellowAge" type="search" className="inputInerModal"
                                        placeholder={appointment && appointment.fellow ? "Age:" + " " + Math.floor((new Date() - new Date(appointment.fellow.birthDate).getTime()) / 3.15576e+10) + 1 : "Age"}
                                        onChange={this.onChangeInput}
                                        value={this.state.formValues.FellowAge}
                                        name="FellowAge"
                                        title="Fellow Age"
                                        className={`form-control ${
                                            formErrors.FellowAge ? "is-invalid" : ""
                                            }`}
                                    />
                                    <div className="invalid-feedback">{formErrors.FellowAge}</div>


                                </div>
                                <div>
                                    <input id="Weight" type="search" className="inputInerModal" placeholder="Weight"
                                        value={this.state.FellowWeight}
                                        onChange={this.setFellowDetails}
                                        className="form-control txtAL"
                                        title="Fellow Weight"
                                    />
                                </div>

                                <br></br>
                                <div className='row justify-content-center'>
                                    Gender:
               </div>
                                <div className='row justify-content-center'>
                                    <input className="col-1" name="gender" type="radio" id="FellowGenderM"
                                        value={this.state.FellowGender}
                                        checked={appointment.fellow.gender === "M"}
                                        onChange={this.setFellowDetails}
                                        title="Fellow Gender"
                                    />Male
                <input className="col-1" name="gender" type="radio" id="FellowGenderF" value="F"
                                        title="Fellow Gender"
                                        onChange={this.setFellowDetails}
                                        checked={appointment.fellow.gender === "F"}
                                    />Female
                 </div>
                                <br></br>


                                <div className='row justify-content-center'>
                                    High Blood Pressure:
            </div>

                                <div className='row justify-content-center'>
                                    <input id="IsHBPTrue" className="col-1" type="radio"
                                        value={this.state.IsHBP}
                                        name="radioModal"
                                        onChange={this.setFellowDetails} title="High Blood Pressure" />True
                <input id="IsHBBFalse" className="col-1" type="radio"
                                        value={this.state.IsHBP}
                                        name="radioModal"
                                        onChange={this.setFellowDetails} title="High Blood Pressure" />False
              </div>
                                <br></br>
                                <div className='row justify-content-center'>
                                    Diabetes:
            </div>

                                <div className='row justify-content-center'>
                                    <input id="DiabetesTrue" className="col-1" type="radio" value={true} name="radioDiabetes"
                                        onChange={this.setFellowDetails} title="Diabetes" />True
                <input id="DiabetesFalse" className="col-1" type="radio" value={false} name="radioDiabetes"
                                        onChange={this.setFellowDetails} title="Diabetes" />False
              </div>
                                <br></br>

                                <div className='row justify-content-center'>
                                    Xray:
            </div>

                                <div className='row justify-content-center'>
                                    <input id="XrayTrue" className="col-1"
                                        type="radio" value={true} name="radioXray"
                                        title="Xray"
                                    />True
                <input id="XrayFalse" className="col-1" type="radio" value={false} name="radioXray"
                                        title="Xray" />False
              </div>
                                <br></br>
                                <div>
                                    <div class='row modal-footer justify-content-end m-0 p-0 mt-2'>
                                        <div class='col-3 m-0 p-2 px-0'>
                                            <button className="btn btn-outline-primary font-pr m-2" name="butBack"
                                                onClick={this.btnBackFellowDetails} >Back</button>
                                        </div>
                                        <div class='col-6 m-0 p-2 px-0'></div >
                                        <div class='col-3 m-0 p-0 px-2'>

                                            <button type="sudmit"
                                                className="btn btn-outline-primary font-pr m-2"
                                                name="butNext"
                                                value="ServiceProvidersDetails"
                                                disabled={isSubmitting}
                                            >
                                                {isSubmitting ? "Please wait..." : "Next"}
                                            </button>

                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
            </div>
        )
    };
}

export default EditFellowDetails;




import React, { Component } from 'react'


export class ServiceProvidersDetails extends Component {
    constructor(props) {
        super(props)
        this.state = {
            nextPage: "ServiceProvidersDetails",
            backPage: "EditFellowDetails",
            SurgeonName: this.props.appointment.operation.surgeon.fullName,                   //Required
            SurgicalNursName:this.props.appointment.operation.nurse? this.props.appointment.operation.nurse.fullName:"",
            AnesthesiologistName: this.props.appointment.operation.anesthesiologist.fullName, //Required
            CleanTeamName: "",// this.props.appointment.operation.Cleaning.fullName
            SurgeonCode: this.props.appointment.operation.surgeon.id,
            SurgicalNursCode:this.props.appointment.operation.nurse? this.props.appointment.operation.nurse.id:null,
            AnesthesiologistCode: this.props.appointment.operation.anesthesiologist.id,
            CleanTeamCode: 0,
            SurgeonId:this.props.appointment.operation.surgeon.identityNumber,
            AnesthesiologistId:this.props.appointment.operation.anesthesiologist.identityNumber,
            SurgicalNursId: this.props.appointment.operation.nurse?this.props.appointment.operation.nurse.identityNumber:null,
            filterList: false
        }
        this.btnBackServiceDetails = this.btnBackServiceDetails.bind(this);
        this.setServiceProvidersDetails = this.setServiceProvidersDetails.bind(this);
        this.saveServiceProvidersDetails = this.saveServiceProvidersDetails.bind(this);
        this.findElementId = this.findElementId.bind(this);
    }

    findElementId(listElement, element) {
        const result = listElement.find((e) => element === e.fullName);
        return result;
    }

    
    saveServiceProvidersDetails() {
        debugger
        this.props.setServiceProvidersDetails(this.state.SurgeonCode, this.state.SurgicalNursCode, this.state.AnesthesiologistCode, this.state.CleanTeamCode, this.state.SurgeonId, this.state.SurgicalNursId, this.state.AnesthesiologistId)
    }
    btnBackServiceDetails() {
        this.props.setContentEditSurgery(this.state.backPage)

    }
    setServiceProvidersDetails(e) {
        switch (e.target.id) {
            case "SurgeonName":
                {
                    this.setState({ SurgeonName: e.target.value })
                    if (e.target.value != "") {
                        var surgeon = this.findElementId(this.props.serviceProviders, e.target.value);
                        this.setState({ filterList: true });
                        if (surgeon) {
                            this.props.appointment.operation.surgeon = surgeon;
                            this.setState({ SurgeonCode: surgeon.id, SurgeonId: surgeon.identityNumber });
                           
                        }
                        
                      
                    }
                    break;
                }

            case "SurgicalNursName":
                {
                    debugger
                    this.setState({ SurgicalNursName: e.target.value });
                    if (e.target.value != "") {
                        debugger
                        var nurs = this.findElementId(this.props.serviceProviders, e.target.value);
                        this.setState({ filterList: true });
                        if (nurs) {
                            this.props.appointment.operation.nurse = nurs;
                            this.setState({ SurgicalNursCode: nurs.id, SurgicalNursId: nurs.identityNumber });
                        }
                    }
                    break;

                }
            case "AnesthesiologistName":
                {
                    this.setState({ AnesthesiologistName: e.target.value });
                    if (e.target.value != "") {
                        var anesthesiologist = this.findElementId(this.props.serviceProviders, e.target.value);
                        this.setState({ filterList: true });
                        if (anesthesiologist) {
                            this.props.appointment.operation.anesthesiologist = anesthesiologist;
                            this.setState({ AnesthesiologistCode: anesthesiologist.id, AnesthesiologistId: anesthesiologist.identityNumber });
                            
                        }
                      
                       
                    }
                    break;
                }
            case "CleanTeamName":
                {
                    this.setState({ CleanTeamCode: e.target.value });
                    // if (e.target.value != "") {
                    //     var cleanTeam = this.findElementId(this.props.serviceProviders, e.target.value);
                    // }
                    break;
                }
        }
    }


    render() {
        var appointment = this.props.appointment;
        return (
            <div className="container">
                <div className="row row-centered">
                    <div className="col-lg-12">
                        <div className='row justify-content-center'>
                            <input id="SurgeonName" type="search" className="form-control txtAL"
                                placeholder={appointment && appointment.operation.surgeon ? appointment.operation.surgeon.fullName :
                                    "Surgeon"} list="surgeons"
                                value={this.state.SurgeonName}
                                title="Surgeon Name"
                                onChange={this.setServiceProvidersDetails}

                            />
                        </div >
                        <datalist id="surgeons">
                            {
                                this.props.surgeons ?
                                    this.props.surgeons.map((item, i) => {
                                        if (item.serviceProviderType === 1)
                                            return <option key={i}>{item.fullName}</option>
                                    })
                                    :
                                    ""
                            }
                        </datalist>
                        <br></br>
                        <div className='row justify-content-center'>
                            <input id="SurgicalNursName" type="search" className="form-control txtAL"
                                placeholder={appointment && appointment.operation.nurse ? appointment.operation.nurse.fullName :
                                    "Nurse"}
                                list="nurses"
                                value={this.state.SurgicalNursName}
                                onChange={this.setServiceProvidersDetails}
                                title="Nurse Name"
                            />
                        </div >
                        <datalist id="nurses">
                            {
                                this.props.nurses ?
                                    this.props.nurses.map((item, i) => {
                                        if (item.serviceProviderType === 3)
                                            return <option key={i}>{item.fullName}</option>
                                    })
                                    :
                                    ""
                            }
                        </datalist>
                        <br></br>

                        <div className='row justify-content-center'>
                            <input id="AnesthesiologistName" type="search" className="form-control txtAL"
                                placeholder={appointment && appointment.operation.anesthesiologist ? appointment.operation.anesthesiologist.fullName :
                                    "Anesthesiologist"}
                                list="anesthesiologists"
                                value={this.state.AnesthesiologistName}
                                onChange={this.setServiceProvidersDetails}
                                title="Anesthesiologist Name"
                            />
                        </div >
                        <datalist id="anesthesiologists">
                            {
                                this.props.anesthesiologists ?
                                    this.props.anesthesiologists.map((item, i) => {
                                        if (item.serviceProviderType === 2)
                                            return <option key={i}>{item.fullName}</option>
                                    })
                                    :
                                    ""
                            }
                        </datalist>
                        <br></br>

                        <div className='row justify-content-center'>
                            <input id="CleanTeamName" type="search" className="form-control txtAL"
                                placeholder={appointment && appointment.operation.Cleaning ? appointment.operation.Cleaning.fullName :
                                    "Cleaning"} list="cleaningTeams" value={this.state.CleanTeamName} onChange={this.setServiceProvidersDetails}
                                title="CleanTeam Name"
                            />
                        </div >
                        <datalist id="cleaningTeams">
                            {
                                this.props.cleaningTeams ?
                                    this.props.cleaningTeams.map((item, i) => {
                                        if (item.serviceProviderType === 4)
                                            return <option key={i}>{item.fullName}</option>
                                    })
                                    :
                                    "No results"
                            }
                        </datalist>
                        <div>
                            <div class="row modal-footer justify-content-end m-0 p-0 mt-2">
                                <div class='col-3 m-0 p-2 px-0'>
                                    <button className="btn btn-outline-primary font-pr m-2" name="butBack" value="EditFellowDetails" onClick={this.btnBackServiceDetails} >Back</button>
                                </div>
                                <div class='col-6 m-0 p-2 px-0'></div>
                                <div class='col-3 m-0 p-2 px-0'>
                                    <button className="btn btn-outline-primary font-pr m-2" name="butSave" value="ServiceProvidersDetails" onClick={this.saveServiceProvidersDetails} >Save</button>
                                </div>
                            </div>

                        </div>

                    </div>

                </div>

            </div>
        )
    }

}
export default ServiceProvidersDetails;
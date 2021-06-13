import React, { Component } from 'react'
import { getDir } from '../helpers/User';

export class Test extends Component {
    constructor(props) {
        super(props);
        this.state = {
            descriptionTest: "",
            isRequired: false,
            tests: this.props.tests,
            //testsToAdd: props.appointment.appointmentTests,
            isAddTest: false,
            disabledBtn: true,
            valueOfTest: "" //this.props.OBJECT.valueOfTest
        };
        //this.addTest = this.addTest.bind(this);
        //this.showAddTest = this.showAddTest.bind(this);
        //this.handelChange = this.handelChange.bind(this);
        this.renderMinimalKitTests = this.renderMinimalKitTests.bind(this);
        this.renderTests = this.renderTests.bind(this);
        this.testHandler = this.testHandler.bind(this);
        this.changeValueOfTest = this.changeValueOfTest.bind(this);
    }

    renderMinimalKitTests(tests) {
        //var appointment = this.props.appointment;
        // if (appointment.appointmentTests.length > 0) {
        //     appointment.appointmentTests.forEach(task => {
        //         console.log("Test: ", Test.requiredTask.description)
        //     })
        // }
        //let tests = appointment.appointmentTests;
        return <span className='' > {/* mkDisplay */}

            {
                tests.length ?
                    tests.map(test => this.renderTests(test))
                    :
                    ""
            }
        </span>
    }
    
    renderTests(item) {
        var t = this.props.t
        let dir = getDir()
        return (
            <li className="minimalkitStyle" key={item.id} id="main" dir={dir}>
                <div className="p-0">
                    <input className='checkboxMinstyle' type="checkbox" id={"check" + item.id} checked={item.approved} onChange={(e) => this.testHandler(e, item.id)} />
                    <span className="itemText">
                        {item.requiredTest.description}
                        {/* {
                            item.isRequired ?     cursor: pointer
                                <FontAwesomeIcon icon="exclamation" className={dir == "rtl" ? "mr-1" : "ml-1"} color={"#ed1c40"}></FontAwesomeIcon>
                                :
                                ""
                        } */}
                    </span>             
                                    {/* value={item.valueOfTest} */}
                    <input className="inputTest" type="search" id={"result" + item.id} title="Enter only integer or float number!" /* pattern="[0-9]" */ placeholder="Result" autoComplete="off" value={item.valueOfTest} onChange={(e) => this.changeValueOfTest(e, item.id)} />
                    {/* onClick={this.cancleValue} OR type="search" and add cheack if the key is number */}
                    <i id="edit" className={item.approved ? "fs-20 pe-7s-check basicColorAquq" : "fs-20 pe-7s-check basicColorRed"} data-toggle="tooltip" title={t("Users.done")} />
                </div>
            </li>
        );
    }

    testHandler(e, id) {
        console.log("test: ", e.target.checked, " id: ", id)
        let updatedTests = this.state.tests;
        //this.state.tests.find...
        let itemToUpdate = updatedTests.find(x => x.id === id);
        itemToUpdate.approved = e.target.checked;
        for(let i = 0; i < updatedTests.length; i++){
            if(updatedTests[i].id === itemToUpdate.id){
                updatedTests[i] = itemToUpdate
            }
        }
        // updatedTests.push(itemToUpdate)
        this.setState({ tests: updatedTests });
        this.props.saveTests(updatedTests)
    }

    changeValueOfTest(e, id) {
        //console.log("value: ", e.target.value)
        let updatedTests = this.state.tests;
        let itemToUpdate = updatedTests.find(x => x.id === id);
        

        /* VALIDATION */
        let regex = /^([1-9]?[0-9]*)([.]?[0-9]*)$/g;
        let isValidInput = regex.test(e.target.value);

        if(e.target.value == "") {
            console.log("valueOfTest: ", e.target.value)
            itemToUpdate.valueOfTest = e.target.value;
            for(let i = 0; i < updatedTests.length; i++){
                if(updatedTests[i].id === itemToUpdate.id){
                    updatedTests[i] = itemToUpdate;
                }
            }
            //this.setState({ valueOfTest: "" })

        }

        if(isValidInput){
            console.log("valueOfTest: ", e.target.value)
            itemToUpdate.valueOfTest = e.target.value;
            for(let i = 0; i < updatedTests.length; i++){
                if(updatedTests[i].id === itemToUpdate.id){
                    updatedTests[i] = itemToUpdate;
                }
            }
            //this.setState({ valueOfTest: e.target.value })
         }
        else{
            console.log("Value is: ", e.target.value)
            return;
        }
            
        
        // itemToUpdate.valueOfTest = e.target.value;
        // for(let i = 0; i < updatedTests.length; i++){
        //     if(updatedTests[i].id === itemToUpdate.id){
        //         updatedTests[i] = itemToUpdate
        //     }
        // }
        //updatedTests.push(itemToUpdate)

        this.setState({ tests: updatedTests  })

    }

    render() {
        let tests = this.state.tests;
        return <div className="container fs-14">
            <div className="row">
                <div className="col-12">
                    <form>

                        {this.renderMinimalKitTests(tests)}
                    </form>

                    {/* <div className="row">
                        <i id="add-test" className="fs-20 col-1 pe-7s-plus basicColorRed" title="Add test" style={{ cursor: "pointer" }} onClick={this.showAddTask} />
                    </div> 

                    {
                        this.state.isAddTest ?
                            <div >
                                <br />
                                <p className='minimalkitStyle p-0' >
                                    <input type="search" id="descriptionTest" placeholder="Description task" className="form-control col-7" value={this.state.descriptionTest} onChange={this.handelChange} /> <br />
                                    <input className='checkboxMinstyle' type="checkbox" onChange={(e) => this.setState({ isRequired: e.target.checked })} />
                                    <span className='itemText'>Is required task</span><br />
                                    <button className="btn btn-success font-pr col-2" disabled={this.state.disabledBtn} onClick={this.addTask}> save</button>
                                    <button className="btn btn-light font-pr col-2" disabled={this.state.disabledBtn} onClick={this.addTask}> save</button>
                                    
                                </p>
                            </div>
                            : 
                            ""
                    } */}
                </div>
            </div>

        </div>
    }
}

export default Test

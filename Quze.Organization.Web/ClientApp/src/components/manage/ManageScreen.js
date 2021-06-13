import React, { Component } from 'react'
import {ButtonToolbar, DropdownButton, MenuItem} from 'react-bootstrap'
import Select from 'react-select'
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import "react-tabs/style/react-tabs.css"; //using ??
import RequiredTasks from './RequiredTasks'
import RequiredDocuments from './RequiredDocuments'
import HeaderTitlePage from '../HeaderTitlePage';

import * as http from '../../helpers/Http' //call to the server
import { getFellowByUserId } from '../../helpers/AccountService'

const options = [
    { value: '1', label: 'קולונוסקופיה' },
    { value: '2', label: 'גסטרולוגיה' },
    { value: '3', label: 'אורתופדיה' },
    { value: '4', label: 'שיננית' }
]


/* 
visual-studio-online -> what is that
0. https://calendar.2net.co.il/?month=1&year=2019&placeId=51&methodId=0 - אחלה דוגמא לעריכה אינליין בתוך הטקסט
1. onblur js + onblur react
2. HTML contenteditable Attribute- 
https://www.w3schools.com/tags/att_global_contenteditable.asp
https://www.w3schools.com/tags/tryit.asp?filename=tryhtml5_global_contenteditable
 */

export class ManageScreen extends Component {


    constructor(props) {
        super(props);
        console.log(props)
        this.state = { 
            tabIndex: 1,
            selectedOption: null,
            isSeleceted: false,
            fellow: null,
            typeName: null,
            typeId: null,
            resultType: null,

            // TypeName: props.location.state.name, 
            // TypeId: props.location.state.id,
            // ResultType: props.location.state.type,

            //documents: null,
            //tasks: null,
            listTask: [
                {
                    indexT: 1,
                    id: 1,
                    name: "קולונוסקופיה",
                    tasks: [
                        {
                            id: 1,
                            description: "יש להפסיק לאכול פירות וירקות 120 שעות לפני הבדיקה",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 122,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 120,
                                    befor: "שעות לפני "
                                }
                            ]
                        },
                    
                        {
                            id: 2,
                            description: "יש לקחת חומר משלשל 36 שעות לפני הבדיקה - מנה ראשונה",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 38,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 37,
                                    befor: "שעות לפני "
                                }
                            ]
                        },
                        {
                            id: 3,
                            description: "יש לקחת חומר משלשל 36 שעות לפני הבדיקה - מנה שניה",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 37,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 36,
                                    befor: "שעות לפני "
                                }
                            ]
                        },
                        {
                            id: 4,
                            description: "יש להתחיל לצום - צום מלא!",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 0,
                                    befor: "עכשיו "
                                }
                            ]
                        }
                    ]
                    // task3: "יש להתחיל לצום - צום מלא, אין לשתות משקאות ממותקים כולל קפה ותה! אלא מים בלבד!",
                    // task3: "",
                },
                {
                    indexT: 2,
                    id: 2,
                    name: "גסטרולוגיה",
                    tasks: [
                        {
                            id: 1,
                            description: "יש צורך להיות בצום 8 שעות לפני הבדיקה",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 10,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 8,
                                    befor: "שעות לפני "
                                }
                            ]
                        },
                        {   
                            id: 2,
                            description: "אין לנהוג במשך 8 שעות לאחר הבדיקה.",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 10,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 8,
                                    befor: "שעות לפני "
                                }
                            ]
                        },
                        {
                            id: 3,
                            description: "חובה להגיע לבדיקה עם מלווה מבוגר.",
                            hoursBeforeToAlert: [
                                {
                                    message:"יש לשלוח התראה בנוסח ",
                                    hour: 24,
                                    befor: "שעות לפני "
                                }
                                // ,{
                                //     message: "יש לשלוח התראה בנוסח ",
                                //     hour: 24,
                                //     befor: " שעות לפני"
                                // }
                            ]
                        },
                        {
                            id: 4,
                            description: "יש להתחיל צום מלא!",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 0,
                                    befor: "עכשיו "
                                }
                            ]
                        }
                    ]
                },
                {
                    indexT: 3,
                    id: 3,
                    name: "אורתופדיה",
                    tasks: [
                        {
                            id: 1,
                            description: "ניתן לשתות מים או כל משקה אחר למעט חלב, עד 3 שעות לפני הבדיקה.",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 5,
                                    befor: "שעות לפני "
                                }
                            ]
                        },
                        {
                            id: 2,
                            description: "יש להפסיק לשתות",
                            hoursBeforeToAlert: [
                                {
                                    message:"יש לשלוח התראה בנוסח ",
                                    hour: 0,
                                    befor: "עכשיו "
                                }
                            ]
                        }
                    ]
                    
                },
                {
                    indexT: 4,
                    id: 4,
                    name: "שיננית",
                    tasks: [
                        {
                            id: 1,
                            description: "ניתן לאכול ולשתות עד שעתיים לפני הבדיקה",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 4,
                                    befor: "שעות לפני "
                                }
                            ]
                        },
                        {
                            id: 2,
                            description: "יש להפסיק לאכול ולשתות",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 0,
                                    befor: "עכשיו "
                                }
                            ]
                        }
                    ]
                }
            ],

            //Documents
            listDocuments: [
                {
                    indexT: 1,
                    id: 1,
                    name: "קולונוסקופיה",
                    documents: [
                        {
                            id: 1,
                            description: "אישור חברת הביטוח עבור תשלום רופא",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 122,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 120,
                                    befor: "שעות לפני "
                                }
                            ]
                        },
                        {
                            id: 2,
                            description: "אישור חברת הביטוח לשיפוי בית החולים",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 50,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה ",
                                    hour: 48,
                                    befor: "שעות לפני "
                                }
                            ]
                        }
                        // task3: ""
                    ]
                },
                {
                    indexT: 2,
                    id: 2,
                    name: "גסטרולוגיה",
                    documents: [
                        {
                            id: 1,
                            description: "אישור חברת הביטוח עבור תשלום רופא",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 122,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 120,
                                    befor: "שעות לפני "
                                }
                            ]
                            
                        },
                        {
                            id: 2,
                            description: "טופס 17 בתוקף של 3 חודשים לפחות",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 74,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 72,
                                    befor: "שעות לפני "
                                }
                            ]
                        },
                        {
                            id: 3,
                            description: "צילום רנטגן",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 74,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 72,
                                    befor: "שעות לפני "
                                }
                            ]
                        },
                        {
                            id: 4,
                            description: "יש להביא מכתב הפניה מפורט מהרופא המפנה",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 50,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 48,
                                    befor: "שעות לפני "
                                }
                            ]
                        },
                    ]
                },
                {
                    indexT: 3,
                    id: 3,
                    name: "אורתופדיה",
                    documents: [
                        {
                            id: 1,
                            description: "להביא מסמכים רפואיים קודמים, לשם השלמת המידע על ההיסטוריה הרפואית שלך",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 74,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 72,
                                    befor: "שעות לפני "
                                }
                            ]
                            
                        },
                        {
                            id: 2,
                            description: "להביא צילום רנטגן",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 74,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 72,
                                    befor: "שעות לפני "
                                }
                            ]
                            
                        },
                        {
                            id: 3,
                            description: "להביא צילום M.R.I",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 98,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 96,
                                    befor: "שעות לפני "
                                }
                            ]
                        }
                    ]
                },
                {
                    indexT: 4,
                    id: 4,
                    name: "שיננית",
                    documents: [
                        {
                            id: 1,
                            description: "יש להביא מכתב הפניה מפורט מהרופא המפנה",
                            hoursBeforeToAlert: [
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 3,
                                    befor: "שעות לפני "
                                },
                                {
                                    message: "יש לשלוח התראה בנוסח ",
                                    hour: 1,
                                    befor: "שעות לפני "
                                }
                            ]
                        }
                        // document2: "",
                    ]                    
                }
            ]
        };
    }

    //componentWillMount
    componentDidMount() {

        
       
        console.log("location.state - ",this.props.location.state)
        if(this.props.location.state) {
            let itemType = {
                typeName: this.props.location.state.name,
                typeId: this.props.location.state.id,
                resultType: this.props.location.state.type 
            }
             
            this.setState({
                typeName: itemType.typeName,
                typeId: itemType.typeId,
                resultType: itemType.resultType
            })
        } else {
            console.log("did not get ST from search component");
        }
        
        
        /* 
           if get "service type" -> go to the server and bring his details
           if not get "service type" -> go to the server and bring ALL the service types
        */

        //IF
        // http.get('api/ManageScreen/GET-ST'+ itemType)
        //     .then(function (response) {
        //         console.log(response);
        //         if (!response)
        //             return;
        //         thisObj.setState({
        //             , tasks: response.tasks
        //             , documents: response.documents
        //             
        //         })
        //     });

        //ELSE
        // http.get('api/ManageScreen/GET-ALL-ST'+ )
        //     .then(function (response) {
        //         console.log(response);
        //         if (!response)
        //             return;
        //         thisObj.setState({
        //             , tasks: response.tasks
        //             , documents: response.documents
        //             
        //         })
        //     });
    }

    handleClickTab(event, index) {
        event.stopPropagation();
        event.nativeEvent.stopImmediatePropagation();
        this.setState({ 
            tabIndex: index
        })
    }
    
    handleClickSelect = (selectedOption) => {
        //console.log(`Option selected:`, selectedOption); -> good give the 'label' (name) that selected + the 'value'
        this.setState({
            isSeleceted: true,
            selectedOption // like selectedOption: selectedOption
            // name: selectedOption.label
        })
    }

    render() {
        const selectedOption = this.state.selectedOption;
        return (
            /* יש בעיה עם העיצוב שתןצאןת החיפוש לא עולות על האלמנטים בדף הזה */ 
        <div className="">
                    
                    {/* <HeaderPage /> */}
                     <HeaderTitlePage title="סוגי שירות"/>
       
                    {/* <div>
                        {this.state.TypeName} 
                        <br/>
                        {this.state.TypeId} 
                        <br/>
                        {this.state.ResultType}
                    </div> */}
                    
                    <div className=" p-2 m-2">
                       
                            <Select id="managed" options={options} autoFocus='true' value={selectedOption} onChange={this.handleClickSelect}/>
                    </div>

                    <div className="row p-2 m-2">
                        {/* <div className="col-12">
                            <Tabs selectedIndex={this.state.tabIndex} onSelect={tabIndex => this.setState({ tabIndex })}>
                                <TabList>
                                    <Tab>משימות</Tab>
                                    <Tab>מסמכים</Tab>
                                </TabList>
                                <TabPanel>
                                    <h2>Any content 1</h2>
                                </TabPanel>
                                <TabPanel>
                                    <h2>Any content 2</h2>
                                </TabPanel>
                            </Tabs>
                        </div> */}

                        {
                           this.state.isSeleceted ? 
                              <div className="col-12" >
                                <ul className="nav nav-tabs p-2" role="tablist">
                                    <li className="nav-item" >
                                        <a className={"nav-link " + (this.state.tabIndex == 1 ? " active " : "")}
                                            onClick={(event) => { this.handleClickTab(event, 1) }} role="tab">
                                            {/* טבלת משימות */}
                                            {/* <FontAwesomeIcon icon="table" data-toggle="tooltip" title="משימות"/> */}
                                            משימות
                                        </a>
                                    </li>
                                    <li className="nav-item" >
                                        <a className={"nav-link " + (this.state.tabIndex == 2 ? " active " : "")}
                                            onClick={(event) => { this.handleClickTab(event, 2) }} role="tab">
                                            {/* קלפי משימות */}
                                            {/* <FontAwesomeIcon icon="th-large" data-toggle="tooltip"  title="מסמכים"/> */}
                                            מסמכים
                                        </a>
                                    </li>
                                </ul>
                                
                                <div className="tab-content h-75" id="myTabContent" >
                                    {
                                        this.state.tabIndex == 1 ? 
                                        <div className="tab-pane fade active show">
                                            <RequiredTasks listTasks={this.state.listTask} serviceType={selectedOption.value} serviceName={selectedOption.label}/>
                                        </div> : ""
                                    }
                                    {
                                        this.state.tabIndex == 2 ?
                                        <div className="tab-pane fade active show">
                                            <RequiredDocuments listDocuments={this.state.listDocuments} serviceType={selectedOption.value} serviceName={selectedOption.label}/>
                                        </div> : ""
                                    }
                                </div>
                            </div> : ""  
                        }

                    </div>

                    
            </div>
        )//return
    }//render
}

//export default ManageScreen

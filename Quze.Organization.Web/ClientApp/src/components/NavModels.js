import React, { Component } from 'react'
import '../css/NavModal.css'
import $ from 'jquery'

export class NavModels extends Component {
    constructor(props) {
        super(props)
        this.state = {
        }
    }
   
    render() {
        return (
            <div>
                <nav className="navbar navbar-expand-lg navbar-light p-0 m-0">
                    <div className="collapse navbar-collapse p-0 m-0" id="navbarNavAltMarkup">
                        <div className="navbar-nav">
                            {
                                this.props.navHeadlines.map(head => {
                                    if (head != null)
                                    return <button id="target" key={head} className="nav-item nav-link btnNav" value={head} style={{ fontWeight: 'bold' }} onClick={this.props.threeModesOfEditing}>
                                        {head}
                                    </button>    
                                })    
                            }
                        </div>
                    </div>
                </nav>
            </div>
        )
    }
}

export default NavModels

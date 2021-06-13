import React, { Component } from 'react'
import '../css/HeaderTitlepage.css'



 class HeaderTitlePage extends Component {
  render() {
    return (


<div className="title">
           <h5  className="headerTitlePage">{this.props.title}</h5>
    </div>



    )
  }
}

export default HeaderTitlePage

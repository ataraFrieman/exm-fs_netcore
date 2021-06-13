import React, { Component } from 'react';
import "../../css/site.css"
import "../../css/general.css"
import { withI18n } from 'react-i18next';



export class AncestorsLinkList extends Component {
    // constructor(props) {
    //     super(props);
    // }
    render() {
        let { list } = this.props;
        // let mapedList = <div/>;
        let counter = 0;
        let mappedList = (this.props.listLength && list) ?
            this.props.list.map((li, index) => {
                counter++
                return <span key={counter} >
                <u >
                    <font color="blue"
                            className="hand-cursor p-1"
                            onClick={() => { this.props.click(li) }}>{li.content.description}</font>
                    </u>
                    <span>>></span>
            </span>
            }
            )
            : ""

        return (<span>
            {mappedList}
        </span>);
  
    }
};

export default withI18n()( AncestorsLinkList);

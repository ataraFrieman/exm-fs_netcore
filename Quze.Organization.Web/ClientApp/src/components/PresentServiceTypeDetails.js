import React, { Component } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import * as http from '../helpers/Http';
import { withRouter } from 'react-router-dom';
import { setLogin } from '../helpers/User';
import ButtonServiceType from './ButtonServiceType';
import history from '../helpers/History';

//The format the details sould be presented
class TreeNode {
    constructor(content, childrenTreeNode) {
        this.content = content;
        this.childrenTreeNode = childrenTreeNode;
    }
}


export class SelectServiceType extends Component {
    constructor(props) {
        super(props);
        this.state = {
            tree: TreeNode,
            loading: true,
            parentId: null,
            parent: TreeNode,
        };
    }

    SetServiceTypeToPresent = (node) => {
        this.setState({
            parent: node,
            parentId: node.content.id,
        })
        history.push("/SelectServiceType", this.state.parent);
    }

    
    componentDidMount = () => {
        http.get('api/ServiceTypes/LoadAllServiceTypeDetaildByOrganizationIdAsTree')
            .then(res => {
                console.log("Response:", res);
                if (!res || res.length == 0) {
                    //console.log("there no User,Doctor or Service with this name")
                    console.log("Get failed...")
                    this.setState({
                    });
                } else {
                    this.setState({
                        tree: res,
                        loading: false,
                        parent: res.root,
                        parentId: res.root.content.id
                    });
                }
            })
    }



    render(){
        console.log("have: " + this.state.tree);
        console.log(this.state.parentId);

        const wellcomeMassege = this.state.loading == false ? < h1 > בחרו סוג טיפול ב{this.state.parent.content.description}
        </h1> : "";

        const serviceTypeMap = this.state.loading == false ? this.state.parent.childrenTreeNode.map(
            singleServiceType => <span key={singleServiceType.content.id}>
                <ButtonServiceType TreeNode={singleServiceType} SetServiceTypeToPresent={this.SetServiceTypeToPresent} />
            </span>
        ) : "";

        console.log(history);

        return <div>
            {wellcomeMassege}
            {serviceTypeMap}

        </div>
    }
};

export default SelectServiceType;

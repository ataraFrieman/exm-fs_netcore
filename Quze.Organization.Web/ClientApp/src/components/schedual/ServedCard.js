import React, { Component } from "react";

export class ServedCard extends Component {
    render() {
        let header = this.props.title || this.props.category ? (
            <div className={"header" + (this.props.hCenter ? " text-center" : "")}>
                <h4 className="title">{this.props.title}</h4>
                <p className="category">{this.props.category}</p>
                {this.props.subCategory ?
                    <p className="category">{this.props.subCategory}</p> : ""}
            </div>) : ' ';
        return (
            <div className=" card ">
                {header}
                <div
                    className={
                        "content" +
                        (this.props.ctAllIcons ? " all-icons" : "") +
                        (this.props.ctTableFullWidth ? " table-full-width" : "") +
                        (this.props.ctTableResponsive ? " table-responsive" : "") +
                        (this.props.ctTableUpgrade ? " table-upgrade" : "")
                    }
                >
                    {this.props.content}

                    <div className="footer">
                        {this.props.legend}
                        {this.props.stats != null ? <hr /> : ""}
                        <div className="stats">
                            <i className={this.props.statsIcon} /> {this.props.stats}
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default Card;


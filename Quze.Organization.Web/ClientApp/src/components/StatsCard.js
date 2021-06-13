import React, { Component } from "react";
import { Row, Col } from "react-bootstrap";
import '../css/statsCard.css';
export class StatsCard extends Component {
    render() {
        return (
            <div className={this.props.active ? "card card-stats active-card" : " card card-stats"}>
                <div className="content">
                    <Row className="align-items-center">
                        <div className="col-5">
                            <div className="icon-big text-center icon-warning">
                                {this.props.bigIcon} {this.props.statsValue}
                            </div>
                        </div>
                        <div className="col-7">
                            <div className="numbers">
                                <p> {this.props.statsText}</p>
                            </div>
                        </div>
                    </Row>

                </div>
            </div>
        );
    }
}

export default StatsCard;

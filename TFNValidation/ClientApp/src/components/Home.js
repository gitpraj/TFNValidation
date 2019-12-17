import React, { Component } from 'react';
import '../Styles/Home.css';

export class Home extends Component {
    displayName = Home.name

    constructor(props) {
        super(props);
        this.state = {
        }
        this.handleEnter = this.handleEnter.bind(this);
    }

    handleEnter(e) {
        e.preventDefault();
        this.props.history.push("/validate");
    }

    render() {
        return (
            <div className="bg-text">
                <div className="back">
                    <div className="button_base b07_3d_double_roll" onClick={this.handleEnter}>
                        <div>Click to Start</div>
                        <div>Click to Start</div>
                        <div>Click to Start</div>
                        <div>Click to Start</div>
                    </div>
                </div>
            </div>
        );
    }
}

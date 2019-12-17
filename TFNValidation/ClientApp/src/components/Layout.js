import React, { Component } from 'react';
import Background from '../Images/AusGovt.jpg';

export class Layout extends Component {
    displayName = Layout.name

    render() {
        return (
            <div id="out">
                <div className="background" style={{ backgroundImage: "url(" + Background + ")" }}>
                </div>
                {this.props.children}
            </div>
        );
    }
}

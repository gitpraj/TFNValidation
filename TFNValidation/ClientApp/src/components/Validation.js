import React, { Component } from 'react';
import '../Styles/Validation.css';
import { CONFIG } from '../Config/config.js';

export class Validation extends Component {
    displayName = Validation.name

    constructor(props) {
        super(props);
        this.state = {
            tfn: '',
            message: ''
        }
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleBackButton = this.handleBackButton.bind(this);
    }

    /**
     * This function hadles submit on the form and calls API to create new rider.
     * @param {any} e
     */
    handleSubmit(e) {
        e.preventDefault();
        this.setState({
            message: "Loading......."
        })

        var url = CONFIG.validationURL;
        fetch(url + this.state.tfn, {
            method: 'GET',
        }).then((response) => response.json())
            .then((responseJson) => {
                console.log("success: " + JSON.stringify(responseJson))
                if (responseJson == true) {
                    this.setState({
                        message: responseJson.message
                    })
                } else {
                    this.setState({ message: responseJson.message })
                }
            })
    }

    /**
     * This function handled the back button. Goes to home page
     * @param {any} e
     */
    handleBackButton(e) {
        this.props.history.push("/");
    }

    render() {

        let { message } = this.state;
        return (
            <div className="bg-text">
                <form onSubmit={this.handleSubmit}>
                    <div className="">

                        <div className="row">
                            <div className="col-lg-3 back-button">
                                <a className="next round" onClick={this.handleBackButton}>&#8249;</a>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col-lg-3">
                                <div className="form-group">
                                    <input value=""
                                        name="tfn"
                                        type="text"
                                        className="form-control input-w-60"
                                        id="tfn"
                                        placeholder="Tax File Number"
                                        onChange={e => this.setState({ tfn: e.target.value })}
                                        value={this.state.tfn}
                                        required />
                                    <span className="help-block" id=""></span>
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col-lg-3">
                                <button type="submit" className="btn btn-success" id="ValidateButton">Validate</button>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col-lg-3">
                                <span><h4 className="ret-message">{message}</h4></span>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        );
    }
}

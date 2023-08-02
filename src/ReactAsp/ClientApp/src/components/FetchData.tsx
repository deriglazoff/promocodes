import React, { Component } from 'react';
import { WeatherForecast } from '../models/WeatherForecast';

type MyState = {
    forecasts: WeatherForecast[]; // like this
    loading: boolean; // like this
};

export class FetchData extends Component {
    static displayName = FetchData.name;

    state: MyState;
    constructor(props: any) {
        super(props);
        this.state = { forecasts: [], loading: true };
        this.incrementCounter = this.incrementCounter.bind(this);
    }

    componentDidMount() {
        this.populateWeatherData();
    }


    incrementCounter() {
        this.populateWeatherData();
    }
    async populateWeatherData() {
        const response = await fetch('weatherforecast');
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderForecastsTable(this.state.forecasts);

        return (
            <div>

                <h1 id="tableLabel">Weather forecast</h1>

                <button className="btn btn-primary" onClick={this.incrementCounter}>Refresh</button>

                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }
    static renderForecastsTable(forecasts: WeatherForecast[]) {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.date}>
                            <td>{forecast.date}</td>
                            <td>{forecast.temperatureC}</td>
                            <td>{forecast.temperatureF}</td>
                            <td>{forecast.summary}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

}

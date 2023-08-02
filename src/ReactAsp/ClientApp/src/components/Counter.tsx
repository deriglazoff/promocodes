import React from "react";

type MyState = {
    count: number; // like this
};
export class Counter extends React.Component<MyState> {

    state: MyState = {
        // optional second annotation for better type inference
        count: 0,
    }
    constructor(props: any) {
        super(props);
        this.incrementCounter = this.incrementCounter.bind(this);
    }
    
    incrementCounter() {
        this.setState({
            count: this.state.count + 1
        });
    }

    render() {
        return (
            <div>
                <h1>Counter</h1>

                <p aria-live="polite">Current count: <strong>{this.state.count}</strong></p>

                <button className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>

            </div>
        );
    }
}
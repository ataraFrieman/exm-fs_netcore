import React, { Component } from 'react';
import { RadioGroup, Radio } from 'react-radio-group'



class FindByCategory extends Component {
    state = {}
    render() {
        return (
            <RadioGroup
                name="fruit"
                selectedValue={this.state.selectedValue}
                onChange={value => this.props.OnChecked(value)}>
                <div className='row'>
                    <label>
                        <Radio value="doctor" />חפש לפי שם רופא
            </label >
                </div>
                <div className='row'>
                    <label>
                        <Radio value="serviceKind" />חפש לפי תחום שרות
            </label>
                </div>
                <div className='row'>
                    <label>
                        <Radio value="free" />חיפוש חופשי
            </label>
                </div>

            </RadioGroup>
        );
    }
}

export default FindByCategory;
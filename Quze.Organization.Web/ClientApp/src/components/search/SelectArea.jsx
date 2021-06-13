import React, { Component } from 'react';
import Select from 'react-select';

const area = [
    { value: '1', label: 'ירושלים' },
    { value: '2', label: 'תל אביב' },
    { value: '3', label: 'צפון' },
    { value: '4', label: 'דרום' }
];


class SelectArea extends Component {

    render() {
        return (
            <div>
                <Select id='select2' options={area} autoFocus='true'
                    onChange={evt => this.props.onSelect([evt.value, evt.label])}
                    placeholder={'לדוגמא:ירושלים...'} arrowRenderer />
            </div>

        );
    }
}

export default SelectArea;
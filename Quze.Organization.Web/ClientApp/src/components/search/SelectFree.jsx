import React, { Component } from 'react';
import Select from 'react-select';
import { withI18n } from 'react-i18next';
const freeSearch = [
    // { value: '7', label:t("selectFree.dentistry") },
    // { value: '8', label:t("selectFree.Otolaryngology")},
    // { value: '9', label:t("selectFree.Pediatrics") },
    { value: '1', label: 'משה מושקוביץ' },
    { value: '2', label: 'ארווין בוזגלו' },
    { value: '3', label: 'מיכאל צור' },
    { value: '4', label: 'גל דקל' },
]


class SelectFree extends Component {

    render() {
        var t=this.props.t;
        return (
            <div>
                <Select id='select3' options={freeSearch} autoFocus='true' className="font-pr"
                    onChange={evt => this.props.onSelect(evt.label)}
                    placeholder={t("selectFree.search")} arrowRenderer />
            </div>

        );
    }
}

export default withI18n()(SelectFree) ;
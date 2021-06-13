import React, { Component } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { getDateYYYMMDD } from '../../helpers/TimeService';
import { observer } from 'mobx-react';
import './../../css/Filtered.css'

const btn1 = {
    margin: '3px'
};

var timeOfDay = hour => {
    if (hour >= 0 && hour <= 6)
        return 'לילה';
    if (hour >= 7 && hour <= 12)
        return 'בוקר';
    if (hour >= 13 && hour <= 16)
        return 'צהרים';
    if (hour >= 17 && hour <= 23)
        return 'לילה';
};

class Filtered extends Component {
    /*
      render() {
        const {filtersArray} = this.props; 
    
        return ( 
          <div id='filteredButton' dir='rtl'>
        
           {this.props.filtersArray.map(item =>  (
            item.value != ''? // print button
          <button id={item.id} type='button' className='btn btn-info' style={btn1}
           onClick={() =>this.props.onCancelFilter(item.id)}>  {item.key + ': ' + item.value + '   | x'}  </button>
            :''
           ))
           }
        
          </div>
       
         );
      }
    */

    constructor(props) {
        super(props);
        this.btnsToDisplay = this.btnsToDisplay.bind(this);
        this.cleanToDisplay = this.cleanToDisplay.bind(this);
        this.cleanToDisplay();
    }


    cleanToDisplay = () => this.props.filtersArray.filter(item =>
        item.value = '');

    btnsToDisplay = () => this.props.filtersArray.filter(item =>
        item.value !== '');

    displayItemValue = item => {
        if (item.id === 'dateRangeSelect' && item.value !== undefined && item.valueId !== undefined) {
            const startDate = getDateYYYMMDD(item.value);
            const endDate = getDateYYYMMDD(item.valueId);
            return startDate + ' - ' + endDate;
        }
        if (item.id === 'timeRangeSelect' && item.value !== undefined && item.valueId !== undefined) {
            var item1 = new Date(item.value);
            var item2 = new Date(item.valueId);
            let startTime = item1.getHours();
            startTime = startTime == 0 ? `12am` : startTime <= 12 ?( startTime + "am" ): (startTime + "pm");
            let endTime = item2.getHours();
            endTime = endTime == 0 ? `12am` : endTime <= 12 ? (endTime + "am") : (endTime + "pm");
            return  endTime + "-" + startTime ;
        }
        return item.value;
    }

    getKeyName(key) {
        switch (key) {
            case "בין השעות":
                return <FontAwesomeIcon icon="clock" />;
            case "בין התאריכים":
                return <FontAwesomeIcon icon="calendar" />;
            default:
                return "";
        }
    }


    render() {
        const { filtersArray } = this.props;

        return (
            <div id='filteredButton ' dir='rtl' className="m-0 row">

                {this.btnsToDisplay().map(item => (
                    <div key={item.id} onClick={() => this.props.onCancelFilter(item.id)} className="m-1 ">
                        <span className="fs-14 badge badge-secondary fc-gray bg-lightGray font-weight-normal" id={item.id} key={item.id}>
                            { this.displayItemValue(item)  }
                            <span className=" font-weight-bold text-danger pr-2">  x</span>
                        </span>
                        </div>
                ))
                }

            </div>

        );
    }







}


export default observer(Filtered);
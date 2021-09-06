import React, {useState} from 'react';
import {render} from 'react-dom';

import Header from '../Header/Header';
import Calendar from 'react-calendar';
import '../Calendar/MainCalendar.css';

const MainCalendar = ({onAdd, showAdd}) => {

    const [date,setDate] = useState(new Date());   
    const onChange = date => {
        setDate(date);
    };

    return (
        <div className='container'>
            
            <Calendar onChange={onChange} value={date}/>
            <Header
                title={date.toDateString()}
                onAdd={onAdd}
                showAdd={showAdd}
            />
            {console.log(date)}
        </div>
    )
}



export default MainCalendar

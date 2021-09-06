import React from 'react';

import {useState, useEffect} from 'react'
import {BrowserRouter as Router, Route} from 'react-router-dom'

import '../src/components/Calendar/MainCalendar.css';

import AddRing from './components/Button/AddRing'
import ConfigButton from './components/Button/ConfigButton'
import CalendarConfig from './components/Calendar/CalendarConfig';
import Header from '../src/components/Header/Header';
import Calendar from 'react-calendar';
import Rings from "../src/components/Rings/Rings"
import axios from 'axios';


//id, bellringtime, interval, intervalseconds, audiopath, type(int)
const App = () =>{
  const [showAddRing, setShowAddRing] = useState(false)

  //lekéri a mai dátumot
  const [date,setDate] = useState(new Date());   
    useEffect(() => {
      console.log(date);
    }, [date])

  //beállítja a dátumot, amire kattintottunk
  const onChange = date => {setDate(date)};

 

  return(
    <Router>
      <div style={{
        backgroundImage: "url(/logo.png)",
        backgroundRepeat: 'no-repeat',
        backgroundPositionX: '40px',
        backgroundSize: '150px'
        }}>  
        <Route path='/' exact render={(props)=> (
          <div>
            <div className='container' >      
              <Calendar  onChange={onChange} value={date}/>
              <Header
                  title={date.toDateString()}
                  onAdd={()=>setShowAddRing(!showAddRing)}
                  showAdd={showAddRing}
              />
            </div>
            {showAddRing && <AddRing date={date}/>}
            <Rings date={date}/>
            <ConfigButton/>
          </div>
        )}/>              
        <Route path='/calConfig' component={CalendarConfig}/>

      </div>
    </Router>   
  )
}

export default App;
import React from 'react'
import DateChooser from '../Calendar/DateChooser'
import RadioMaker from '../Radio/RadioMaker'
import SoundUploader from '../Radio/SoundUploader'
import TtsMaker from '../Radio/TtsMaker'


const CalendarConfig = () => {
    return (
        <div>
            <div>           
                <DateChooser/>
            </div>
            <div>
                <RadioMaker/>
            </div>
            <div>
                <TtsMaker/>
            </div>
            
            <div className='calcontainer'>
                <br />
                <a className='headerlink' href='/'> Naptár</a>
            </div>      
        </div>
    )
}

export default CalendarConfig

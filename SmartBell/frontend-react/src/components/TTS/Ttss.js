import React from 'react'
import Tts from './Tts'

const Ttss = ({ttss, onDelete}) => {
    
    return (
        <div className='container'>
            {ttss.map((tts)=>(
                <Tts
                    key={tts.id} 
                    tts={tts} 
                    onDelete={onDelete}
                />
            ))}
        </div>
    )
}

export default Ttss

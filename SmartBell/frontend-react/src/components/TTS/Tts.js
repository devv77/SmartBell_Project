import React from 'react'
import {FaTimes} from 'react-icons/fa'


const Tts = ({tts, onDelete}) => {
    return (
        <div className={`task ${tts.normal? 'reminder':''}`}>
            <h3>
                {tts.name}
                <FaTimes 
                style={{color: 'red', cursor: 'pointer'}}
                onClick={() => onDelete(tts.id)}
                />
            </h3>
            <p>{tts.text}</p>
        </div>
    )
}

export default Tts

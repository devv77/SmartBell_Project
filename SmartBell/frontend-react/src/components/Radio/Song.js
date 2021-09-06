import React from 'react'
import {FaTimes} from 'react-icons/fa'


const Song = ({song, onDelete}) => {
    const file = song.split(".");

    return (
        <div className={`task ${song.used? 'reminder':''}`}>
            <h3>
                {file[0]}
                <FaTimes style={{color: 'red', cursor: 'pointer'}} onClick={() => onDelete(song)}/>
            </h3>
            <p>.{file[1]}</p>
        </div>
    )
}

export default Song

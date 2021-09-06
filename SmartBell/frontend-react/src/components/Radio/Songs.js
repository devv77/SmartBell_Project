import React from 'react'
import Song from './Song'

const Songs = ({songs, onDelete}) => {

    return (
        <div className='container'>
            {songs.map((song)=>(
                <Song
                    key={song.id} 
                    song={song} 
                    onDelete={onDelete}
                />
            ))}
        </div>
    )
}

export default Songs

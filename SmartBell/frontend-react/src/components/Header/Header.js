import React from "react";

import Button from '../../components/Button/Button';

//rafce -->tabtab
const Header = ({title, onAdd, showAdd}) => {

    return (
        <header className='header'>
            <h1>Kiválasztva: {title}</h1>
            <Button 
                onClicked={onAdd} 
                color={showAdd? 'red' : 'green'} 
                text={showAdd ? 'Mégse' : 'Új csengetés hozzáadása'}
            />
        </header>
    )
}

export default Header
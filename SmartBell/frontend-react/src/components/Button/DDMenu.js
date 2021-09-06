import React from 'react'
import Dropdown from 'react-dropdown';
import 'react-dropdown/style.css';

const DDMenu = ({props, first}) => {
      const defaultOption = first;

    return (
        <div>
            <Dropdown options={props} onChange={this._onSelect} value={defaultOption} placeholder="Válassz egyet az opciók közül:" />
            <br/>
        </div>
    )
}

export default DDMenu

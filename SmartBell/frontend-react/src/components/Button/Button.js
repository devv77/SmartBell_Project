import React from 'react'

import PropTypes from 'prop-types'

const Button = ({color,text, onClicked}) => {

    return <button onClick={onClicked} style={{backgroundColor:color}} className='btn'>{text}</button>
}

Button.defaultProps={
    color:'grey'
}

Button.propTypes = {
    text: PropTypes.string,
    color: PropTypes.string,
    onClick: PropTypes.func,
}

export default Button

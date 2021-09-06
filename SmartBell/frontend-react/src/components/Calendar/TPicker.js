import React from 'react'
import moment from 'moment';
import TimePicker from 'rc-time-picker';

import 'rc-time-picker/assets/index.css';

const format = 'h:mm a';
const now = moment();

function onChange(value) {
  console.log(value && value/*.format(format)*/);
}

const TPicker = ({onChange}) => {
    return (
      <TimePicker
      showSecond={false}
      defaultValue={now}
      className="xxx"
      onChange={onChange}
      format={format}
      use12Hours
      inputReadOnly
      />
    )
}

export default TPicker

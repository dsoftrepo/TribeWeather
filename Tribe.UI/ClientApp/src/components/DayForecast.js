/* eslint-disable react/prop-types */
import React from 'react';

const DayForecast = ({ data }) => {
  return (
    <div className='my-3'>
      {JSON.stringify(data)}
    </div>
  );
}

export default DayForecast
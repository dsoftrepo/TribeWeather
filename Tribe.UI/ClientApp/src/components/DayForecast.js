/* eslint-disable react/prop-types */
import { Card, CardBody, CardFooter, CardHeader } from 'reactstrap';
import React from 'react';
import moment from 'moment';

const DayForecast = ({ day }) => {
  const pictures = [];
  const getPictures = () => {
    if(day.isSunny) pictures.push(<img alt="cat" src={require('../assets/cat-sun.png')} />);
    if(day.isRainy) pictures.push(<img alt="cat" src={require('../assets/cat-rain.png')} />);
    if(day.isSnowy) pictures.push(<img alt="cat" src={require('../assets/cat-snow.png')} />);
    if(day.isCloudy) pictures.push(<img alt="cat" src={require('../assets/cat-cloud.png')} />);
    return pictures;
  }
  if(!day) return null;
  return (
    <Card className='my-3 d-flex flex-column bd-highlight mb-3 border card-3d'>
      <CardHeader className='p-2 d-flex justify-content-between align-items-center"'>
        <span>{moment(day.date).format('LL')}</span>
        <div>
          <span>{day.temperatureMin} °C</span>
          {' - '}
          <span>{day.temperatureMax} °C</span>
        </div>
      </CardHeader>
      <CardBody className="p-1 d-flex justify-content-center">
        {getPictures()}
      </CardBody>
      <CardFooter className='p-2 small-text'>{day.summary}</CardFooter>
    </Card>
  );
}

export default DayForecast
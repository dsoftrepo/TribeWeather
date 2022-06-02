import React, { useState, useEffect } from 'react';
import { Alert, Row, Col } from 'reactstrap';
import moment from 'moment';
import AsyncSelect from 'react-select/async';
import Select from 'react-select'
import DayForecast from './DayForecast';

const Home = () => {
  const [searchPhrase, setsearchPhrase] = useState();
  const [locations, setLocations] = useState([
    { value: "53286", label: "Vancouver - Canada" },
    { value: "0", label: "Test - Canada" }
  ]);
  const [days, setDays] = useState('1');
  const [selectedLocation, setSelectedLocation] = useState();
  const [daysData, setDaysData] = useState([
    
    // had to add this test data my AcuWeather account has 50 requests limit a day and it run out quickly 
    
    { 
      date: moment().add(1, 'day').format("LL"),
      TemperatureMin: 11,
      TemperatureMax: 19,
      isCloudy: true,
      isSunny: false,
      isRainy: false,
      isSnowy: true,
      summary: "Not too sunny and will snow, later cloudy"
    },
    {
      date: moment().add(2, 'day').format("LL"),
      TemperatureMin: 10,
      TemperatureMax: 17,
      isCloudy: true,
      isSunny: false,
      isRainy: true,
      isSnowy: false,
      summary: "Not too sunny but rainy and cloudy"
    },
    {
      date: moment().add(3, 'days').format("LL"),
      TemperatureMin: 13,
      TemperatureMax: 21,
      isCloudy: true,
      isSunny: true,
      isRainy: true,
      isSnowy: false,
      summary: "Sunny! but later showers and cloudy"
    },
    {
      date: moment().add(4, 'days').format("LL"),
      TemperatureMin: 12,
      TemperatureMax: 22,
      isCloudy: false,
      isSunny: true,
      isRainy: true,
      isSnowy: false,
      summary: "Sunny and rainy as well"
    },
    {
      date: moment().add(5, 'days').format("LL"),
      TemperatureMin: 12,
      TemperatureMax: 18,
      isCloudy: true,
      isSunny: false,
      isRainy: false,
      isSnowy: false,
      summary: "Cloudy all day long"
    }
  ]);
  const [errorMessage, setErrorMessage] = useState();
  const [alertVisible, setAlertVisible] = useState(false);

  useEffect(() => {
    if (errorMessage) {
      setAlertVisible(true);
      setTimeout(() => {
        setAlertVisible(false);
        setErrorMessage(null);
      }, 3000);
    }
  }, [errorMessage])

  useEffect(() => {
    fetch(`api/weatherforecast/toplocations`)
      .then(response => response.json())
      .then((data) => {
        const options = Object.keys(data).map((key) => ({ value: key, label: data[key] }))
        if (options.length) setLocations(options);
        else setErrorMessage("No locations data received")
      })
      .catch(error => setErrorMessage(error));
  }, []);

  const loadMoreLocations = async () => {
    if (searchPhrase.length <= 3) return;
    const response = await fetch(`api/weatherforecast/locations/${searchPhrase}`);
    const data = await response.json();
    if (!response.ok) {
      const message = `An error has occured: ${response.status}`;
      setErrorMessage(message);
    }
    return Object.keys(data).map((key) => ({ value: key, label: data[key] }));
  }

  const loadWeatherForecastData = async (value) => {
    if(value === selectedLocation) return;
    const response = await fetch(`api/weatherforecast/${selectedLocation}/${days}`);
    const data = await response.json();
    setSelectedLocation(value);
    setDaysData(data);
    if (!data.length) setErrorMessage("No weather forecast data received");
  }

  return (
    <>
    { errorMessage && <Alert className='mx-5 mt-2 py-2 px-3 fixed-top' color="danger" isOpen={alertVisible}>{errorMessage}</Alert>}
    <Row>
      <Col md="2" />
      <Col md="6">
        <AsyncSelect
          loadOptions={loadMoreLocations}
          onChange={({ value }) => loadWeatherForecastData(value)}
          placeholder="Select or start typing"
          options={locations}
          defaultOptions={locations}
          onInputChange={(v) => setsearchPhrase(v)}
        />
      </Col>
      <Col md="2">
        <Select
          onChange={({ value }) => setDays(value)}
          defaultValue={{ value: '1', label: '1' }}
          placeholder="Days"
          options={[
          { value: '1', label: '1' },
          { value: '2', label: '2' },
          { value: '3', label: '3' },
          { value: '4', label: '4' },
          { value: '5', label: '5' }
          // no more for free on acuweather :(
          ]}
        />
      </Col>
      <Col md="2" />
    </Row>
    <Row>
      {
        daysData.map((d) => 
          <Col key={d.date}>
            <DayForecast day={d} />
          </Col>
        )
      }
    </Row>
    </>
  );
}

export default Home;

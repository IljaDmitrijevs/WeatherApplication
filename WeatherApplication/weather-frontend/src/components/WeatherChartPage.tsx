import React, { useState, useEffect } from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import { fetchWeatherData, fetchLatestWeatherDataTimestamp } from '../services/api';
import { WeatherData } from '../interfaces/WeatherDataInterface';
import styles from '../styles/WeatherChartPage.module.css';

const WeatherChartPage: React.FC = () => {
  const [data, setData] = useState<WeatherData[]>([]);
  const [latestDbTimestamp, setLatestDbTimestamp] = useState<Date | null>(null);
  const [fromDate, setFromDate] = useState<Date>(() => {
    const date = new Date();
    date.setHours(date.getHours() - 12);
    return date;
  });
  const [toDate, setToDate] = useState<Date>(new Date());
  const [loading, setLoading] = useState(false);

  // Fetch latest timestamp once when component mounts
  useEffect(() => {
    const fetchLatestTimestamp = async () => {
      try {
        const timestamp = await fetchLatestWeatherDataTimestamp();
        setLatestDbTimestamp(timestamp);
      } catch (error) {
        console.error('Error fetching latest timestamp:', error);
      }
    };
    
    fetchLatestTimestamp();
  }, []);

  // Fetch weather data when date range changes
  useEffect(() => {
    const loadData = async () => {
      try {
        setLoading(true);
        const result = await fetchWeatherData(fromDate, toDate);
        setData(result);
      } catch (error) {
        console.error('Error fetching weather data:', error);
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, [fromDate, toDate]);

  // Sets the chart data.
  const chartData = data.map(item => {
  const main = item.payload?.main || {};
  const sys = item.payload?.sys || {};
  return {
    ...item,
    temperature: +(Number(main.temp) - 273.15).toFixed(1),
    tempMin: +(Number(main.temp_min) - 273.15).toFixed(1),
    tempMax: +(Number(main.temp_max) - 273.15).toFixed(1),
    xAxisKey: `${item.city}, ${sys.country} | ${new Date(item.timestamp).toLocaleTimeString()}`,
    displayCity: item.city,
    displayCountry: sys.country,
    displayTime: new Date(item.timestamp).toLocaleTimeString([], {
      hour: '2-digit',
      minute: '2-digit',
      hour12: false
    })
  };
}).sort((a, b) => new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime());

  // Custom xAxis tick value for the chart.
  const CustomXAxisTick = ({ x, y, payload }: any) => {
  const item = chartData.find(d => d.xAxisKey === payload.value);
  return (
    <g transform={`translate(${x},${y})`}>
      <text x={0} y={0} dy={16} textAnchor="middle" fill="#666">
        <tspan x="0" dy="14" fontSize="11">
          {item?.displayCity}|{item?.displayCountry}
        </tspan>
        <tspan x="0" dy="16" fontSize="11">
          {item?.displayTime}
        </tspan>
      </text>
    </g>
  );
};

  // Time difference in milliseconds
  const timeDiffMs = toDate.getTime() - fromDate.getTime();

  // 15 minutes in milliseconds
  const fifteenMinutesMs = 15 * 60 * 1000;

  // Set xAxisInterval only if the time range is 15 minutes or more. 
  let xAxisInterval = 0;
  if (timeDiffMs >= fifteenMinutesMs) {
    xAxisInterval = Math.floor(chartData.length / 9);
  }

  // Formatted latest timestamp available form Db.
  const formattedLatestTimestamp = latestDbTimestamp 
    ? latestDbTimestamp.toLocaleString()
    : 'No data available';

  return (
    <div className={styles.container}>
      <h1 className={styles.header}>Weather Data</h1>
      
      <div className={styles.datePickerContainer}>
        <div className={styles.datePickerGroup}>
          <label className={styles.datePickerLabel}>From:</label>
          <DatePicker
            selected={fromDate}
            onChange={date => setFromDate(date || new Date())}
            showTimeSelect
            dateFormat="yyyy-MM-dd HH:mm"
            timeFormat="HH:mm"
            className={styles.datePickerInput}
          />
        </div>
        <div className={styles.datePickerGroup}>
          <label className={styles.datePickerLabel}>To:</label>
          <DatePicker
            selected={toDate}
            onChange={date => setToDate(date || new Date())}
            showTimeSelect
            dateFormat="yyyy-MM-dd HH:mm"
            timeFormat="HH:mm"
            className={styles.datePickerInput}
          />
        </div>
      </div>

      {loading ? (
        <div className={styles.loadingContainer}>
          <div className={styles.loadingSpinner}></div>
        </div>
      ) : (
        <div className={styles.chartContainer}>
          <div className={styles.timestampInfo}>
            <span className={styles.timestampLabel}>Latest available data:</span> {formattedLatestTimestamp}
          </div>
          <div className={styles.chartWrapper}>
            <ResponsiveContainer width="100%" height="100%">
              <LineChart data={chartData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis 
                  dataKey="xAxisKey"
                  interval={xAxisInterval}
                  height={50}
                  tick={<CustomXAxisTick />} 
                />
                <YAxis label={{ value: '°C', angle: -90, position: 'insideLeft' }} />
                <Tooltip />
                <Legend />
                <Line type="monotone" dataKey="tempMin" stroke="#8884d8" name="Min Temp" />
                <Line type="monotone" dataKey="tempMax" stroke="#82ca9d" name="Max Temp" />
                <Line type="monotone" dataKey="temperature" stroke="#ff7300" name="Temp" />
              </LineChart>
            </ResponsiveContainer>
          </div>
        </div>
      )}
    </div>
  );
};

export default WeatherChartPage;
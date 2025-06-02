import axios from 'axios';
import { WeatherData } from '../interfaces/WeatherDataInterface';
import { FetchLog } from '../interfaces/FetchLogInterface';

// Api url.
export const api = axios.create({
  baseURL: process.env.REACT_APP_API_URL,
});

// Api call for weather data fetching.
export const fetchWeatherData = async (dateTimeFrom?: Date, dateTimeTo?: Date): Promise<WeatherData[]> => {
  const params = new URLSearchParams();
  
  if (dateTimeFrom) {
    params.append('dateTimeFrom', dateTimeFrom.toISOString());
  }
  if (dateTimeTo) { 
    params.append('dateTimeTo', dateTimeTo.toISOString());
  }

  const response = await api.get<WeatherData[]>('/api/weather', { params });

  return response.data;
};

// Api call for logs fetchigs.
export const fetchLogs = async (): Promise<FetchLog[]> => {
  const response = await api.get<FetchLog[]>('/api/logs');
  
  return response.data;
};

// Api call for fethcing latest available weather data timestamp.
export async function fetchLatestWeatherDataTimestamp(): Promise<Date> {
  const response = await api.get('/api/weather/latest-timestamp');

  return new Date(response.data.timestamp);
}
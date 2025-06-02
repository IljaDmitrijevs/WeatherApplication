export interface WeatherMain {
  temp: number;
  temp_min: number;
  temp_max: number;
}

export interface WeatherSys {
  country: string;
}

export interface WeatherPayload {
  main: WeatherMain;
  sys: WeatherSys;
  [key: string]: any;
}

export interface WeatherData {
  id: number;
  city: string;
  timestamp: string;
  payload: WeatherPayload;
}
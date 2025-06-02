import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import WeatherChartPage from '../../components/WeatherChartPage';
import { fetchWeatherData, fetchLatestWeatherDataTimestamp } from '../../services/api';

jest.mock('../../services/api');

const mockWeatherData = [
  {
    city: 'Riga',
    timestamp: new Date().toISOString(),
    payload: {
      main: { temp: 290.15, temp_min: 288.15, temp_max: 293.15 },
      sys: { country: 'LV' },
    }
  }
];

describe('WeatherChartPage', () => {
  beforeEach(() => {
    (fetchWeatherData as jest.Mock).mockResolvedValue(mockWeatherData);
    (fetchLatestWeatherDataTimestamp as jest.Mock).mockResolvedValue(new Date());
  });

  it('renders chart and timestamp after loading', async () => {
    render(<WeatherChartPage />);
    expect(screen.getByText(/weather data/i)).toBeInTheDocument();

    await waitFor(() => {
      expect(fetchWeatherData).toHaveBeenCalled();
      expect(fetchLatestWeatherDataTimestamp).toHaveBeenCalled();
      expect(screen.getByText(/latest available data:/i)).toBeInTheDocument();
    });
  });
});

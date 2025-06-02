import { fetchWeatherData, api } from '../../services/api';
import { WeatherData } from '../../interfaces/WeatherDataInterface';

describe('API service with spyOn', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('fetches weather data', async () => {
    const mockWeatherData: WeatherData[] = [{
      id: 1,
      city: 'Riga',
      timestamp: '2025-05-29T14:30:00Z',
      payload: {
        main: { temp: 294.15, temp_min: 293.15, temp_max: 295.15 },
        sys: { country: 'LV' },
      },
    }];

    jest.spyOn(api, 'get').mockResolvedValue({ 
      data: mockWeatherData,
      status: 200,
      statusText: 'OK',
      headers: {},
      config: {}
    });

    const data = await fetchWeatherData();
    expect(api.get).toHaveBeenCalled();
    expect(data).toEqual(mockWeatherData);
  });
});
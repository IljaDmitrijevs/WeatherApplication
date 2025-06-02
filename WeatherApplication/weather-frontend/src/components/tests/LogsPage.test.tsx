import React from 'react';
import { render, screen, waitFor, act } from '@testing-library/react'; // Add act import
import LogsPage from '../../components/LogsPage';
import { fetchLogs } from '../../services/api';

jest.mock('../../services/api');

const mockedLogs = [
  {
    id: '1',
    city: 'London',
    success: true,
    message: 'Fetched successfully',
    timestamp: new Date().toISOString(),
  }
];

describe('LogsPage', () => {
  beforeEach(() => {
    (fetchLogs as jest.Mock).mockResolvedValue(mockedLogs);
  });

  it('renders "No logs available" when API returns empty array', async () => {
    (fetchLogs as jest.Mock).mockResolvedValueOnce([]);
    
    await act(async () => { // Wrap render in act
      render(<LogsPage />);
    });

    await act(async () => { // Wait for updates
      await waitFor(() => expect(screen.getByText(/no logs available/i)).toBeInTheDocument());
    });
  });
});
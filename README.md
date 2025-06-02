# Weather appliction

Azure Functions + ASP.NET Core + React app that:

- Fetches weather every minute for London, Rome, Riga
- Stores full payload + logs to SQL DB
- Displays min/max temp chart with city and  shortened country
- Shows fetch logs in a table

## Tech Stack

- Azure Functions (Timer trigger)
- ASP.NET Core Web API + EF Core + SQL Server
- React (TypeScript, Recharts)
- Unit tests with xUnit + Jest

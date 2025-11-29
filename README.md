# DublinBikes API

## Overview
This is a .NET 8 Web API for DublinBikes stations.  
It loads station data from a JSON file and provides endpoints to get stations, get station by number, get summary, and create/update stations.

---

## How to Run
1. Open the solution in Visual Studio.
2. Set `DublinBikesApi` as the startup project.
3. Press **F5** to run the API.
4. The API will be available at:
   - `https://localhost:7010`
   - `http://localhost:5033`

---

## API Endpoints

### GET /api/v2/stations
Returns a list of stations.

**Query Parameters (optional):**
- `status=OPEN|CLOSED` → Filter by station status
- `minBikes=int` → Minimum available bikes
- `q=searchTerm` → Search in station name and address
- `sort=name|availableBikes|occupancy` → Sort field
- `dir=asc|desc` → Sort direction
- `page=int` → Page number (default 1)
- `pageSize=int` → Number of items per page (default 10)

**Example:**

GET /api/v2/stations?status=OPEN&minBikes=5&q=Dublin&sort=name&dir=asc&page=1&pageSize=5


---

### GET /api/v2/stations/{number}
Returns a single station by its number.  
Returns **404** if not found.

**Example:**
GET /api/v2/stations/42


---

### GET /api/v2/stations/summary
Returns aggregated information:
- totalStations
- totalBikeStands
- totalAvailableBikes
- counts by status (OPEN/CLOSED)

**Example:**

GET /api/v2/stations/summary


---

### POST /api/v2/stations
Create a new station.  
Send a JSON body with station details.

**Example body:**
```json
{
  "number": 999,
  "name": "New Station",
  "address": "123 Dublin St",
  "position": { "lat": 53.345, "lng": -6.260 },
  "bike_Stands": 20,
  "available_Bikes": 10,
  "available_Bike_Stands": 10,
  "status": "OPEN",
  "last_Update": 1698745600000
}

PUT /api/v2/stations

Update an existing station.
Send the same JSON structure as POST.
The station is matched by number.

Tests

Run the tests in Test Explorer in Visual Studio.

The tests cover basic filtering, search, and summary functionality.

All implemented tests should pass.

Notes

Data is loaded from Data/dublinbike.json.

Occupancy is calculated as available_bikes / bike_stands.

Last update is stored in epoch milliseconds (Last_Update).

Only endpoints implemented: GET, POST, PUT, summary.

Filters, search, sort, and pagination are supported in GET /stations.
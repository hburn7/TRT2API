## The Roundtable II API
This API is used by The Roundtable II website & gosumemory stream overlay. This API functions as a bridge between these services and a central PostgreSQL database.

## Usage
Setup is straightforward.

* Clone the repository: ```git clone https://github.com/hburn7/TRT2API.git && cd TRT2API```
* Run the project via the dotnet CLI (note, CD to the folder containing the `.csproj` file): `cd TRT2API && dotnet run`
* Configure a PostgreSQL database and update the connection string in `appsettings.json` (replace values in example appsettings.json)
* Run the provided database scripts to create the necessary tables
* Query an endpoint on the running port: `GET https://localhost:44395/api/players/all` for example

# Database
Run the db.sql script into your PostgreSQL instance. This will create the necessary tables for the API to function.

# API Endpoints

All responses are provided in JSON format.

## Players Endpoints

* **Get All Players:** `GET /api/players/all`
    - Returns all players in the database. If no players exist, a 404 error is returned.

* **Get Player by ID:** `GET /api/players/{playerID:long}`
    - Returns a specific player by their ID. If no player exists with the provided ID, a 404 error is returned.

* **Get Player Matches:** `GET /api/players/{playerID:long}/matches`
    - Returns all matches a specific player has participated in. If no matches exist for the player, a 404 error is returned.

* **Add Player:** `POST /api/players`
    - Adds a new player to the database. If the player already exists, a 409 error is returned.

* **Update Player:** `PUT /api/players/{playerID:long}`
    - Updates an existing player's data in the database. If the player does not exist, a 404 error is returned.

## Matches Endpoints

* **Get All Matches:** `GET /api/matches/all`
    - Returns all matches in the database. If no matches exist, a 404 error is returned.

* **Get Match by ID:** `GET /api/matches/{matchID:int}`
    - Returns a specific match by its ID. If no match exists with the provided ID, a 404 error is returned.

* **Get Players in Match:** `GET /api/matches/{matchID:int}/players`
    - Returns all players that participated in a specific match. If the match does not exist, a 404 error is returned.

* **Update Match:** `PUT /api/matches/{matchID:int}`
    - Updates an existing match's data in the database. If the match does not exist, a 404 error is returned.

## Schedules Endpoints

* **Get All Schedules:** `GET /api/schedules/all`
    - Returns all schedules in the database. If no schedules exist, a 404 error is returned.

* **Get Schedule by ID:** `GET /api/schedules/{id:int}`
    - Returns a specific schedule by its ID. If no schedule exists with the provided ID, a 404 error is returned.

* **Update Schedule:** `PUT /api/schedules/{id:int}`
    - Updates an existing schedule's data in the database. If the schedule does not exist, a 404 error is returned.

## Maps Endpoints

* **Get All Maps:** `GET /api/maps/all`
    - Returns all maps in the database. If no maps exist, a 404 error is returned.

* **Get Map by MapID:** `GET /api/maps/{mapID:long}`
    - Returns a specific map by its MapID. If no map exists with the provided MapID, a 404 error is returned.

* **Update Map:** `PUT /api/maps/{mapID:long}`
    - Updates an existing map's data in the database. If the map does not exist, a 404 error is returned.

Note: successful operations return HTTP status code 200 (OK) or 204 (No Content) unless otherwise noted.


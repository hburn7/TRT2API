## The Roundtable II API

This API is used by The Roundtable II website & gosumemory stream overlay. This API functions as a bridge between these services and a central PostgreSQL database.

## Usage

Setup is straightforward.

- Clone the repository: `git clone https://github.com/hburn7/TRT2API.git && cd TRT2API`
- Run the project via the dotnet CLI (note, CD to the folder containing the `.csproj` file): `cd TRT2API && dotnet run`
- Configure a PostgreSQL database and update the connection string in `appsettings.json` (replace values in example appsettings.json)
- Run the provided database scripts to create the necessary tables
- Query an endpoint on the running port: `GET https://localhost:44395/api/players/all` for example

# Database

Run the db.sql script into your PostgreSQL instance. This will create the necessary tables for the API to function.

# API Endpoints

All responses are provided in JSON format.

### **Important Note:** All POST requests with an "id" field are ignored, the database tracks ids automatically.

## Players Endpoints

- **Get All Players:** `GET /api/players/all`

  - Returns all players in the database. If no players exist, a 404 error is returned.

- **Get Player by osu! ID:** `GET /api/players/{osuPlayerId:long}`

  - Returns a specific player by their osu! Id. If no player exists with the provided Id, a 404 error is returned.

- **Get Player Matches by osu! ID:** `GET /api/players/{osuPlayerId:long}/matches`

  - Returns all matches a specific player has participated in by their osu! Id. If no matches exist for the player, a 404 error is returned.

- **Get Player by ID:** `GET /api/players/{playerId:int}`

  - Returns a specific player by their Id. If no player exists with the provided Id, a 404 error is returned.

- **Update Player:** `PUT /api/players/{osuPlayerId:long}`

  - Updates an existing player's data in the database. If the player does not exist, a 404 error is returned. The osuPlayerId in the endpoint must match the value in the post body.

- **Add Player:** `POST /api/players/add`

  - Adds a new player to the database. If the request is malformed, a 400 error is returned. If the request is unable to be processed, a 409 error is returned.

  Post body:

  ```json
  {
    "id": 0,
    "osuPlayerId": 0,
    "name": "string",
    "totalMatches": 0,
    "totalWins": 0,
    "status": "string",
    "isEliminated": true,
    "seeding": 0
  }
  ```

## Matches Endpoints

- **Get All Matches:** `GET /api/matches/all`

  - Returns all matches in the database. If no matches exist, a 404 error is returned.

- **Get Match by ID:** `GET /api/matches/{id:int}`

  - Returns a specific match by its Id. If no match exists with the provided Id, a 404 error is returned. Returns status code 500 if another error occurs. Note: This is not the osu! match Id.

  Response body:

  ```json
  {
    "match": {
      "id": 2,
      "osuMatchId": null,
      "type": "battle_royale",
      "scheduleId": 0,
      "winnerId": 3,
      "timeStart": "2023-07-08T22:05:00",
      "lastUpdated": "2023-07-08T22:05:00",
      "bracketMatchId": null,
      "roundId": null
    },
    "matchPlayers": [
      {
        "id": 3,
        "matchId": 2,
        "playerId": 2,
        "score": 835433
      },
      {
        "id": 4,
        "matchId": 2,
        "playerId": 3,
        "score": 732681
      }
    ],
    "matchMaps": [
      {
        "id": 8,
        "matchId": 2,
        "playerId": 3,
        "mapId": 5,
        "action": "picked",
        "orderInMatch": 69
      }
    ]
  }
  ```

- **Get by osu! Match ID:** `GET /api/matches/osumatch/{osuMatchId:long}`

  - Returns a specific match by its osu! match Id. If no match exists with the provided osu! match Id, a 404 error is returned. Returns status code 500 if another error occurs.

- **Get Players in Match:** `GET /api/matches/{matchID:long}/players`

  - Returns all players that participated in a specific match. If there are no players for the match or the match could not be found, a 404 error is returned.

<!-- - **Update Match:** `PUT /api/matches/{matchID:long}`

  - Updates an existing match's data in the database. If the match does not exist, a 404 error is returned. -->

- **Add Match:** `POST /api/matches/add`

  - Adds a new match to the database. If the request cannot be parsed correctly, a 400 error is returned. If any other issue occurs, status code 500 is returned.

  Post body: (TODO: Fix posting format)

  ```json
  {
    "match": {
      "id": 0,
      "osuMatchId": 0,
      "type": "string",
      "scheduleId": 0,
      "winnerId": 0,
      "timeStart": "2023-06-27T13:21:29.196Z",
      "lastUpdated": "2023-06-27T13:21:29.196Z",
      "bracketMatchId": 0
    }
  }
  ```

## Schedules Endpoints

- **Get All Schedules:** `GET /api/schedules/all`

  - Returns all schedules in the database. If no schedules exist, a 404 error is returned.

- **Get Schedule by ID:** `GET /api/schedules/{id:int}`

  - Returns a specific schedule by its ID. If no schedule exists with the provided ID, a 404 error is returned.

- **Update Schedule:** `PUT /api/schedules/{id:int}`

  - Updates an existing schedule's data in the database. If the schedule does not exist, a 404 error is returned.

- **Delete Schedule:** `DELETE /api/schedules/{id:int}`

  - Deletes an existing schedule from the database. If there is any error, a 500 error is returned.

- **Add Schedule:** `POST /api/schedules/add`
  - Adds a new schedule to the database. If there is any error, a 409 error is returned.

## Maps Endpoints

- **Get All Maps:** `GET /api/maps/all`

  - Returns all maps in the database. If no maps exist, a 404 error is returned.

- **Get Map by osu! Map Id:** `GET /api/maps/{osuMapId:long}`

  - Returns a specific map by its osuMapId. If no map exists with the provided osuMapId, a 404 error is returned.

- **Update Map:** `PUT /api/maps/{mapID:long}`

  - Updates an existing map's data in the database with what is provided in the `PUT` body. If the map does not exist, a 404 error is returned.

- **Delete Map:** `DELETE /api/maps/{osuMapId:long}`

  - Deletes an existing map from the database. If the map does not exist, a 404 error is returned. If another error occurs, a 500 error is returned.

- **Add Map:** `POST /api/maps/add`

  - Adds a new map to the database. If the map already exists, a 409 error is returned.

  Post body:

  ```json
  {
    "id": 0,
    "osuMapId": 0,
    "round": "string",
    "mod": "string",
    "postModSr": 0,
    "metadata": "string"
  }
  ```

## Rounds Endpoints

- **Get All Rounds:** `GET /api/rounds/all`

  - Returns all rounds in the database. If no rounds exist, a 404 error is returned. If there is an error, a 500 error is returned.

- **Get Round by Name:** `GET /api/rounds/{name:string}`
  - Returns a specific round by its name. If no round exists with the provided name, a 404 error is returned. If there is an error, a 500 error is returned. A name might be `RO16` or `GF` (grand finals)

## Matchlog

- **Get All Logs:** `GET /api/matchlog/all`

  - Returns all logs in the database.

- **Get Logs by Match ID:** `GET /api/matchlog/{matchID:int}`

  - Returns all logs for a specific match. Note: This is not the osu! match ID, but the match ID from the database.

- **Add Log:** `POST /api/matchlog/add`
  - Adds a new matchlog entry. Returns error 400 if the request is malformed.
    Post body:
  ```json
  {
    "id": 0,
    "matchId": 0,
    "playerId": 0,
    "mapId": 0,
    "action": "string",
    "orderInMatch": 0
  }
  ```

**Misc Notes:**

- When getting a match, the `matchPlayers` field's `score` property will be:
  - For Battle Royale: The numeric score the player achieved for a specific map.
  - For 1v1 & Main Tournament: The number of maps won by the player in the osu! match.
- Each map played in a Battle Royale match is counted as a `match`.

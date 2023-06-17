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
Import these scripts into your PostgreSQL database to create the necessary tables.

```sql
create table if not exists matches
(
    id         serial
        primary key,
    matchid    bigint
        unique,
    winnerid   integer
        references players (playerid),
    player_ids bigint[]
);

create table if not exists players
(
    id          serial
        primary key,
    playerid    bigint       not null
        unique,
    playername  varchar(255) not null,
    totalwins   integer      not null,
    totallosses integer      not null,
    status      varchar(255) not null
);
```

# Routes & Endpoints
All data will be returned as a JSON object. All success response codes are 200.

## GET
**Players:** `/api/players`
* `/all` - Returns all players in the database.

```json
[
  {
    "id": 1,
    "playerID": 12345,
    "playerName": "Stage",
    "totalWins": 4,
    "totalLosses": 0,
    "status": "this is a test status"
  },
  {
    "id": 2,
    "playerID": 123456,
    "playerName": "ROB_",
    "totalWins": 0,
    "totalLosses": 1,
    "status": "get owned"
  }
]
```

* `/{playerID}` - Returns a single player for the given ID. Returns status code 400 if the specified ID does not belong to any player.

```json
{
  "id": 1,
  "playerID": 12345,
  "playerName": "Stage",
  "totalWins": 4,
  "totalLosses": 0,
  "status": "this is a test status"
}
```

* `/{playerID}/matches` - This endpoint returns all matches involving the player with the given player ID. Matches are returned where the player was either Player 1 or Player 2. Returns an empty array if the player has not participated in any matches. Returns status code 400 if the specified ID does not belong to any player.

For example, the following might be a typical response for `/api/players/12345/matches`:
```json
[
  {
    "id": 1,
    "matchID": 1,
    "playerIDs": [
      12345,
      456
    ],
    "winnerID": null
  }
]
```

This shows that player 12345 has been involved in two matches, one where they were Player 1 and won, and another where they were Player 2 and lost.
## The Roundtable II API
This API is used by The Roundtable II website & gosumemory stream overlay. This API functions as a bridge between these services and a central PostgreSQL database.

## Usage
Setup is straightforward.

* Clone the repository: ```git clone https://github.com/hburn7/TRT2API.git && cd TRT2API```
* Run the project via the dotnet CLI (note, CD to the folder containing the `.csproj` file): `cd TRT2API && dotnet run`
* Query an endpoint on the running port: `GET https://localhost:44395/api/players/all` for example

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
    "matchID": 99999,
    "player1ID": 12345,
    "player2ID": 123456,
    "winnerID": 12345
  },
  {
    "id": 2,
    "matchID": 100000,
    "player1ID": 1234567,
    "player2ID": 12345,
    "winnerID": 1234567
  }
]
```

This shows that player 12345 has been involved in two matches, one where they were Player 1 and won, and another where they were Player 2 and lost.
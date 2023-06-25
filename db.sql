CREATE TABLE players
(
    id           SERIAL
        PRIMARY KEY,
    osuplayerid     BIGINT
        UNIQUE,
    name         VARCHAR(100),
    totalmatches INTEGER,
    totalwins    INTEGER,
    status       VARCHAR(50),
    iseliminated BOOLEAN,
    seeding      INTEGER
);

CREATE TABLE matches
(
    id             SERIAL
        PRIMARY KEY,
    osumatchid        BIGINT,
    winnerid       BIGINT,
    timestart      TIMESTAMP,
    lastupdated    TIMESTAMP,
    type           VARCHAR(50),
    scheduleid     INTEGER,
    bracketmatchid INTEGER
);

CREATE TABLE match_players
(
    id          SERIAL
        PRIMARY KEY,
    matchid     INTEGER
        REFERENCES matches,
    playerid    INTEGER
        REFERENCES players,
    score INTEGER,
    iswinner    BOOLEAN
);

CREATE TABLE schedule
(
    id          SERIAL
        PRIMARY KEY,
    title       VARCHAR(200),
    description TEXT,
    type        TEXT,
    image       TEXT,
    priority    SMALLINT,
    link        TEXT,
    timestamp   TIMESTAMP
);

CREATE TABLE maps
(
    id        SERIAL
        PRIMARY KEY,
    osumapid     BIGINT           NOT NULL
        UNIQUE,
    round     TEXT             NOT NULL,
    mod       TEXT             NOT NULL,
    postmodsr DOUBLE PRECISION NOT NULL,
    metadata  TEXT
);

CREATE TABLE match_maps
(
    id           SERIAL
        PRIMARY KEY,
    matchid      INTEGER     NOT NULL
        REFERENCES matches,
    mapid        INTEGER     NOT NULL
        REFERENCES maps,
    playerid     INTEGER     NOT NULL
        REFERENCES players,
    action       VARCHAR(50) NOT NULL,
    orderinmatch INTEGER     NOT NULL
);

create table rounds
(
    id     SERIAL
        PRIMARY KEY,
    name   VARCHAR(20) NOT NULL,
    bestof INTEGER     NOT NULL
);

CREATE TABLE players (
    id SERIAL PRIMARY KEY,
    player_id BIGINT UNIQUE,
    player_name VARCHAR(100),
    total_wins INT,
    total_losses INT,
    status VARCHAR(50),
    is_eliminated BOOLEAN,
    seeding INT
);

CREATE TABLE matches (
    id SERIAL PRIMARY KEY,
    match_id BIGINT,
    winner_id BIGINT,
    time_start TIMESTAMP,
    last_updated TIMESTAMP,
    match_type VARCHAR(50), -- "main_tournament", "battle_royale", "final"
    schedule_id INT,
    bracket_match_id INT
);

CREATE TABLE match_players (
    id SERIAL PRIMARY KEY,
    match_id INT REFERENCES matches(id),
    player_id INT REFERENCES players(id),
    player_score INT,
    is_winner BOOLEAN
);

CREATE TABLE schedule (
    id SERIAL PRIMARY KEY,
    title VARCHAR(200),
    description TEXT,
    type TEXT,
    image TEXT,
    priority SMALLINT,
    link TEXT,
    timestamp TIMESTAMP
);

CREATE TABLE maps (
    id SERIAL PRIMARY KEY,
    map_id BIGINT UNIQUE NOT NULL,
    round TEXT NOT NULL,
    mod TEXT NOT NULL,
    post_mod_sr DOUBLE PRECISION NOT NULL,
    metadata JSONB NOT NULL
);

CREATE TABLE match_maps (
    id SERIAL PRIMARY KEY,
    match_id INT REFERENCES matches(id),
    map_id INT REFERENCES maps(id),
    player_id INT REFERENCES players(id),
    action VARCHAR(50), -- "picked" or "banned"
    order_in_match INT
);

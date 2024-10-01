CREATE TABLE IF NOT EXISTS messages (
    id SERIAL PRIMARY KEY,
    messagetext VARCHAR(128) NOT NULL,
    messageTimestamp TIMESTAMP,
    MessageIndex INTEGER NOT NULL
    CONSTRAINT chk_positive_index CHECK (MessageIndex >= 0)
)
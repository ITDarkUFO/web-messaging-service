CREATE TABLE IF NOT EXISTS messages (
    id SERIAL PRIMARY KEY,
    messagetext TEXT NOT NULL
	CONSTRAINT chk_text_message_length CHECK (char_length(messagetext) <= 128),
    messageTimestamp TIMESTAMP with time zone,
    MessageIndex INTEGER NOT NULL
    CONSTRAINT chk_positive_index CHECK (MessageIndex >= 0)
)
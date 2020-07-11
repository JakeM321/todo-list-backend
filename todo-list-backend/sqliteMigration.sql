CREATE TABLE Users (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	Email TEXT,
	DisplayName TEXT,
	PasswordHash TEXT,
	Salt TEXT,
	SsoUser INTEGER
);

INSERT INTO Users VALUES (1, 'admin@admin.com', 'Super user', '\u002BKjsGU6qjAoIHzpNJm5iUfYkNtwW00Zy1L6v6kg38B8=', 'I7J5RhwbXbWlUKxad2algg==', 0);
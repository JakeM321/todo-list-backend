CREATE TABLE Users (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	Email TEXT,
	DisplayName TEXT,
	PasswordHash TEXT,
	Salt TEXT,
	SsoUser INTEGER
);

CREATE TABLE Notifications (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER,
    Subject TEXT,
    Message TEXT,
    isLink INTEGER,
    Link TEXT,
    Seen INTEGER
);
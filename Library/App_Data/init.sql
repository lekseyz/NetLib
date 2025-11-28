CREATE TABLE IF NOT EXISTS Clients (
    Id               TEXT PRIMARY KEY,
    Name             TEXT NOT NULL,
    PassportId       TEXT NOT NULL,
    RegistrationDate TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Books (
    Isbn        TEXT PRIMARY KEY,
    Title       TEXT NOT NULL,
    Author      TEXT NOT NULL,
    Description TEXT,
    Amount      INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS Issues (
    Id          TEXT PRIMARY KEY,
    UserId      TEXT NOT NULL,
    Isbn        TEXT NOT NULL,
    IssueDate   TEXT NOT NULL,
    DueDate     TEXT NOT NULL,
    ReturnDate  TEXT NULL,
    FOREIGN KEY (UserId)   REFERENCES Clients(Id),
    FOREIGN KEY (Isbn)     REFERENCES Books(Isbn)
);
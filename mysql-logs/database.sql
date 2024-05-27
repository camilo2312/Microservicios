CREATE TABLE logs (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Application VARCHAR(255),
    LogType VARCHAR(255),
    Module VARCHAR(255),
    Timestamp DATETIME,
    Summary VARCHAR(255),
    Description VARCHAR(255)
);
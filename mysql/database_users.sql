CREATE TABLE Users (
    cedula varchar(20) not null,
    nombre varchar(100),
    apellido varchar(100),
    email varchar(100),
    contrasena varchar(10),
    PRIMARY KEY (cedula)
);


INSERT INTO Users VALUES ('1005308685', 'Camilo', 'Ramos', 'camiramos234@gmail.com', '1234');
INSERT INTO Users VALUES ('222', 'Daniel', 'Tusarma', 'daniel@gmail.com', '1234');
INSERT INTO Users VALUES ('333', 'Pepe', 'Perez', 'pepe@gmail.com', '1234');
INSERT INTO Users VALUES ('987654321', 'John', 'Doe', 'johndoe@gmail.com', '1234');
INSERT INTO Users VALUES ('123456789', 'Jane', 'Smith', 'janesmith@gmail.com', '1234');
-- INSERT INTO Users VALUES ('456789123', 'Michael', 'Johnson', 'michaeljohnson@gmail.com', '1234');
-- INSERT INTO Users VALUES ('789123456', 'Emily', 'Davis', 'emilydavis@gmail.com', '1234');
-- INSERT INTO Users VALUES ('321654987', 'Daniel', 'Martinez', 'danielmartinez@gmail.com', '1234');
-- INSERT INTO Users VALUES ('654987321', 'Sophia', 'Brown', 'sophiabrown@gmail.com', '1234');
-- INSERT INTO Users VALUES ('987321654', 'David', 'Taylor', 'davidtaylor@gmail.com', '1234');
-- INSERT INTO Users VALUES ('159753468', 'Olivia', 'Miller', 'oliviamiller@gmail.com', '1234');
-- INSERT INTO Users VALUES ('753159852', 'Emma', 'Wilson', 'emmawilson@gmail.com', '1234');
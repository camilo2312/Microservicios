CREATE TABLE user_profile (
    UserId INT AUTO_INCREMENT PRIMARY KEY,
    PersonalPageUrl VARCHAR(300),
    Nickname VARCHAR(45),
    IsContactInfoPublic VARCHAR(45),
    MailingAddress VARCHAR(45),
    Biography VARCHAR(200),
    Organization VARCHAR(45),
	Country VARCHAR(45),
	SocialLinks LONGTEXT,
	Id VARCHAR(45)
);
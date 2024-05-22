namespace CMPE344.Services;

public static class ConstantProcedures
{
	public const string CreateCustomerProcedure = @$"USE `{Database.DATABASE_NAME}`;
DROP PROCEDURE IF EXISTS `CreateCustomer`;

CREATE PROCEDURE `CreateCustomer` (IN `email` VARCHAR(255), IN `password` VARCHAR(255), IN `first_name` VARCHAR(255), IN `last_name` VARCHAR(255), IN `address` VARCHAR(255), IN `phone_number` VARCHAR(15))
BEGIN
	DECLARE `user_id` INT;
    DECLARE `preference_id` INT;
    
	IF `email` IS NOT NULL AND `password` IS NOT NULL AND `first_name` IS NOT NULL AND `last_name` IS NOT NULL THEN
		INSERT INTO `user` (`Email`, `Password`, `FirstName`, `LastName`, `Address`, `PhoneNumber`, `Role`) VALUES (`email`, `password`, `first_name`, `last_name`, `address`, `phone_number`, ""Customer"");
		SELECT last_insert_id() INTO `user_id`;
		
		INSERT INTO `preference` VALUES ();
		SELECT last_insert_id() INTO `preference_id`;
		
		INSERT INTO `customer` (`PreferenceId`, `UserId`) VALUES (`preference_id`, `user_id`);
		
		SET @sql = CONCAT(""CREATE USER 'User_"", `user_id`, ""'@'localhost' IDENTIFIED WITH mysql_native_password BY "", QUOTE(`password`), ""DEFAULT ROLE 'customer'"");

		PREPARE `stmt` FROM @sql;
		EXECUTE `stmt`;
		DEALLOCATE PREPARE `stmt`;
	END IF;
END";

	public const string CreateTAProcedure = @$"USE `{Database.DATABASE_NAME}`;
DROP PROCEDURE IF EXISTS `CreateTA`;

CREATE PROCEDURE `CreateTA` (IN `email` VARCHAR(255), IN `password` VARCHAR(255), IN `first_name` VARCHAR(255), IN `last_name` VARCHAR(255), IN `address` VARCHAR(255), IN `phone_number` VARCHAR(15), IN `agency_name` VARCHAR(255), IN `commission_rate` DOUBLE)
BEGIN
	DECLARE `user_id` INT;
    
	IF `email` IS NOT NULL AND `password` IS NOT NULL AND `first_name` IS NOT NULL AND `last_name` IS NOT NULL AND `agency_name` IS NOT NULL AND `commission_rate` IS NOT NULL THEN
		INSERT INTO `user` (`Email`, `Password`, `FirstName`, `LastName`, `Address`, `PhoneNumber`, `Role`) VALUES (`email`, `password`, `first_name`, `last_name`, `address`, `phone_number`, ""Travel Agent"");
		SELECT last_insert_id() INTO `user_id`;
		
		INSERT INTO `travel_agent` (`AgencyName`, `CommissionRate`, `UserId`) VALUES (`agency_name`, `commission_rate`, `user_id`);
		
		SET @sql = CONCAT(""CREATE USER 'User_"", `user_id`, ""'@'localhost' IDENTIFIED WITH mysql_native_password BY "", QUOTE(`password`), "" DEFAULT ROLE 'travel_agent'"");

		PREPARE `stmt` FROM @sql;
		EXECUTE `stmt`;
		DEALLOCATE PREPARE `stmt`;
	END IF;
END";

	public const string GetUserDetails2Procedure = @$"USE `{Database.DATABASE_NAME}`;
DROP PROCEDURE IF EXISTS `GetUserDetails2`;

CREATE PROCEDURE `GetUserDetails2` (IN `userId` INT)
BEGIN
SELECT * FROM `user`
	LEFT JOIN `customer` USING (`UserId`)
    LEFT JOIN `preference` USING (`PreferenceId`)
    LEFT JOIN `travel_agent` USING (`UserId`)
    WHERE `user`.`UserId` = `userId`
    LIMIT 1;
END";

	public const string GetUserDetailsProcedure = @$"USE `{Database.DATABASE_NAME}`;
DROP PROCEDURE IF EXISTS `GetUserDetails`;

CREATE PROCEDURE `GetUserDetails` (IN `email` VARCHAR(255), IN `password` VARCHAR(255))
BEGIN
SELECT * FROM `user`
	LEFT JOIN `customer` USING (`UserId`)
    LEFT JOIN `preference` USING (`PreferenceId`)
    LEFT JOIN `travel_agent` USING (`UserId`)
    WHERE `user`.`Email` = `email` AND `user`.`Password` = `password`
    LIMIT 1;
END";

    public const string PrepareRolesProcedure = @$"USE `{Database.DATABASE_NAME}`;
DROP PROCEDURE IF EXISTS `PrepareRoles`;

CREATE PROCEDURE `PrepareRoles` ()
BEGIN
DROP ROLE IF EXISTS 'app_developer', 'customer', 'travel_agent';

CREATE ROLE 'app_developer', 'customer', 'travel_agent';
GRANT ALL ON `{Database.DATABASE_NAME}`.* TO 'app_developer';
GRANT SELECT, INSERT, UPDATE, DELETE ON `{Database.DATABASE_NAME}`.* TO 'customer', 'travel_agent';

DROP USER IF EXISTS 'dev1';
CREATE USER 'dev1' IDENTIFIED WITH mysql_native_password BY '15E2B0D3C33891EBB0F1EF609EC419420C20E320CE94C65FBC8C3312448EB225' DEFAULT ROLE 'app_developer'; # SHA256 Hash of '123456789'
END";

    public const string PrepareTablesProcedure = @$"USE `{Database.DATABASE_NAME}`;
DROP PROCEDURE IF EXISTS `PrepareTables`;

CREATE PROCEDURE `PrepareTables` ()
BEGIN

SET foreign_key_checks = 0;

DROP TABLE IF EXISTS
	`preference`, `user`, `customer`, `travel_agent`, `hotel`, `flight`, `tour`, `customer_tour_buy`;
    
SET foreign_key_checks = 1;

CREATE TABLE `preference` (
	`PreferenceId` INT NOT NULL AUTO_INCREMENT,
    `EmailPreference` TINYINT NOT NULL DEFAULT '0',
    `PhonePreference` TINYINT NOT NULL DEFAULT '0',
    PRIMARY KEY (`PreferenceId`)
);

CREATE TABLE `user` (
	`UserId` INT NOT NULL AUTO_INCREMENT,
    `Email` VARCHAR(255) NOT NULL,
    `Password` VARCHAR(255) NOT NULL,
    `FirstName` VARCHAR(255) NOT NULL,
    `LastName` VARCHAR(255) NOT NULL,
    `Address` VARCHAR(255) NULL,
    `PhoneNumber` VARCHAR(15) NULL,
    `Role` VARCHAR(20) NOT NULL DEFAULT 'Customer',
    PRIMARY KEY (`UserId`),
    UNIQUE INDEX `Email_UNIQUE` (`Email` ASC) VISIBLE,
    CONSTRAINT `Check_Role` CHECK (`Role` IN ('Customer', 'Travel Agent')));

CREATE TABLE `customer` (
	`CustomerId` INT NOT NULL AUTO_INCREMENT,
    `PreferenceId` INT NOT NULL,
    `UserId` INT NOT NULL,
    PRIMARY KEY (`CustomerId`),
    INDEX `FK_Customer_Preference_idx` (`PreferenceId` ASC) VISIBLE,
    INDEX `FK_Customer_User_idx` (`UserId` ASC) VISIBLE,
    CONSTRAINT `FK_Customer_Preference`
		FOREIGN KEY (`PreferenceId`)
        REFERENCES `preference` (`PreferenceId`),
    CONSTRAINT `FK_Customer_User`
		FOREIGN KEY (`UserId`)
        REFERENCES `user` (`UserId`)
	);

CREATE TABLE `travel_agent` (
	`AgentId` INT NOT NULL AUTO_INCREMENT,
    `AgencyName` VARCHAR(255) NOT NULL,
    `CommissionRate` DOUBLE NOT NULL,
    `UserId` INT NULL,
    PRIMARY KEY (`AgentId`),
    INDEX `AgencyName_UNIQUE` (`AgencyName` ASC) VISIBLE,
    INDEX `FK_Travel_Agent_User_idx` (`UserId` ASC) VISIBLE,
    CONSTRAINT `FK_Travel_Agent_User`
		FOREIGN KEY (`UserId`)
        REFERENCES `user` (`UserId`)
	);

CREATE TABLE `hotel` (
	`HotelId` INT NOT NULL AUTO_INCREMENT,
    `HotelName` VARCHAR(255) NOT NULL,
    `Location` VARCHAR(255) NOT NULL,
    PRIMARY KEY (`HotelId`));
    
CREATE TABLE `flight` (
	`FlightId` INT NOT NULL AUTO_INCREMENT,
    `Origin` VARCHAR(255) NOT NULL,
    `Destination` VARCHAR(255) NOT NULL,
    `Airline` VARCHAR(255) NOT NULL,
    `DepartureTime` DATETIME NOT NULL,
    `ArrivalTime` DATETIME NOT NULL,
    PRIMARY KEY (`FlightId`));

CREATE TABLE `tour` (
	`TourId` INT NOT NULL AUTO_INCREMENT,
	`Title` VARCHAR(45) NOT NULL,
    `Description` VARCHAR(1023) NULL,
    `StartDate` DATETIME NOT NULL,
    `EndDate` DATETIME NOT NULL,
    `Capacity` INT NOT NULL DEFAULT 0,
    `Price` DOUBLE NOT NULL,
    `HotelId` INT NOT NULL,
    `FlightId` INT NOT NULL,
    `CreatedBy` INT NOT NULL,
    PRIMARY KEY (`TourId`),
    INDEX `FK_Tour_Hotel_idx` (`HotelId` ASC) VISIBLE,
    INDEX `FK_Tour_Flight_idx` (`FlightId` ASC) VISIBLE,
    INDEX `FK_Tour_Travel_Agent_idx` (`CreatedBy` ASC) VISIBLE,
    CONSTRAINT `FK_Tour_Hotel`
		FOREIGN KEY (`HotelId`) 
		REFERENCES `hotel` (`HotelId`),
    CONSTRAINT `FK_Tour_Flight`
		FOREIGN KEY (`FlightId`)
		REFERENCES `flight` (`FlightId`),
    CONSTRAINT `FK_Tour_Travel_Agent`
		FOREIGN KEY (`CreatedBy`)
		REFERENCES `travel_agent` (`AgentId`)
);

CREATE TABLE `customer_tour_buy` (
	`CustomerTourBuyId` INT NOT NULL AUTO_INCREMENT,
    `CustomerId` INT NOT NULL,
    `TourId` INT NOT NULL,
    `ApplyDate` DATETIME NOT NULL,
    PRIMARY KEY (`CustomerTourBuyId`),
    INDEX `FK_CustomerTour_Customer_idx` (`CustomerId` ASC) VISIBLE,
    INDEX `FK_CustomerTour_Tour_idx` (`TourId` ASC) VISIBLE,
    CONSTRAINT `FK_CustomerTour_Customer`
		FOREIGN KEY (`CustomerId`)
        REFERENCES `customer` (`CustomerId`),
	CONSTRAINT `FK_CustomerTour_Tour`
		FOREIGN KEY (`TourId`)
        REFERENCES `tour` (`TourId`));
END";

    public const string TourWithDetailsView = @$"USE `{Database.DATABASE_NAME}`;
CREATE OR REPLACE VIEW `TourWithDetails` AS
WITH `AppliedTours` AS (SELECT `TourId`, COUNT(*) AS `Applied` FROM `customer_tour_buy` GROUP BY `TourId`)
SELECT `TourId`, `Title`, `Description`, `StartDate`, `EndDate`, `Capacity`, `Price`, `HotelId`, `FlightId`, `CreatedBy`, COALESCE(`Applied`, 0) AS `Applied` FROM `tour` LEFT JOIN `AppliedTours` USING (`TourId`);";

    public const string UpdateCustomerProcedure = @$"USE `{Database.DATABASE_NAME}`;
DROP PROCEDURE IF EXISTS `UpdateCustomer`;

CREATE PROCEDURE `UpdateCustomer` (IN `userId` INT, IN `address` VARCHAR(255), IN `email` VARCHAR(255), IN `firstName` VARCHAR(255), IN `lastName` VARCHAR(255), IN `phoneNumber` VARCHAR(255), IN `emailPreference` TINYINT, IN `phonePreference` TINYINT)
BEGIN
	UPDATE `user` SET `Address` = `address`, `Email` = `email`, `FirstName` = `firstName`, `LastName` = `lastName`, `PhoneNumber` = `phoneNumber`
		WHERE `UserId` = `userId`;
	UPDATE `preference` SET `Email` = `emailPreference`, `Phone` = `phonePreference`
		WHERE `PreferenceId` = (SELECT `PreferenceId` FROM `customer` WHERE `UserId` = `userId`);
END";

    public const string UpdateTravelAgentProcedure = @$"USE `{Database.DATABASE_NAME}`;
DROP PROCEDURE IF EXISTS `UpdateTravelAgent`;

CREATE PROCEDURE `UpdateTravelAgent` (IN `userId` INT, IN `address` VARCHAR(255), IN `email` VARCHAR(255), IN `firstName` VARCHAR(255), IN `lastName` VARCHAR(255), IN `phoneNumber` VARCHAR(255), IN `agencyName` VARCHAR(255), IN `commissionRate` DOUBLE)
BEGIN
	UPDATE `user` SET `Address` = `address`, `Email` = `email`, `FirstName` = `firstName`, `LastName` = `lastName`, `PhoneNumber` = `phoneNumber`
		WHERE `UserId` = `userId`;
	
    UPDATE `travel_agent` SET `AgencyName` = `agencyName`, `CommissionRate` = `commissionRate`
		WHERE `UserId` = `userId`;
END";

    public const string TourBeforeDeleteTrigger = @$"DROP TRIGGER IF EXISTS `{Database.DATABASE_NAME}`.`tour_BEFORE_DELETE`;

CREATE DEFINER = CURRENT_USER TRIGGER `{Database.DATABASE_NAME}`.`tour_BEFORE_DELETE` BEFORE DELETE ON `tour` FOR EACH ROW
BEGIN
DELETE FROM `{Database.DATABASE_NAME}`.`customer_tour_buy` WHERE `TourId` = OLD.`TourId`;
END";

    public const string TourAfterDeleteTrigger = @$"DROP TRIGGER IF EXISTS `{Database.DATABASE_NAME}`.`tour_AFTER_DELETE`;

CREATE DEFINER = CURRENT_USER TRIGGER `{Database.DATABASE_NAME}`.`tour_AFTER_DELETE` AFTER DELETE ON `tour` FOR EACH ROW
BEGIN
DELETE FROM `{Database.DATABASE_NAME}`.`hotel` WHERE `HotelId` = OLD.`HotelId`;
DELETE FROM `{Database.DATABASE_NAME}`.`flight` WHERE `FlightId` = OLD.`FlightId`;
END";
}
CREATE TABLE `dustsensor` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Dev_id` varchar(45) DEFAULT NULL,
  `Name` varchar(45) DEFAULT NULL,
  `Loc` varchar(45) DEFAULT NULL,
  `Coordx` double DEFAULT NULL,
  `Coordy` double DEFAULT NULL,
  `Ison` bit(1) DEFAULT NULL,
  `Pm10_after` int DEFAULT NULL,
  `Pm25_after` int DEFAULT NULL,
  `State` int DEFAULT NULL,
  `Timestamp` datetime DEFAULT NULL,
  `Company_id` varchar(45) DEFAULT NULL,
  `Company_name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`Id`)
);

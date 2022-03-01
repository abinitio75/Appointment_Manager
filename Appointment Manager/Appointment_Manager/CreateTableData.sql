/*populate table `country`*/
INSERT INTO `country` VALUES 
(1,'US','2022-02-10 00:08:00','test','2022-02-10 00:08:00','test'),
(2,'Canada','2022-02-10 00:08:00','test','2022-02-10 00:08:00','test'),
(3,'Norway','2022-02-10 00:08:00','test','2022-02-10 00:08:00','test');

/*populate table `city`*/
INSERT INTO `city` VALUES 
(1,'New York',1,'2022-02-10 00:08:00','test','2022-02-10 00:08:00','test'),
(2,'Los Angeles',1,'2022-02-10 00:08:00','test','2022-02-10 00:08:00','test'),
(3,'Toronto',2,'2022-02-10 00:08:00','test','2022-02-10 00:08:00','test'),
(4,'Vancouver',2,'2022-02-10 00:08:00','test','2022-02-10 00:08:00','test'),
(5,'Oslo',3,'2022-02-10 00:08:00','test','2022-02-10 00:08:00','test');

/*populate table `address`*/
INSERT INTO `address` VALUES 
(1,'123 Main','',1,'11111','555-1212','2022-02-10 00:08:00','test','2022-02-10 00:08:00','test'),
(2,'123 Elm','',3,'11112','555-1213','2022-02-10 00:08:00','test','2022-02-10 00:08:00','test'),
(3,'123 Oak','',5,'11113','555-1214','2022-02-10 00:08:00','test','2022-02-10 00:08:00','test');

/*populate table `customer`*/
INSERT INTO `customer` VALUES 
(1,'John Doe',1,1,'2022-02-10 00:08:00','test','2022-02-10 00:08:00','test'),
(2,'Alfred E Newman',2,1,'2022-02-10 00:08:00','test','2022-02-10 00:08:00','test'),
(3,'Ina Prufung',3,1,'2022-02-10 00:08:00','test','2022-02-10 00:08:00','test');

/*populate table `user`*/
INSERT INTO `user` VALUES 
(1,'test',SHA2('test',0),1,'2022-02-10 00:08:00','test','2022-02-10 00:08:00','test');

/*populate table `appointment`*/
INSERT INTO `appointment` VALUES 
(1,1,1,'not needed','prospective','not needed','not needed','Consultation','not needed','2022-02-28 14:00:00','2022-02-28 14:30:00','2022-02-10 00:08:00','test','2022-02-10 00:08:00','test'),
(2,2,1,'not needed','recurring','not needed','not needed','Sales','not needed','2022-03-04 16:10:00','2022-03-04 16:45:00','2022-02-10 00:08:00','test','2022-02-10 00:08:00','test');

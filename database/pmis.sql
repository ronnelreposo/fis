# Host: localhost  (Version 5.5.5-10.1.16-MariaDB)
# Date: 2017-10-07 16:36:51
# Generator: MySQL-Front 5.4  (Build 1.38)

/*!40101 SET NAMES utf8 */;

#
# Structure for table "faculty"
#

DROP TABLE IF EXISTS `faculty`;
CREATE TABLE `faculty` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `school_id` varchar(6) COLLATE utf8_bin NOT NULL DEFAULT '',
  `first_name` varchar(30) COLLATE utf8_bin NOT NULL DEFAULT 'Not Available',
  `middle_name` varchar(30) COLLATE utf8_bin DEFAULT 'Not Available',
  `last_name` varchar(30) COLLATE utf8_bin DEFAULT 'Not Available',
  `age` varchar(2) COLLATE utf8_bin NOT NULL DEFAULT '20',
  `address` varchar(50) COLLATE utf8_bin NOT NULL DEFAULT 'Not Available',
  `cellphone` varchar(10) COLLATE utf8_bin NOT NULL DEFAULT '0000000000',
  `date_of_birth` varchar(10) COLLATE utf8_bin DEFAULT NULL,
  `place_of_birth` varchar(50) COLLATE utf8_bin NOT NULL DEFAULT 'Not Available',
  `date_hired` varchar(30) COLLATE utf8_bin NOT NULL DEFAULT '',
  `current_status` varchar(30) COLLATE utf8_bin NOT NULL DEFAULT 'Not Available',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

#
# Data for table "faculty"
#

INSERT INTO `faculty` VALUES (1,'1','firstname','middlename','lastname','1','address','0987654321','10/5/2017','place of birth','10/5/2017','status');

#
# Structure for table "faculty_accounts"
#

DROP TABLE IF EXISTS `faculty_accounts`;
CREATE TABLE `faculty_accounts` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `employee_id` varchar(6) COLLATE utf8_bin NOT NULL DEFAULT '',
  `password` varchar(88) COLLATE utf8_bin NOT NULL DEFAULT '',
  `salt` varchar(32) COLLATE utf8_bin NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

#
# Data for table "faculty_accounts"
#

INSERT INTO `faculty_accounts` VALUES (1,'jon','ulSJjVe+G0BllFx5uuaiD4v1gx4lZeaLY2IhpPayQVbdvjfk7DEkGsItDWwNp0JHCQEVeR2e16FjOmSgVUJ7kw==','f2c7f4cc705a4ca38065919251db8fb9'),(2,'sansa','sJGRGKny4h6DWgmSNoZLrivopTnhIe56k8QySTKsoHHlumhv43b1AiCdl7AdtYb0ZyjnbFQt8Q7qw+SvS8ua0g==','0aaa3e7c5205488d8e3784443f2985cd'),(3,'arya','VFMJ9jtKA+hUV7ZK2sRtD5ttrl39DpwNweXKqvcz2OeAWbJ5nSdbCMz+q3FQpl0+IN9Xg19WTtqnEsKEhs6AlQ==','31e1e1a35da0428687d8bf3ea5586ff6');

#
# Procedure "add_faculty"
#

DROP PROCEDURE IF EXISTS `add_faculty`;
CREATE PROCEDURE `add_faculty`(_school_id varchar(6), _first_name varchar(30), _middle_name varchar(30), _last_name varchar(30), _age varchar(2), _address varchar(50), _cellphone varchar(10), _date_of_birth varchar(10), _place_of_birth varchar(50), _date_hired varchar(30), _current_status varchar(30))
BEGIN
        insert into faculty set school_id=_school_id, first_name=_first_name, middle_name=_middle_name, last_name=_last_name, age=_age, address=_address, cellphone=_cellphone, date_of_birth=_date_of_birth, place_of_birth=_place_of_birth, date_hired=_date_hired, current_status=_current_status;
END;

#
# Procedure "add_faculty_account"
#

DROP PROCEDURE IF EXISTS `add_faculty_account`;
CREATE PROCEDURE `add_faculty_account`(_employee_id varchar(6), _password varchar(88), _salt varchar(32))
BEGIN
 insert into faculty_accounts set employee_id=_employee_id, password=_password, salt=_salt;
END;

#
# Procedure "get_faculty_account"
#

DROP PROCEDURE IF EXISTS `get_faculty_account`;
CREATE PROCEDURE `get_faculty_account`(_employee_id varchar(6))
BEGIN
 select * from faculty_accounts where employee_id=_employee_id limit 1;
END;

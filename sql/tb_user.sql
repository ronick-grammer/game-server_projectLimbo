CREATE TABLE `tb_user` (
  `user_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `kakao_id` varchar(200) DEFAULT '',
  `kakao_name` varchar(200) DEFAULT '',
  `kakao_photo_url` varchar(512) DEFAULT '',
  `point` int(11) DEFAULT '0',
  `access_token` varchar(200) DEFAULT '',
  `item` varchar(200) DEFAULT '',
  `play_hour` int(11) DEFAULT '0',
  `ranking` int(11) DEFAULT '0',
  `deleted` tinyint(2) DEFAULT '0',
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*
CREATE TABLE `tb_user` (
  `user_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `facebook_id` varchar(200) DEFAULT '',
  `facebook_name` varchar(200) DEFAULT '',
  `facebook_photo_url` varchar(512) DEFAULT '',
  `point` int(11) DEFAULT '0',
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `access_token` varchar(200) DEFAULT '',
  `diamon` int(11) DEFAULT '0',
  `health` int(11) DEFAULT '100',
  `defense` int(11) DEFAULT '0',
  `damage` int(11) DEFAULT '10',
  `speed` int(11) DEFAULT '10',
  `health_level` int(11) DEFAULT '1',
  `defense_level` int(11) DEFAULT '1',
  `damage_level` int(11) DEFAULT '1',
  `speed_level` int(11) DEFAULT '1',
  `level` int(11) DEFAULT '1',
  `experience` int(11) DEFAULT '0',
  `deleted` tinyint(2) DEFAULT '0',
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
*/
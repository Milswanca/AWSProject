<?php
	require_once('Puzzled.php');
	
	$sql = "CREATE TABLE Profiles (
	Username CHAR(10) NOT NULL,
	Greeting CHAR(100),
	DisplayPic INT NOT NULL,
	PRIMARY KEY (Username)
	);";
	
	$result = SafeQuery($pdo, $sql);
?>
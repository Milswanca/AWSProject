<?php
	require_once('Puzzled.php');
	
	$sql = "CREATE TABLE Highscores (
	Username CHAR(10) NOT NULL,
	Score INT(10) NOT NULL,
	PRIMARY KEY (Username)
	);";
	
	$result = SafeQuery($pdo, $sql);
?>
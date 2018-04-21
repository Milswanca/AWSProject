<?php
	require_once('Puzzled.php');
	
	$sql = "CREATE TABLE Login (
	Username CHAR(10) NOT NULL,
	Password CHAR(60) NOT NULL,
	PRIMARY KEY (Username)
	);";
	
	$result = SafeQuery($pdo, $sql);
?>
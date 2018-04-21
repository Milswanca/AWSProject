<?php
	require_once('Puzzled.php');
	
	$sql = "CREATE TABLE Friends (
	Requestee CHAR(10) NOT NULL,
	Requester CHAR(10) NOT NULL,
	Accepted BIT NOT NULL,
	PRIMARY KEY (Requestee)
	);";
	
	$result = SafeQuery($pdo, $sql);
?>
<?php
	require_once('Puzzled.php');
	
	$sql = "CREATE TABLE Friends (
	RequestID Int,
	Requestee CHAR(10) NOT NULL,
	Requester CHAR(10) NOT NULL,
	Accepted BIT NOT NULL,
	PRIMARY KEY (RequestID)
	);";
	
	$result = SafeQuery($pdo, $sql);
?>
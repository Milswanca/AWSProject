<?php
	require_once('Connect.php');
	require_once('Login.php');
	
	$friend = GetArg('Friend');
	
	$sql = 
	"SELECT * FROM Friends
	IF( Requestee = '$accountName' AND Requester = '$friend',
	
	UPDATE Friends SET Accepted = TRUE
	RETURN 'Accepted',
	
	INSERT INTO Friends(Requestee, Requester, Accepted)
	VALUES ('$friend', '$accountName', FALSE)
	RETURN 'Requested');";

	$result = SafeQuery($pdo, $sql);
?>
<?php
	require_once('Connect.php');
	require_once('Login.php');
	
	$friend = GetArg('Friend');
	$hash = GetArg('Hash');

	if(!CheckHash($accountUser . $accountPass . $friend, $hash))
	{
		trigger_error("Incorrect Hash", E_USER_WARNING);
	}

	if(!DoesUserExist($friend, $pdo))
	{
		trigger_error("User does not Exist", E_USER_WARNING);
	}
	
	//Check for an existing request
	$sqlCheckExisting = "SELECT * FROM Friends 
	WHERE Requester = '$friend' AND Requestee = '$accountUser'";
	
	$result = SafeQuery($pdo, $sqlCheckExisting);
	$rows = $result->fetch_all();

	if(Count($result) > 0)
	{
		//Update Friends if existing request is valid
		$sqlUpdateExisting = "UPDATE Friends
		SET Accepted = 1
		WHERE Requester = '$friend' AND Requestee = '$accountUser'";

		$result = SafeQuery($pdo, $sqlUpdateExisting);
	}
	else
	{
		//New Friend Request
		$sqlNewRequest = "INSERT INTO Friends(Requestee, Requester, Accepted)
		VALUES ('$friend', '$accountUser', 0)";

		$result = SafeQuery($pdo, $sqlNewRequest);
	}
?>
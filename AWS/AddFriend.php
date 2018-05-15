<?php
	require_once('Puzzled.php');
	
	$accountName = GetArg('Username');
	$accountPass = GetArg('Password');
	$friend 	 = GetArg('Friend');
	$hash 		 = GetArg('Hash');

	if(!CheckHash($accountName . $accountPass . $friend, $hash))
	{
		trigger_error("Incorrect Hash", E_USER_WARNING);
	}

	if(!CheckLogin($accountName, $accountPass, $pdo))
	{
		trigger_error("Failed to verify account", E_USER_WARNING);
	}

	if(!DoesUserExist($friend, $pdo))
	{
		//Delete the request
		$sql = "DELETE FROM Friends WHERE (Requestee = '$accountName' AND Requester = '$friend') OR (Requestee = '$friend' AND Requester = '$accountName')";
		SafeQuery($pdo, $sql);

		//Error
		trigger_error("User does not Exist", E_USER_WARNING);
	}
	
	//Check for an existing request
	$sqlCheckExisting = "SELECT * FROM Friends 
	WHERE Requester = '$friend' AND Requestee = '$accountName'";
	
	$result = SafeQuery($pdo, $sqlCheckExisting);
	$rows = $result->fetchAll();

	if(Count($rows) > 0)
	{
		//Update Friends if existing request is valid
		$sqlUpdateExisting = "UPDATE Friends
		SET Accepted = 1
		WHERE Requester = '$friend' AND Requestee = '$accountName'";

		SafeQuery($pdo, $sqlUpdateExisting);
	}
	else
	{
		//New Friend Request
		$sqlNewRequest = "INSERT INTO Friends(Requestee, Requester, Accepted)
		VALUES ('$friend', '$accountName', 0)";

		SafeQuery($pdo, $sqlNewRequest);
	}
?>
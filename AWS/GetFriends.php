<?php
	require_once('Puzzled.php');
	
	$me = GetArg('FromUser');
	$hash = GetArg('Hash');

	if(!CheckHash($me, $hash))
	{
		trigger_error("Incorrect Hash", E_USER_WARNING);
	}

	//Could most definitely be optimised but is fine for our uses
	//Pending
	$sqlPending = "SELECT Requester FROM Friends
	WHERE Requestee = '$me' AND Accepted = 0";

	//Unaccepted
	$sqlUnaccepted = "SELECT Requestee FROM Friends
	WHERE Requester = '$me' AND Accepted = 0";

	//Friends
	$sqlFriends = 
	"SELECT CASE
    	WHEN Requestee = '$me' THEN Requester
    	WHEN Requester = '$me' THEN Requestee
	END 
	FROM Friends
	WHERE Accepted = 1 AND ('$me' = Requestee OR '$me' = Requester)";
	 
	//Return pending
	$result = SafeQuery($pdo, $sqlPending);
	$pending = $result->fetchAll();
	for($i = 0; $i < Count($pending); $i += 1)
	{
    	echo "&Pending".$i."=".$pending[$i][0];
	}

	//Return unaccepted
	$result = SafeQuery($pdo, $sqlUnaccepted);
	$pending = $result->fetchAll();
	for($i = 0; $i < Count($pending); $i += 1)
	{
		echo "&Unaccepted".$i."=".$pending[$i][0];
	}

	//Return friends
	$result = SafeQuery($pdo, $sqlFriends);
	$pending = $result->fetchAll();
	for($i = 0; $i < Count($pending); $i += 1)
	{
		echo "&Friends".$i."=".$pending[$i][0];
	}
?>
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
	$sqlPending = "SELECT Requestee FROM Friends
	WHERE Requester = '$me' AND Accepted = 0";

	//Unaccepted
	$sqlUnaccepted = "SELECT Requester FROM Friends
	WHERE Requestee = '$me' AND Accepted = 0";
	 
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
	$result = GetFriends($me, $pdo);
	for($i = 0; $i < Count($result); $i += 1)
	{
		echo "&Friends".$i."=".$result[$i];
	}
?>
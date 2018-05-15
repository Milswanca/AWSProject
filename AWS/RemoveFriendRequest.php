<?php
	require_once('Puzzled.php');
	
	$accountUser = GetArg('Username');
	$accountPass = GetArg('Password');
	$friend = GetArg('Friend');
	$hash = GetArg('Hash');

	if(!CheckHash($accountUser . $accountPass . $friend, $hash))
	{
		trigger_error("Incorrect Hash", E_USER_WARNING);
	}

	if(!CheckLogin($accountUser, $accountPass, $pdo))
	{
		trigger_error("Failed to verify account", E_USER_WARNING);
	}
	
	//Check for an existing request
	$sql = "DELETE FROM Friends WHERE (Requestee = '$accountUser' AND Requester = '$friend') OR (Requestee = '$friend' AND Requester = '$accountUser')";
	SafeQuery($pdo, $sql);
?>
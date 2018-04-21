<?php
	require_once('Puzzled.php');

	$accountName = GetArg('Username');
	$accountPass = GetArg('Password');
	$newGreeting = GetArg('Greeting');
	$newDP 		 = GetArg('DisplayPic');
	$hash 		 = GetArg('Hash');

	if(!CheckHash($accountName . $accountPass . $newGreeting . $newDP, $hash))
	{
		trigger_error("Incorrect Hash", E_USER_WARNING);
	}

	if(!CheckLogin($accountName, $accountPass, $pdo))
	{
		trigger_error("Failed to verify account", E_USER_WARNING);
	}

	$sql = "UPDATE Profiles 
	SET Greeting = '$newGreeting', DisplayPic = $newDP 
	WHERE username = '$accountName'";

	$result = SafeQuery($pdo, $sql);
?>
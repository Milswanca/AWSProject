<?php
	require_once('Puzzled.php');
	
	$accountName = GetArg('Username');
	$accountPass = GetArg('Password');
	$hash 		 = GetArg('Hash');

	$password_hash = password_hash($accountPass, PASSWORD_BCRYPT);

	if(!CheckHash($accountName . $accountPass, $hash))
	{
		trigger_error("Incorrect Hash", E_USER_WARNING);
	}
	
	if(DoesUserExist($accountName, $pdo) == TRUE)
	{
		trigger_error("User Already Exists", E_USER_NOTICE);
	}
	else
	{
		$sqlLogin = "INSERT INTO Login SET Username = '$accountName', Password = '$password_hash'";
		SafeQuery($pdo, $sqlLogin);

		$sqlProfiles = "INSERT INTO Profiles SET Username = '$accountName', DisplayPic = 0";
		SafeQuery($pdo, $sqlProfiles);
	}
?>
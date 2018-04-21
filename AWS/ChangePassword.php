<?php
	require_once('Puzzled.php');

	$accountName = GetArg('Username');
	$accountPass = GetArg('Password');
	$newPass 	 = GetArg('NewPassword');
	$hash 		 = GetArg('Hash');

	if(!CheckHash($accountName . $accountPass . $newPass, $hash))
	{
		trigger_error("Incorrect Hash", E_USER_WARNING);
	}

	if(!CheckLogin($accountName, $accountPass, $pdo))
	{
		trigger_error("Failed to verify account", E_USER_WARNING);
	}

	$password_hash = password_hash($newPass, PASSWORD_BCRYPT);

	$sql = "UPDATE Login SET Password = '$password_hash'  WHERE username = '$accountName'";
	$result = SafeQuery($pdo, $sql);
?>
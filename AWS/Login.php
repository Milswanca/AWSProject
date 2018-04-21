<?php
	require_once('Puzzled.php');
	
	$accountName = GetArg('Username');
	$accountPass = GetArg('Password');
	$hash 		 = GetArg('Hash');

	if(!CheckHash($accountName . $accountPass, $hash))
	{
		trigger_error("Incorrect Hash", E_USER_WARNING);
	}

	if(!CheckLogin($accountName, $accountPass, $pdo))
	{
		trigger_error("Failed to verify account", E_USER_WARNING);
	}
?>
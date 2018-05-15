<?php
	require_once('Puzzled.php');
	
	$accountName = GetArg('Username');
	$accountPass = GetArg('Password');

	if(!CheckLogin($accountName, $accountPass, $pdo))
	{
		trigger_error("Failed to verify account", E_USER_WARNING);
	}
?>
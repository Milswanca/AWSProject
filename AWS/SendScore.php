<?php
	require_once('Puzzled.php');

	$accountName = GetArg('Username');
	$accountPass = GetArg('Password');
	$score 		 = GetArg('Score');
	$hash 		 = GetArg('Hash');

	if(!CheckHash($accountName . $accountPass . $score, $hash))
	{
		trigger_error("Incorrect Hash", E_USER_WARNING);
	}

	if(!CheckLogin($accountName, $accountPass, $pdo))
	{
		trigger_error("Failed to verify account", E_USER_WARNING);
	}

	$sql = "INSERT INTO Highscores (Username, Score) Values ('$accountName', $score)
	ON DUPLICATE KEY UPDATE Score = $score;";
				  
	$result = SafeQuery($pdo, $sql);
?>
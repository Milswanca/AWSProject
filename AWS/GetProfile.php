<?php
	require_once('Puzzled.php');

	$profileName = GetArg('ProfileName');
	$hash 		 = GetArg('Hash');

	if(!CheckHash($profileName, $hash))
	{
		trigger_error("Incorrect Hash", E_USER_WARNING);
	}

	$sql = "SELECT * FROM Profiles WHERE Username = '$profileName';";

	$result = SafeQuery($pdo, $sql);
 	$rows = $result->fetchAll();
 	
	if(count($rows) > 0)
	{
    	echo "&Username=".$rows[0]['Username'];
		echo "&Greeting=".$rows[0]['Greeting'];
		echo "&DisplayPic=".$rows[0]['DisplayPic'];
	}
?>
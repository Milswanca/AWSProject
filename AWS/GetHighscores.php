<?php
	require_once('Puzzled.php');
	
	$accountName = GetArg('Username');
	$scope 		 = GetArg('Scope');
	$hash 		 = GetArg('Hash');

	if(!CheckHash($accountName . $scope, $hash))
	{
		$expected = md5($accountName . $scope);
		trigger_error("Incorrect Hash", E_USER_WARNING);
	}

	$sql = null;

	if($scope == "Friends")
	{
		$sql = "SELECT * 
		FROM Highscores 
		INNER JOIN Friends ON (Highscores.Username = Friends.Requestee OR Highscores.Username = Friends.Requester) 
		WHERE Accepted = 1
		ORDER BY Score DESC LIMIT 10";
	}
	else
	{
		$sql = "SELECT * FROM Highscores ORDER BY Score DESC LIMIT 10";
	}

	$result = SafeQuery($pdo, $sql);
 	$rows = $result->fetchAll();
 	
	for($i = 0; $i < Count($rows); $i += 1)
	{
    	echo "&Username".$i."=".$rows[$i]['Username'];
		echo "&Score".$i."=".$rows[$i]['Score'];
	}
?>
<?php
	require_once('ErrorHandling.php');

	function DoesUserExist($user, $pdo)
	{
		$sql = "SELECT * FROM Login WHERE Username = '$user';";
		$result = SafeQuery($pdo, $sql);
		$rows = $result->fetchAll();

		if(count($rows) > 0)
		{
			return $rows[0]['Username'] == $user;
		}
		
		return FALSE;
	}

	function CheckHash($inData, $withHash)
	{
		$privateKey="pm36YVRuGh";
		$myHash = md5($inData . $privateKey);
		return $myHash == $withHash;
	}

	function CheckLogin($username, $password, $pdo)
	{
		$sql = "SELECT Password FROM Login WHERE Username = '$username'";
		$result = SafeQuery($pdo, $sql);
		$rows = $result->fetchAll();
	
		$hashedPass = null;
		if(Count($rows) > 0)
		{
			$hashedPass = $rows[0]['Password'];
		}
		
		return password_verify($password, $hashedPass);
	}

	function GetArg($arg)
	{
		return isset($_GET[$arg]) ? $_GET[$arg] : null;
	}

	function GetFriends($user, $pdo)
	{
		$sqlFriends = 
		"SELECT CASE
			WHEN Requestee = '$user' THEN Requester
			WHEN Requester = '$user' THEN Requestee
		END 
		FROM Friends
		WHERE Accepted = 1 AND ('$user' = Requestee OR '$user' = Requester)";

		//Return friends
		$result = SafeQuery($pdo, $sqlFriends);
		$pending = $result->fetchAll();
		$friends = array();
		for($i = 0; $i < Count($pending); $i += 1)
		{
			array_push($friends, $pending[$i][0]);
		}

		return $friends;
	}
?>
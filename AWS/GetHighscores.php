<?php
	require_once('Puzzled.php');
	
	$sql = "SELECT * FROM Highscores ORDER by Score DESC;";

	$result = SafeQuery($pdo, $sql);
 	$rows = $result->fetchAll();
 	
	for ($i = 0; $i < count($rows); $i++) 
	{
    	echo $rows[$i]['Username'];
    	echo $rows[$i]['Score'];
	}
?>
<?php
	$dbhost = $_SERVER['RDS_HOSTNAME'];
	$dbport = $_SERVER['RDS_PORT'];
	$dbname = $_SERVER['RDS_DB_NAME'];
	$charset = 'utf8' ;
	
	$dsn = "mysql:host={$dbhost};port={$dbport};dbname={$dbname};charset={$charset}";
	
	$serverUsername = $_SERVER['RDS_USERNAME'];
	$serverPassword = $_SERVER['RDS_PASSWORD'];
	$pdo = new PDO($dsn, $serverUsername, $serverPassword);

	if($pdo == NULL)
	{
		trigger_error("PDO Failed to Create", E_USER_ERROR);
	}
?>
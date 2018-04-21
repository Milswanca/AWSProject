<?php
    function CustomError($errno, $errstr) 
    {
        echo "<b>Error:</b> [$errno] $errstr \n";
        die();
    }

    function SafeQuery($pdo, $sql)
    {
        $result = NULL;
        try
        {
            $result = $pdo->query($sql);
        }
        catch(PDOException $e)
        {
            trigger_error('Message: ' .$e->getMessage());
        }

        return $result;
    }

    set_error_handler("CustomError");
?>
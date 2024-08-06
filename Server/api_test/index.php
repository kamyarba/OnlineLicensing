<?php

    // db of license keys and associated hardware IDs
$LICENSE_KEYS = [
    "VALID_LICENSE_KEY" => "123456",
    "Recorded_HW_ID" => "zxc"
];


// Route to validate a license key
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $license_key = @$_POST['license_key'];
    $hardware_id = @$_POST['hardware_id'];
    
    //Check license in database 
    if ($LICENSE_KEYS["VALID_LICENSE_KEY"] == $license_key) {
        echo json_encode(["valid" => "true"]);
        //Save in db
    } else {
        echo json_encode(["valid" => "false"]);
    }
    exit;
}

// Route to check a license status
if ($_SERVER['REQUEST_METHOD'] === 'GET') {
    
    $license_key = @$_GET['license_key'];
    $hardware_id = @$_GET['hardware_id'];

    if ($LICENSE_KEYS["VALID_LICENSE_KEY"] == $license_key) {
        echo json_encode(["License"=>$license_key,"status" => "active"]);
        //Save in db
    } else {
        echo json_encode(["License"=>$license_key,"status" => "inactive"]);
    }
    exit;
}

?>

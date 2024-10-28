<?php

function sayHello($name){
    echo "Hello $name\n";
}

$name = readline();
sayHello($name);
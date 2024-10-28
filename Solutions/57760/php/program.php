<?php
function daysUntilNowruz($date) {
    // Parse the given date (assuming format yyyy/mm/dd)
    list($year, $month, $day) = explode('/', $date);

    // Define the number of days in each month (ignoring leap years)
    $daysInMonth = [0, 31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29];

    // Calculate the total number of days left in the current year
    $daysLeftInCurrentYear = $daysInMonth[(int) $month] - $day;
    for ($i = $month + 1; $i <= 12; $i++) {
        $daysLeftInCurrentYear += $daysInMonth[$i];
    }

    $totalDays = $daysLeftInCurrentYear + 1;

    return $totalDays;
}

$date = readline();
echo daysUntilNowruz($date);

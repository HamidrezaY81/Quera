// Get total number of inputs
const t = Number(readline());

// Save each input result
let results = "";

// Get input total t times
for (let _ = 0; _ < t; _++) {
  // Get id from std input
  const nationalID = readline();

  let multiplier = 10;
  let unknownMultiplier = null;
  let c = 0;

  // Loop through s1 to s9
  for (let i = 0; i < 9; i++) {
    const char = nationalID[i];

    if (char === "?") {
      unknownMultiplier = multiplier;
    } else {
      c += Number(char) * multiplier;
    }

    multiplier -= 1;
  }

  // Save last digit for control
  const s10 = "?" === nationalID[9] ? "?" : Number(nationalID[9]);

  // If control digit is missing calculate base on c
  if (s10 === "?") {
    c = c % 11;
    if (c === 0 || c === 1) {
      results += `${nationalID.replace("?", c)}\n`;
    } else {
      results += `${nationalID.replace("?", 11 - c)}\n`;
    }

    continue;
  }

  // Loop through 0 to 9 for unknown digit and test
  let validDigits = [];
  for (let i = 0; i < 10; i++) {
    let tempC = (c + unknownMultiplier * i) % 11;
    if (tempC === 0 || tempC === 1) {
      if (s10 === tempC) {
        validDigits.push(i);
      }
    }

    if (tempC > 1 && tempC < 11) {
      if (s10 === 11 - tempC) {
        validDigits.push(i);
      }
    }
  }

  // Save result
  if (validDigits.length === 1) {
    results += `${nationalID.replace("?", validDigits[0])}\n`;
  } else if (validDigits.length > 1) {
    results += "it's not unique!\n";
  } else {
    results += "cannot be recovered!\n";
  }
}

// Print results
console.log(results);

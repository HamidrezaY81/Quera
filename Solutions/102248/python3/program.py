def compare(string1, string2):
    while string1 and string2:
        if string1[0] < string2[0]:
            string1 = string1[1:]
        elif string1[0] > string2[0]:
            string2 = string2[1:]
        else:
            string1, string2 = string1[1:], string2[1:]

        # Reverse both strings only if they are non-empty
        if string1 and string2:
            string1, string2 = string1[::-1], string2[::-1]

    # Handle the final result after the loop ends
    if not string1 and not string2:
        return "Both strings are empty!"
    return string1 or string2
